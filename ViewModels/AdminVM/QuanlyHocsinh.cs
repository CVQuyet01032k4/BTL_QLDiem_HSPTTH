using System.Collections.ObjectModel;
using System.Windows.Input;
using BTL_QLDiem_HSPTTH.Data.Models;
using BTL_QLDiem_HSPTTH.Data.Services;
using BTL_QLDiem_HSPTTH.Services;
using BTL_QLDiem_HSPTTH.Helpers;
using System;

namespace BTL_QLDiem_HSPTTH.AdminVM
{
    public class QuanlyHocsinh : BindableObject
    {
        private readonly DatabaseService _db;
        private readonly ToastService _toast;

        public ObservableCollection<Hocsinh> DanhsachHS { get; set; }
        public ObservableCollection<Hocsinh> DanhsachHS_Filter { get; set; }
        public ObservableCollection<Lophoc> DanhsachLop { get; set; }

        public string Hoten { get; set; }
        public DateTime Ngaysinh { get; set; } = DateTime.Now;
        public string Gioitinh { get; set; }
        public Lophoc LopSelected { get; set; }

        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; Filter(); OnPropertyChanged(); }
        }
        private string _searchText;

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }

        public QuanlyHocsinh()
        {
            _db = App.Current.Handler.MauiContext.Services.GetService<DatabaseService>();
            _toast = App.Current.Handler.MauiContext.Services.GetService<ToastService>();

            DanhsachHS = new ObservableCollection<Hocsinh>();
            DanhsachHS_Filter = new ObservableCollection<Hocsinh>();
            DanhsachLop = new ObservableCollection<Lophoc>();

            AddCommand = new Command(AddHS);
            DeleteCommand = new Command<Hocsinh>(DeleteHS);

            LoadLop();
            LoadHS();
        }

        void LoadLop()
        {
            DanhsachLop.Clear();
            var list = _db.GetAllLophoc();
            foreach (var item in list)
                DanhsachLop.Add(item);
        }

        void LoadHS()
        {
            DanhsachHS.Clear();
            var list = _db.GetAllHocsinh();
            foreach (var item in list)
                DanhsachHS.Add(item);

            Filter();
        }

        void Filter()
        {
            DanhsachHS_Filter.Clear();
            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? DanhsachHS
                : DanhsachHS.Where(x => x.Hoten.ToLower().Contains(SearchText.ToLower()));

            foreach (var item in filtered)
                DanhsachHS_Filter.Add(item);
        }

        async void AddHS()
        {
            if (string.IsNullOrWhiteSpace(Hoten) || LopSelected == null)
            {
                await _toast.ShowError("Nhập đầy đủ thông tin!");
                return;
            }

            var hs = new Hocsinh
            {
                Hoten = Hoten,
                Ngaysinh = Ngaysinh,
                Gioitinh = Gioitinh,
                LophocId = LopSelected.Id,
                Mahocsinh = $"HS{LopSelected.Id:D4}{DateTime.Now:MMddHHmmss}".Substring(0, 10) // Tự động tạo mã học sinh
            };

            _db.AddHocsinh(hs);

            Hoten = "";
            Gioitinh = "";
            LopSelected = null;
            OnPropertyChanged(nameof(Hoten));
            OnPropertyChanged(nameof(Gioitinh));
            OnPropertyChanged(nameof(LopSelected));

            LoadHS();
            await _toast.ShowSuccess("Thêm học sinh thành công!");
        }

        async void DeleteHS(Hocsinh hs)
        {
            if (!await DialogHelper.Confirm($"Xoá học sinh '{hs.Hoten}'?")) return;

            _db.DeleteHocsinh(hs);
            LoadHS();

            await _toast.ShowSuccess("Đã xoá!");
        }
    }
}
