using System.Collections.ObjectModel;
using System.Windows.Input;
using BTL_QLDiem_HSPTTH.Data.Models;
using BTL_QLDiem_HSPTTH.Data.Services;
using BTL_QLDiem_HSPTTH.Services;
using BTL_QLDiem_HSPTTH.Helpers;
using System.Linq;
using static BTL_QLDiem_HSPTTH.Data.Models.Config;

namespace BTL_QLDiem_HSPTTH.ParentVM
{
    public class ParentDashboardVM : BindableObject
    {
        private readonly DatabaseService _db;
        private readonly UserSessionService _session;

        public Hocsinh Hocsinh { get; set; }
        public Lophoc Lop { get; set; }
        public Giaovien GiaoVienCN { get; set; }
        public ObservableCollection<Thongbao> DanhsachThongbao { get; set; }
        public ObservableCollection<DiemMonDisplay> DanhsachDiemMon { get; set; }

        // Summary properties
        public double? DiemTrungBinh { get; private set; }
        public string HocLuc { get; private set; }
        public string HanhKiem { get; private set; } = "Tốt";
        public int SoVangCoPhep { get; private set; } = 0;

        public ICommand LoadDataCommand { get; }

        public ParentDashboardVM()
        {
            _db = App.Current.Handler.MauiContext.Services.GetService<DatabaseService>();
            _session = App.Current.Handler.MauiContext.Services.GetService<UserSessionService>();

            DanhsachThongbao = new ObservableCollection<Thongbao>();
            DanhsachDiemMon = new ObservableCollection<DiemMonDisplay>();

            LoadDataCommand = new Command(LoadData);

            LoadData();
        }

        void LoadData()
        {
            var user = _session.CurrentUser;
            if (user?.HocsinhId.HasValue == true)
            {
                Hocsinh = _db.GetHocsinhById(user.HocsinhId.Value);
                OnPropertyChanged(nameof(Hocsinh));

                // Load lớp và giáo viên chủ nhiệm
                if (Hocsinh != null)
                {
                    Lop = _db.GetLophocById(Hocsinh.LophocId);
                    if (Lop?.GiaovienCNId.HasValue == true)
                    {
                        GiaoVienCN = _db.GetGiaovienById(Lop.GiaovienCNId.Value);
                    }
                    OnPropertyChanged(nameof(Lop));
                    OnPropertyChanged(nameof(GiaoVienCN));
                }

                // Load thông báo
                DanhsachThongbao.Clear();
                var list = _db.GetThongbaoByHocsinh(user.HocsinhId.Value);
                foreach (var item in list.Take(3))
                    DanhsachThongbao.Add(item);

                // Load điểm mới cập nhật
                LoadDiemMoiCapNhat(user.HocsinhId.Value);
            }
        }

        void LoadDiemMoiCapNhat(int hocsinhId)
        {
            DanhsachDiemMon.Clear();

            // Lấy học kỳ hiện tại
            var hockyHienTai = _db.GetAllHocky().OrderByDescending(h => h.IsActive).ThenByDescending(h => h.So).FirstOrDefault();
            if (hockyHienTai == null) return;

            // Lấy các môn học của học sinh
            if (Hocsinh == null) return;

            var phancongList = _db.GetPhancongByLop(Hocsinh.LophocId);
            var monIds = phancongList.Select(pc => pc.MonhocId).Distinct().ToList();

            double tongDiemTB = 0;
            int soMon = 0;

            foreach (var monId in monIds.Take(4)) // Chỉ hiển thị 4 môn đầu tiên
            {
                var mon = _db.GetMonById(monId);
                if (mon == null) continue;

                var diems = _db.GetDiemByHocsinhMon(hocsinhId, monId, hockyHienTai.Id);
                var diemTB = _db.TinhDiemTrungBinhMon(hocsinhId, monId, hockyHienTai.Id);

                if (diemTB.HasValue)
                {
                    tongDiemTB += diemTB.Value;
                    soMon++;
                }

                var display = DiemHelper.GroupDiemByMon(mon, diems, diemTB);
                DanhsachDiemMon.Add(display);
            }

            // Tính điểm trung bình chung
            DiemTrungBinh = soMon > 0 ? System.Math.Round(tongDiemTB / soMon, 1) : null;
            HocLuc = DiemTrungBinh.HasValue ? XepLoai.GetHocLuc(DiemTrungBinh.Value) : "";

            OnPropertyChanged(nameof(DiemTrungBinh));
            OnPropertyChanged(nameof(HocLuc));
            OnPropertyChanged(nameof(HanhKiem));
            OnPropertyChanged(nameof(SoVangCoPhep));
            OnPropertyChanged(nameof(DanhsachDiemMon));
        }
    }
}

