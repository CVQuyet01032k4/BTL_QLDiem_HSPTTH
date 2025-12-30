using System.Collections.ObjectModel;
using System.Windows.Input;
using BTL_QLDiem_HSPTTH.Data.Models;
using BTL_QLDiem_HSPTTH.Data.Services;
using BTL_QLDiem_HSPTTH.Services;
using BTL_QLDiem_HSPTTH.Helpers;

namespace BTL_QLDiem_HSPTTH.AdminVM
{
    public class QuanlyMon : BindableObject
    {
        private readonly DatabaseService _db;
        private readonly ToastService _toast;

        public ObservableCollection<Monhoc> DanhsachMon { get; set; }
        public ObservableCollection<Monhoc> DanhsachMon_Filter { get; set; }

        public string TenMon { get; set; }
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; Filter(); OnPropertyChanged(); }
        }
        private string _searchText;

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }

        public QuanlyMon()
        {
            _db = App.Current.Handler.MauiContext.Services.GetService<DatabaseService>();
            _toast = App.Current.Handler.MauiContext.Services.GetService<ToastService>();

            DanhsachMon = new ObservableCollection<Monhoc>();
            DanhsachMon_Filter = new ObservableCollection<Monhoc>();

            AddCommand = new Command(AddMon);
            DeleteCommand = new Command<Monhoc>(DeleteMon);

            LoadData();
        }

        void LoadData()
        {
            DanhsachMon.Clear();
            var list = _db.GetAllMon();
            foreach (var item in list)
                DanhsachMon.Add(item);

            Filter();
        }

        void Filter()
        {
            DanhsachMon_Filter.Clear();
            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? DanhsachMon
                : DanhsachMon.Where(x => x.Tenmh.ToLower().Contains(SearchText.ToLower()));

            foreach (var m in filtered)
                DanhsachMon_Filter.Add(m);
        }

        async void AddMon()
        {
            if (string.IsNullOrWhiteSpace(TenMon))
            {
                await _toast.ShowError("Vui lòng nhập tên môn!");
                return;
            }

            if (_db.GetMonByTen(TenMon) != null)
            {
                await _toast.ShowError("Môn học đã tồn tại!");
                return;
            }

            _db.AddMon(new Monhoc { Tenmh = TenMon });
            TenMon = "";
            LoadData();

            await _toast.ShowSuccess("Thêm thành công!");
        }

        async void DeleteMon(Monhoc mon)
        {
            if (!await DialogHelper.Confirm($"Xoá môn '{mon.Tenmh}'?")) return;

            _db.DeleteMon(mon);
            LoadData();

            await _toast.ShowSuccess("Đã xoá!");
        }
    }
}
