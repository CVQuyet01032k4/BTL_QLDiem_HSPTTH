using System.Collections.ObjectModel;
using System.Windows.Input;
using BTL_QLDiem_HSPTTH.Data.Models;
using BTL_QLDiem_HSPTTH.Data.Services;
using BTL_QLDiem_HSPTTH.Services;
using BTL_QLDiem_HSPTTH.Helpers;

namespace BTL_QLDiem_HSPTTH.AdminVM
{
    public class QuanlyGiaovien : BindableObject
    {
        private readonly DatabaseService _db;
        private readonly ToastService _toast;

        public ObservableCollection<Giaovien> DanhsachGV { get; set; }
        public ObservableCollection<Giaovien> DanhsachGV_Filter { get; set; }
        public ObservableCollection<Monhoc> DanhsachMon { get; set; }

        public string Hoten { get; set; }
        public Monhoc MonSelected { get; set; }

        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; Filter(); OnPropertyChanged(); }
        }
        private string _searchText;

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }

        public QuanlyGiaovien()
        {
            _db = App.Current.Handler.MauiContext.Services.GetService<DatabaseService>();
            _toast = App.Current.Handler.MauiContext.Services.GetService<ToastService>();

            DanhsachGV = new ObservableCollection<Giaovien>();
            DanhsachGV_Filter = new ObservableCollection<Giaovien>();
            DanhsachMon = new ObservableCollection<Monhoc>();

            AddCommand = new Command(AddGV);
            DeleteCommand = new Command<Giaovien>(DeleteGV);

            LoadMon();
            LoadGV();
        }

        void LoadMon()
        {
            DanhsachMon.Clear();
            var list = _db.GetAllMon();
            foreach (var item in list)
                DanhsachMon.Add(item);
        }

        void LoadGV()
        {
            DanhsachGV.Clear();
            var list = _db.GetAllGiaovien();
            foreach (var item in list)
                DanhsachGV.Add(item);

            Filter();
        }

        void Filter()
        {
            DanhsachGV_Filter.Clear();
            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? DanhsachGV
                : DanhsachGV.Where(x => x.Hoten.ToLower().Contains(SearchText.ToLower()));

            foreach (var item in filtered)
                DanhsachGV_Filter.Add(item);
        }

        async void AddGV()
        {
            if (string.IsNullOrWhiteSpace(Hoten) || MonSelected == null)
            {
                await _toast.ShowError("Nhập đầy đủ thông tin!");
                return;
            }

            var gv = new Giaovien
            {
                Hoten = Hoten,
                MonhocId = MonSelected.Id
            };

            _db.AddGiaovien(gv);

            Hoten = "";
            MonSelected = null;
            OnPropertyChanged(nameof(Hoten));
            OnPropertyChanged(nameof(MonSelected));

            LoadGV();
            await _toast.ShowSuccess("Thêm giáo viên thành công!");
        }

        async void DeleteGV(Giaovien gv)
        {
            if (!await DialogHelper.Confirm($"Xoá giáo viên '{gv.Hoten}'?")) return;

            _db.DeleteGiaovien(gv);
            LoadGV();

            await _toast.ShowSuccess("Đã xoá!");
        }
    }
}
