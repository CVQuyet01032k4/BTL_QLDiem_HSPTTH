using System.Collections.ObjectModel;
using System.Windows.Input;
using BTL_QLDiem_HSPTTH.Data.Models;
using BTL_QLDiem_HSPTTH.Data.Services;
using BTL_QLDiem_HSPTTH.Services;
using BTL_QLDiem_HSPTTH.Helpers;

namespace BTL_QLDiem_HSPTTH.AdminVM
{
    public class QuanlyPhancong : BindableObject
    {
        private readonly DatabaseService _db;
        private readonly ToastService _toast;

        public ObservableCollection<Phancong> DanhsachPC { get; set; } = new();
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
            DeleteCommand = new Command<Phancong>(Delete);

            LoadData();
        }

        void LoadData()
        {
            DanhsachPC.Clear();
            foreach (var pc in _db.GetAllPhancong())
                DanhsachPC.Add(pc);

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

        async void Delete(Phancong pc)
        {
            if (!await DialogHelper.Confirm("Xoá phân công này?")) return;

            _db.DeletePhancong(pc);
            LoadData();

            await _toast.ShowSuccess("Đã xoá!");
        }
    }
}
