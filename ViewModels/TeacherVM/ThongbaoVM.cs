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
    public class ThongbaoVM : BindableObject
    {
        private readonly DatabaseService _db;
        private readonly ToastService _toast;
        private readonly UserSessionService _session;

        public ObservableCollection<Thongbao> DanhsachThongbao { get; set; }
        public ObservableCollection<Lophoc> DanhsachLop { get; set; }
        public ObservableCollection<Hocsinh> DanhsachHocsinh { get; set; }

        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
        public Lophoc LopSelected { get; set; }
        public Hocsinh HocsinhSelected { get; set; }

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand LoadHocsinhCommand { get; }

        public ThongbaoVM()
        {
            _db = App.Current.Handler.MauiContext.Services.GetService<DatabaseService>();
            _toast = App.Current.Handler.MauiContext.Services.GetService<ToastService>();
            _session = App.Current.Handler.MauiContext.Services.GetService<UserSessionService>();

            DanhsachThongbao = new ObservableCollection<Thongbao>();
            DanhsachLop = new ObservableCollection<Lophoc>();
            DanhsachHocsinh = new ObservableCollection<Hocsinh>();

            AddCommand = new Command(AddThongbao);
            DeleteCommand = new Command<Thongbao>(DeleteThongbao);
            LoadHocsinhCommand = new Command(LoadHocsinh);

            LoadThongbao();
            LoadLop();
        }

        void LoadThongbao()
        {
            DanhsachThongbao.Clear();
            var list = _db.GetAllThongbao();
            foreach (var item in list)
                DanhsachThongbao.Add(item);
        }

        void LoadLop()
        {
            DanhsachLop.Clear();
            var list = _db.GetAllLophoc();
            foreach (var item in list)
                DanhsachLop.Add(item);
        }

        void LoadHocsinh()
        {
            if (LopSelected == null) return;

            DanhsachHocsinh.Clear();
            var list = _db.GetHocsinhByLop(LopSelected.Id);
            foreach (var item in list)
                DanhsachHocsinh.Add(item);
        }

        async void AddThongbao()
        {
            if (string.IsNullOrWhiteSpace(TieuDe) || string.IsNullOrWhiteSpace(NoiDung))
            {
                await _toast.ShowError("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            var user = _session.CurrentUser;
            if (user == null) return;

            var tb = new Thongbao
            {
                TieuDe = TieuDe,
                NoiDung = NoiDung,
                NgayTao = DateTime.Now,
                NguoiGuiId = user.Id,
                NguoiGuiRole = user.Role,
                LophocId = LopSelected?.Id,
                HocsinhId = HocsinhSelected?.Id,
                IsRead = false
            };

            _db.AddThongbao(tb);

            TieuDe = "";
            NoiDung = "";
            LopSelected = null;
            HocsinhSelected = null;
            OnPropertyChanged(nameof(TieuDe));
            OnPropertyChanged(nameof(NoiDung));
            OnPropertyChanged(nameof(LopSelected));
            OnPropertyChanged(nameof(HocsinhSelected));

            LoadThongbao();
            await _toast.ShowSuccess("Gửi thông báo thành công!");
        }

        async void DeleteThongbao(Thongbao tb)
        {
            if (!await DialogHelper.Confirm("Xóa thông báo này?")) return;

            _db.DeleteThongbao(tb);
            LoadThongbao();

            await _toast.ShowSuccess("Đã xóa!");
        }
    }
}

