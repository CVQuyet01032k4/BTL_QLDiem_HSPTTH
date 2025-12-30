using System.Collections.ObjectModel;
using System.Windows.Input;
using BTL_QLDiem_HSPTTH.Data.Models;
using BTL_QLDiem_HSPTTH.Data.Services;
using BTL_QLDiem_HSPTTH.Services;
using BTL_QLDiem_HSPTTH.Helpers;
using System;
using System.Linq;

namespace BTL_QLDiem_HSPTTH.TeacherVM
{
    public class NhapdiemVM : BindableObject
    {
        private readonly DatabaseService _db;
        private readonly ToastService _toast;
        private readonly UserSessionService _session;

        public ObservableCollection<Phancong> DanhsachPhancong { get; set; }
        public ObservableCollection<Hocsinh> DanhsachHocsinh { get; set; }
        public ObservableCollection<Diem> DanhsachDiem { get; set; }
        public ObservableCollection<Hocky> DanhsachHocky { get; set; }

        public Phancong PhancongSelected { get; set; }
        public Hocsinh HocsinhSelected { get; set; }
        public Hocky HockySelected { get; set; }
        public LoaiDiem LoaiDiemSelected { get; set; }
        public int HeSo
        {
            get => _heSo;
            set { _heSo = value; OnPropertyChanged(); }
        }
        private int _heSo = 1;

        public void SetHeSo(int value)
        {
            HeSo = value;
        }
        public double? DiemSo { get; set; }
        public string GhiChu { get; set; }

        public ICommand LoadHocsinhCommand { get; }
        public ICommand LoadDiemCommand { get; }
        public ICommand AddDiemCommand { get; }
        public ICommand DeleteDiemCommand { get; }

        public NhapdiemVM()
        {
            _db = App.Current.Handler.MauiContext.Services.GetService<DatabaseService>();
            _toast = App.Current.Handler.MauiContext.Services.GetService<ToastService>();
            _session = App.Current.Handler.MauiContext.Services.GetService<UserSessionService>();

            DanhsachPhancong = new ObservableCollection<Phancong>();
            DanhsachHocsinh = new ObservableCollection<Hocsinh>();
            DanhsachDiem = new ObservableCollection<Diem>();
            DanhsachHocky = new ObservableCollection<Hocky>();

            LoadHocsinhCommand = new Command(LoadHocsinh);
            LoadDiemCommand = new Command(LoadDiem);
            AddDiemCommand = new Command(AddDiem);
            DeleteDiemCommand = new Command<Diem>(DeleteDiem);

            LoadPhancong();
            LoadHocky();
        }

        void LoadPhancong()
        {
            DanhsachPhancong.Clear();
            var user = _session.CurrentUser;
            if (user?.GiaovienId.HasValue == true)
            {
                var list = _db.GetPhancongByGiaovien(user.GiaovienId.Value);
                foreach (var item in list)
                    DanhsachPhancong.Add(item);
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

        void LoadHocsinh()
        {
            if (PhancongSelected == null) return;

            DanhsachHocsinh.Clear();
            var list = _db.GetHocsinhByLop(PhancongSelected.LophocId);
            foreach (var item in list)
                DanhsachHocsinh.Add(item);
        }

        void LoadDiem()
        {
            if (PhancongSelected == null || HocsinhSelected == null || HockySelected == null) return;

            DanhsachDiem.Clear();
            var list = _db.GetDiemByHocsinhMon(HocsinhSelected.Id, PhancongSelected.MonhocId, HockySelected.Id);
            foreach (var item in list)
                DanhsachDiem.Add(item);

            // Tính điểm trung bình
            var diemTB = _db.TinhDiemTrungBinhMon(HocsinhSelected.Id, PhancongSelected.MonhocId, HockySelected.Id);
            OnPropertyChanged(nameof(DiemTrungBinh));
        }

        public double? DiemTrungBinh
        {
            get
            {
                if (PhancongSelected == null || HocsinhSelected == null || HockySelected == null) return null;
                return _db.TinhDiemTrungBinhMon(HocsinhSelected.Id, PhancongSelected.MonhocId, HockySelected.Id);
            }
        }

        async void AddDiem()
        {
            if (PhancongSelected == null || HocsinhSelected == null || HockySelected == null)
            {
                await _toast.ShowError("Vui lòng chọn đầy đủ thông tin!");
                return;
            }

            if (!DiemSo.HasValue || DiemSo < 0 || DiemSo > 10)
            {
                await _toast.ShowError("Điểm phải từ 0 đến 10!");
                return;
            }

            var user = _session.CurrentUser;
            if (user == null) return;

            var diem = new Diem
            {
                HocsinhId = HocsinhSelected.Id,
                MonhocId = PhancongSelected.MonhocId,
                HockyId = HockySelected.Id,
                LoaiDiem = (int)LoaiDiemSelected,
                HeSo = HeSo,
                DiemSo = DiemSo,
                NgayNhap = DateTime.Now,
                GiaovienId = user.GiaovienId ?? 0,
                GhiChu = GhiChu
            };

            _db.AddDiem(diem);

            DiemSo = null;
            GhiChu = "";
            OnPropertyChanged(nameof(DiemSo));
            OnPropertyChanged(nameof(GhiChu));

            LoadDiem();
            await _toast.ShowSuccess("Thêm điểm thành công!");
        }

        async void DeleteDiem(Diem diem)
        {
            if (!await DialogHelper.Confirm("Xóa điểm này?")) return;

            _db.DeleteDiem(diem);
            LoadDiem();

            await _toast.ShowSuccess("Đã xóa!");
        }
    }
}

