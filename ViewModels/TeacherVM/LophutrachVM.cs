using System.Collections.ObjectModel;
using System.Windows.Input;
using BTL_QLDiem_HSPTTH.Data.Models;
using BTL_QLDiem_HSPTTH.Data.Services;
using BTL_QLDiem_HSPTTH.Services;
using System.Linq;

namespace BTL_QLDiem_HSPTTH.TeacherVM
{
    public class LophutrachVM : BindableObject
    {
        private readonly DatabaseService _db;
        private readonly UserSessionService _session;

        public ObservableCollection<PhancongDisplay> DanhsachPhancong { get; set; }
        public ObservableCollection<Hocsinh> DanhsachHocsinh { get; set; }

        public PhancongDisplay PhancongSelected { get; set; }

        public ICommand LoadHocsinhCommand { get; }

        public LophutrachVM()
        {
            _db = App.Current.Handler.MauiContext.Services.GetService<DatabaseService>();
            _session = App.Current.Handler.MauiContext.Services.GetService<UserSessionService>();

            DanhsachPhancong = new ObservableCollection<PhancongDisplay>();
            DanhsachHocsinh = new ObservableCollection<Hocsinh>();

            LoadHocsinhCommand = new Command(LoadHocsinh);

            LoadPhancong();
        }

        void LoadPhancong()
        {
            DanhsachPhancong.Clear();
            var user = _session.CurrentUser;
            if (user?.GiaovienId.HasValue == true)
            {
                var list = _db.GetPhancongByGiaovien(user.GiaovienId.Value);
                foreach (var item in list)
                {
                    var lop = _db.GetLophocById(item.LophocId);
                    var mon = _db.GetMonById(item.MonhocId);
                    var display = new PhancongDisplay
                    {
                        Id = item.Id,
                        GiaovienId = item.GiaovienId,
                        LophocId = item.LophocId,
                        MonhocId = item.MonhocId,
                        TenLop = lop != null ? lop.Tenlop : "",
                        TenMon = mon != null ? mon.Tenmh : ""
                    };
                    DanhsachPhancong.Add(display);
                }
            }
        }

        void LoadHocsinh()
        {
            if (PhancongSelected == null) return;

            DanhsachHocsinh.Clear();
            var list = _db.GetHocsinhByLop(PhancongSelected.LophocId);
            foreach (var item in list)
                DanhsachHocsinh.Add(item);
            
            OnPropertyChanged(nameof(DanhsachHocsinh));
        }

        public class PhancongDisplay : Phancong
        {
            public string TenLop { get; set; }
            public string TenMon { get; set; }
            
            public override string ToString()
            {
                return $"{TenLop} - {TenMon}";
            }
        }
    }
}

