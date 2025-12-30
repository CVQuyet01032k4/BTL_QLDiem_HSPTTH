using System.Collections.ObjectModel;
using System.Windows.Input;
using BTL_QLDiem_HSPTTH.Data.Models;
using BTL_QLDiem_HSPTTH.Data.Services;
using BTL_QLDiem_HSPTTH.Services;
using BTL_QLDiem_HSPTTH.Helpers;

namespace BTL_QLDiem_HSPTTH.AdminVM
{
    public class QuanlyLop : BindableObject
    {
        private readonly DatabaseService _db;
        private readonly ToastService _toast;

        public ObservableCollection<Lophoc> DanhsachLop { get; set; }
        public ObservableCollection<Lophoc> DanhsachLop_Filter { get; set; }
        public ObservableCollection<Khoid> DanhsachKhoid { get; set; }

        public string TenLop { get; set; }
        public Khoid KhoidSelected { get; set; }
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; Filter(); OnPropertyChanged(); }
        }
        private string _searchText;

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }

        public QuanlyLop()
        {
            _db = App.Current.Handler.MauiContext.Services.GetService<DatabaseService>();
            _toast = App.Current.Handler.MauiContext.Services.GetService<ToastService>();

            DanhsachLop = new ObservableCollection<Lophoc>();
            DanhsachLop_Filter = new ObservableCollection<Lophoc>();
            DanhsachKhoid = new ObservableCollection<Khoid>();

            AddCommand = new Command(AddLop);
            DeleteCommand = new Command<Lophoc>(DeleteLop);

            LoadKhoid();
            LoadData();
        }

        void LoadKhoid()
        {
            DanhsachKhoid.Clear();
            var list = _db.GetAllKhoid();
            foreach (var item in list)
                DanhsachKhoid.Add(item);
        }

        void LoadData()
        {
            DanhsachLop.Clear();
            var list = _db.GetAllLophoc();
            foreach (var item in list)
                DanhsachLop.Add(item);

            Filter();
        }

        void Filter()
        {
            DanhsachLop_Filter.Clear();
            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? DanhsachLop
                : DanhsachLop.Where(x => x.Tenlop.ToLower().Contains(SearchText.ToLower()));

            foreach (var l in filtered)
                DanhsachLop_Filter.Add(l);
        }

        async void AddLop()
        {
            if (string.IsNullOrWhiteSpace(TenLop))
            {
                await _toast.ShowError("Vui lòng nhập tên lớp!");
                return;
            }

            if (KhoidSelected == null)
            {
                await _toast.ShowError("Vui lòng chọn khối!");
                return;
            }

            if (_db.GetLopByTen(TenLop) != null)
            {
                await _toast.ShowError("Lớp đã tồn tại!");
                return;
            }

            _db.AddLophoc(new Lophoc { Tenlop = TenLop, KhoidId = KhoidSelected.Id });

            TenLop = "";
            KhoidSelected = null;
            OnPropertyChanged(nameof(TenLop));
            OnPropertyChanged(nameof(KhoidSelected));
            LoadData();

            await _toast.ShowSuccess("Thêm lớp thành công!");
        }

        async void DeleteLop(Lophoc lop)
        {
            if (!await DialogHelper.Confirm($"Xoá lớp '{lop.Tenlop}'?")) return;

            _db.DeleteLophoc(lop);
            LoadData();

            await _toast.ShowSuccess("Đã xoá!");
        }
    }
}
