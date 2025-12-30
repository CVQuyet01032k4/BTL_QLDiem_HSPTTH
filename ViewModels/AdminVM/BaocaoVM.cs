using System.Collections.ObjectModel;
using System.Windows.Input;
using BTL_QLDiem_HSPTTH.Data.Models;
using BTL_QLDiem_HSPTTH.Data.Services;
using BTL_QLDiem_HSPTTH.Services;
using System.Linq;
using System;
using static BTL_QLDiem_HSPTTH.Data.Models.Config;

namespace BTL_QLDiem_HSPTTH.AdminVM
{
    public class BaocaoVM : BindableObject
    {
        private readonly DatabaseService _db;

        public ObservableCollection<Lophoc> DanhsachLop { get; set; }
        public ObservableCollection<Khoid> DanhsachKhoid { get; set; }
        public ObservableCollection<Hocky> DanhsachHocky { get; set; }
        public ObservableCollection<ThongKeItem> DanhsachThongKe { get; set; }

        public Lophoc LopSelected { get; set; }
        public Khoid KhoidSelected { get; set; }
        public Hocky HockySelected { get; set; }

        public ICommand LoadThongKeCommand { get; }

        public class ThongKeItem
        {
            public string TenLop { get; set; }
            public int SoLuongHS { get; set; }
            public int SoLuongGioi { get; set; }
            public int SoLuongKha { get; set; }
            public int SoLuongTrungBinh { get; set; }
            public int SoLuongYeu { get; set; }
            public double DiemTrungBinhLop { get; set; }
        }

        public BaocaoVM()
        {
            _db = App.Current.Handler.MauiContext.Services.GetService<DatabaseService>();

            DanhsachLop = new ObservableCollection<Lophoc>();
            DanhsachKhoid = new ObservableCollection<Khoid>();
            DanhsachHocky = new ObservableCollection<Hocky>();
            DanhsachThongKe = new ObservableCollection<ThongKeItem>();

            LoadThongKeCommand = new Command(LoadThongKe);

            LoadLop();
            LoadKhoid();
            LoadHocky();
        }

        void LoadLop()
        {
            DanhsachLop.Clear();
            var list = _db.GetAllLophoc();
            foreach (var item in list)
                DanhsachLop.Add(item);
        }

        void LoadKhoid()
        {
            DanhsachKhoid.Clear();
            var list = _db.GetAllKhoid();
            foreach (var item in list)
                DanhsachKhoid.Add(item);
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

        void LoadThongKe()
        {
            DanhsachThongKe.Clear();

            if (HockySelected == null) return;

            var lopQuery = _db.GetAllLophoc().AsQueryable();

            if (KhoidSelected != null)
                lopQuery = lopQuery.Where(l => l.KhoidId == KhoidSelected.Id);

            if (LopSelected != null)
                lopQuery = lopQuery.Where(l => l.Id == LopSelected.Id);

            foreach (var lop in lopQuery.ToList())
            {
                var hocsinhs = _db.GetHocsinhByLop(lop.Id);
                var tk = new ThongKeItem
                {
                    TenLop = lop.Tenlop,
                    SoLuongHS = hocsinhs.Count
                };

                double tongDiemTB = 0;
                int soHS = 0;

                foreach (var hs in hocsinhs)
                {
                    var diemTBHK = _db.TinhDiemTrungBinhHK(hs.Id, HockySelected.Id);
                    if (diemTBHK.HasValue)
                    {
                        tongDiemTB += diemTBHK.Value;
                        soHS++;

                        var hocLuc = XepLoai.GetHocLuc(diemTBHK.Value);
                        if (hocLuc == "Giỏi") tk.SoLuongGioi++;
                        else if (hocLuc == "Khá") tk.SoLuongKha++;
                        else if (hocLuc == "Trung bình") tk.SoLuongTrungBinh++;
                        else if (hocLuc == "Yếu" || hocLuc == "Kém") tk.SoLuongYeu++;
                    }
                }

                tk.DiemTrungBinhLop = soHS > 0 ? Math.Round(tongDiemTB / soHS, 2) : 0;

                DanhsachThongKe.Add(tk);
            }
        }
    }
}

