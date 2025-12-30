using System.Collections.ObjectModel;
using System.Windows.Input;
using BTL_QLDiem_HSPTTH.Data.Models;
using BTL_QLDiem_HSPTTH.Data.Services;
using BTL_QLDiem_HSPTTH.Services;
using System;
using System.Linq;

namespace BTL_QLDiem_HSPTTH.StudentVM
{
    public class BieudoVM : BindableObject
    {
        private readonly DatabaseService _db;
        private readonly UserSessionService _session;

        public ObservableCollection<Monhoc> DanhsachMon { get; set; }
        public ObservableCollection<Hocky> DanhsachHocky { get; set; }

        public Monhoc MonSelected { get; set; }
        public Hocky HockySelected { get; set; }

        // Dữ liệu cho biểu đồ
        public ObservableCollection<DiemData> DiemDataPoints { get; set; }

        public ICommand LoadDataCommand { get; }

        public class DiemData
        {
            public string Label { get; set; }
            public double Value { get; set; }
        }

        public BieudoVM()
        {
            _db = App.Current.Handler.MauiContext.Services.GetService<DatabaseService>();
            _session = App.Current.Handler.MauiContext.Services.GetService<UserSessionService>();

            DanhsachMon = new ObservableCollection<Monhoc>();
            DanhsachHocky = new ObservableCollection<Hocky>();
            DiemDataPoints = new ObservableCollection<DiemData>();

            LoadDataCommand = new Command(LoadData);

            LoadMon();
            LoadHocky();
        }

        void LoadMon()
        {
            DanhsachMon.Clear();
            var user = _session.CurrentUser;
            if (user?.HocsinhId.HasValue == true)
            {
                var hs = _db.GetHocsinhById(user.HocsinhId.Value);
                if (hs != null)
                {
                    var monIds = _db.GetPhancongByLop(hs.LophocId).Select(pc => pc.MonhocId).Distinct();
                    foreach (var monId in monIds)
                    {
                        var mon = _db.GetMonById(monId);
                        if (mon != null)
                            DanhsachMon.Add(mon);
                    }
                }
            }
        }

        void LoadHocky()
        {
            DanhsachHocky.Clear();
            var list = _db.GetAllHocky().OrderByDescending(h => h.IsActive).ThenByDescending(h => h.So);
            foreach (var item in list)
                DanhsachHocky.Add(item);
        }

        void LoadData()
        {
            var user = _session.CurrentUser;
            if (user?.HocsinhId.HasValue == false) return;

            DiemDataPoints.Clear();

            if (MonSelected != null)
            {
                // Biểu đồ điểm theo học kỳ cho 1 môn
                var hockys = _db.GetAllHocky().OrderBy(h => h.So);
                foreach (var hk in hockys)
                {
                    var diemTB = _db.TinhDiemTrungBinhMon(user.HocsinhId.Value, MonSelected.Id, hk.Id);
                    if (diemTB.HasValue)
                    {
                        DiemDataPoints.Add(new DiemData
                        {
                            Label = hk.Tenhocky,
                            Value = diemTB.Value
                        });
                    }
                }
            }
            else
            {
                // Biểu đồ điểm trung bình HK theo các môn
                var hs = _db.GetHocsinhById(user.HocsinhId.Value);
                if (hs != null && HockySelected != null)
                {
                    var monIds = _db.GetPhancongByLop(hs.LophocId).Select(pc => pc.MonhocId).Distinct();
                    foreach (var monId in monIds)
                    {
                        var mon = _db.GetMonById(monId);
                        if (mon != null)
                        {
                            var diemTB = _db.TinhDiemTrungBinhMon(user.HocsinhId.Value, monId, HockySelected.Id);
                            if (diemTB.HasValue)
                            {
                                DiemDataPoints.Add(new DiemData
                                {
                                    Label = mon.Tenmh,
                                    Value = diemTB.Value
                                });
                            }
                        }
                    }
                }
            }
        }
    }
}

