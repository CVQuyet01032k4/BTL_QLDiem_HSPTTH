using System.Collections.ObjectModel;
using System.Windows.Input;
using BTL_QLDiem_HSPTTH.Data.Models;
using BTL_QLDiem_HSPTTH.Data.Services;
using BTL_QLDiem_HSPTTH.Services;
using BTL_QLDiem_HSPTTH.Helpers;
using System.Linq;

namespace BTL_QLDiem_HSPTTH.AdminVM
{
    public class QuanlyPhancong : BindableObject
    {
        private readonly DatabaseService _db;
        private readonly ToastService _toast;

        public ObservableCollection<PhancongDisplay> DanhsachPC { get; set; } = new();
        public ObservableCollection<Giaovien> DanhsachGV { get; set; } = new();
        public ObservableCollection<Lophoc> DanhsachLop { get; set; } = new();
        public ObservableCollection<Monhoc> DanhsachMon { get; set; } = new();

        public Giaovien GVSelected { get; set; }
        public Lophoc LopSelected { get; set; }
        public Monhoc MonSelected { get; set; }

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }

        public QuanlyPhancong()
        {
            _db = App.Current.Handler.MauiContext.Services.GetService<DatabaseService>();
            _toast = App.Current.Handler.MauiContext.Services.GetService<ToastService>();

            AddCommand = new Command(Add);
            DeleteCommand = new Command<PhancongDisplay>(Delete);

            LoadData();
        }

        void LoadData()
        {
            DanhsachPC.Clear();
            foreach (var pc in _db.GetAllPhancong())
            {
                var gv = _db.GetGiaovienById(pc.GiaovienId);
                var lop = _db.GetLophocById(pc.LophocId);
                var mon = _db.GetMonById(pc.MonhocId);
                DanhsachPC.Add(new PhancongDisplay
                {
                    Phancong = pc,
                    TenGiaovien = gv?.Hoten ?? "",
                    TenLop = lop?.Tenlop ?? "",
                    TenMon = mon?.Tenmh ?? ""
                });
            }

            DanhsachGV.Clear();
            foreach (var gv in _db.GetAllGiaovien())
                DanhsachGV.Add(gv);

            DanhsachLop.Clear();
            foreach (var lop in _db.GetAllLophoc())
                DanhsachLop.Add(lop);

            DanhsachMon.Clear();
            foreach (var mh in _db.GetAllMon())
                DanhsachMon.Add(mh);
        }

        async void Add()
        {
            if (GVSelected == null || LopSelected == null || MonSelected == null)
            {
                await _toast.ShowError("Chọn đầy đủ thông tin!");
                return;
            }

            if (_db.CheckPhancongTonTai(GVSelected.Id, LopSelected.Id, MonSelected.Id))
            {
                await _toast.ShowError("Phân công đã tồn tại!");
                return;
            }

            var pc = new Phancong
            {
                GiaovienId = GVSelected.Id,
                LophocId = LopSelected.Id,
                MonhocId = MonSelected.Id
            };

            _db.AddPhancong(pc);
            LoadData();

            await _toast.ShowSuccess("Thêm thành công!");
        }

        async void Delete(PhancongDisplay pcDisplay)
        {
            if (!await DialogHelper.Confirm("Xoá phân công này?")) return;

            _db.DeletePhancong(pcDisplay.Phancong);
            LoadData();

            await _toast.ShowSuccess("Đã xoá!");
        }
    }

    public class PhancongDisplay
    {
        public Phancong Phancong { get; set; }
        public string TenGiaovien { get; set; }
        public string TenLop { get; set; }
        public string TenMon { get; set; }
    }
}
