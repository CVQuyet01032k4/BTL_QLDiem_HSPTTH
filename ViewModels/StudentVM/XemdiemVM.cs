using System.Collections.ObjectModel;
using System.Windows.Input;
using BTL_QLDiem_HSPTTH.Data.Models;
using BTL_QLDiem_HSPTTH.Data.Services;
using BTL_QLDiem_HSPTTH.Services;
using BTL_QLDiem_HSPTTH.Helpers;
using System;
using System.Linq;
using static BTL_QLDiem_HSPTTH.Data.Models.Config;

namespace BTL_QLDiem_HSPTTH.StudentVM
{
    public class XemdiemVM : BindableObject
    {
        private readonly DatabaseService _db;
        private readonly UserSessionService _session;

        public Hocsinh Hocsinh { get; set; }
        public Lophoc Lophoc { get; set; }
        public ObservableCollection<DiemMonDisplay> DanhsachDiemMon { get; set; }
        public ObservableCollection<Monhoc> DanhsachMon { get; set; }
        public ObservableCollection<Hocky> DanhsachHocky { get; set; }
        public ObservableCollection<Namhoc> DanhsachNamhoc { get; set; }

        public Namhoc NamhocSelected { get; set; }
        public Hocky HockySelected { get; set; }

        public ICommand LoadDiemCommand { get; }

        public double? DiemTrungBinhHK { get; private set; }
        public string HocLuc { get; private set; }

        public XemdiemVM()
        {
            _db = App.Current.Handler.MauiContext.Services.GetService<DatabaseService>();
            _session = App.Current.Handler.MauiContext.Services.GetService<UserSessionService>();

            DanhsachDiemMon = new ObservableCollection<DiemMonDisplay>();
            DanhsachMon = new ObservableCollection<Monhoc>();
            DanhsachHocky = new ObservableCollection<Hocky>();
            DanhsachNamhoc = new ObservableCollection<Namhoc>();

            LoadDiemCommand = new Command(LoadDiem);

            LoadHocsinh();
            LoadMon();
            LoadHocky();
            LoadNamhoc();
        }

        void LoadHocsinh()
        {
            var user = _session.CurrentUser;
            if (user?.HocsinhId.HasValue == true)
            {
                Hocsinh = _db.GetHocsinhById(user.HocsinhId.Value);
                if (Hocsinh != null)
                {
                    Lophoc = _db.GetLophocById(Hocsinh.LophocId);
                }
                OnPropertyChanged(nameof(Hocsinh));
                OnPropertyChanged(nameof(Lophoc));
            }
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

            if (list.Any())
                HockySelected = list.First();
        }

        void LoadNamhoc()
        {
            DanhsachNamhoc.Clear();
            var list = _db.GetAllNamhoc().OrderByDescending(n => n.IsActive);
            foreach (var item in list)
                DanhsachNamhoc.Add(item);

            if (list.Any())
                NamhocSelected = list.First();
        }

        void LoadDiem()
        {
            var user = _session.CurrentUser;
            if (user?.HocsinhId.HasValue == false || HockySelected == null) return;

            DanhsachDiemMon.Clear();

            // Load điểm cho tất cả môn
            foreach (var mon in DanhsachMon)
            {
                var diems = _db.GetDiemByHocsinhMon(user.HocsinhId.Value, mon.Id, HockySelected.Id);
                var diemTB = _db.TinhDiemTrungBinhMon(user.HocsinhId.Value, mon.Id, HockySelected.Id);
                
                var display = DiemHelper.GroupDiemByMon(mon, diems, diemTB);
                DanhsachDiemMon.Add(display);
            }

            // Tính điểm trung bình học kỳ
            DiemTrungBinhHK = _db.TinhDiemTrungBinhHK(user.HocsinhId.Value, HockySelected.Id);
            HocLuc = DiemTrungBinhHK.HasValue ? XepLoai.GetHocLuc(DiemTrungBinhHK.Value) : "";

            OnPropertyChanged(nameof(DiemTrungBinhHK));
            OnPropertyChanged(nameof(HocLuc));
            OnPropertyChanged(nameof(DanhsachDiemMon));
        }
    }
}

