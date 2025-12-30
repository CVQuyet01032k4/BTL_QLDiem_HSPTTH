using SQLite;
using BTL_QLDiem_HSPTTH.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace BTL_QLDiem_HSPTTH.Data.Services
{
    public class DatabaseService
    {
        private SQLiteConnection _db;
        private readonly string _dbPath;

        public DatabaseService()
        {
            _dbPath = Path.Combine(FileSystem.AppDataDirectory, "qldiem.db3");
            _db = new SQLiteConnection(_dbPath);
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            _db.CreateTable<User>();
            _db.CreateTable<Khoid>();
            _db.CreateTable<Lophoc>();
            _db.CreateTable<Monhoc>();
            _db.CreateTable<Giaovien>();
            _db.CreateTable<Hocsinh>();
            _db.CreateTable<Phancong>();
            _db.CreateTable<Namhoc>();
            _db.CreateTable<Hocky>();
            _db.CreateTable<Diem>();
            _db.CreateTable<Thongbao>();
            _db.CreateTable<Config>();

            // Khởi tạo dữ liệu mẫu nếu chưa có
            InitializeDefaultData();
        }

        private void InitializeDefaultData()
        {
            // Tạo admin mặc định
            if (!_db.Table<User>().Any(u => u.Username == "admin"))
            {
                _db.Insert(new User
                {
                    Username = "admin",
                    Password = "admin123", // Nên hash password trong thực tế
                    Role = Role.Admin
                });
            }

            // Tạo khối
            if (!_db.Table<Khoid>().Any())
            {
                _db.Insert(new Khoid { Tenkhoi = "10" });
                _db.Insert(new Khoid { Tenkhoi = "11" });
                _db.Insert(new Khoid { Tenkhoi = "12" });
            }

            // Tạo môn học mẫu
            if (!_db.Table<Monhoc>().Any())
            {
                var monhoc = new[] { "Toán", "Lý", "Hóa", "Văn", "Anh", "Sinh", "Sử", "Địa", "GDCD" };
                foreach (var mh in monhoc)
                {
                    _db.Insert(new Monhoc { Tenmh = mh });
                }
            }

            // Cấu hình hệ số điểm mặc định
            if (!_db.Table<Config>().Any())
            {
                _db.Insert(new Config { Key = DiemConfig.HeSoMiemg, Value = "1", Description = "Hệ số điểm miệng" });
                _db.Insert(new Config { Key = DiemConfig.HeSo15Phut, Value = "1", Description = "Hệ số điểm 15 phút" });
                _db.Insert(new Config { Key = DiemConfig.HeSo1Tiet, Value = "2", Description = "Hệ số điểm 1 tiết" });
                _db.Insert(new Config { Key = DiemConfig.HeSoThi, Value = "3", Description = "Hệ số điểm thi" });
            }
        }

        #region User
        public User GetUser(string username, string password)
        {
            return _db.Table<User>().FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        public User GetUserById(int id)
        {
            return _db.Table<User>().FirstOrDefault(u => u.Id == id);
        }

        public List<User> GetAllUsers()
        {
            return _db.Table<User>().ToList();
        }

        public void AddUser(User user)
        {
            _db.Insert(user);
        }

        public void UpdateUser(User user)
        {
            _db.Update(user);
        }

        public void DeleteUser(User user)
        {
            _db.Delete(user);
        }
        #endregion

        #region Khoid
        public List<Khoid> GetAllKhoid()
        {
            return _db.Table<Khoid>().ToList();
        }

        public Khoid GetKhoidById(int id)
        {
            return _db.Table<Khoid>().FirstOrDefault(k => k.Id == id);
        }

        public void AddKhoid(Khoid khoid)
        {
            _db.Insert(khoid);
        }

        public void DeleteKhoid(Khoid khoid)
        {
            _db.Delete(khoid);
        }
        #endregion

        #region Lophoc
        public List<Lophoc> GetAllLophoc()
        {
            return _db.Table<Lophoc>().ToList();
        }

        public Lophoc GetLophocById(int id)
        {
            return _db.Table<Lophoc>().FirstOrDefault(l => l.Id == id);
        }

        public Lophoc GetLopByTen(string tenlop)
        {
            return _db.Table<Lophoc>().FirstOrDefault(l => l.Tenlop == tenlop);
        }

        public void AddLophoc(Lophoc lop)
        {
            _db.Insert(lop);
        }

        public void UpdateLophoc(Lophoc lop)
        {
            _db.Update(lop);
        }

        public void DeleteLophoc(Lophoc lop)
        {
            _db.Delete(lop);
        }
        #endregion

        #region Monhoc
        public List<Monhoc> GetAllMon()
        {
            return _db.Table<Monhoc>().ToList();
        }

        public Monhoc GetMonById(int id)
        {
            return _db.Table<Monhoc>().FirstOrDefault(m => m.Id == id);
        }

        public Monhoc GetMonByTen(string tenmh)
        {
            return _db.Table<Monhoc>().FirstOrDefault(m => m.Tenmh == tenmh);
        }

        public void AddMon(Monhoc mon)
        {
            _db.Insert(mon);
        }

        public void UpdateMon(Monhoc mon)
        {
            _db.Update(mon);
        }

        public void DeleteMon(Monhoc mon)
        {
            _db.Delete(mon);
        }
        #endregion

        #region Giaovien
        public List<Giaovien> GetAllGiaovien()
        {
            return _db.Table<Giaovien>().ToList();
        }

        public Giaovien GetGiaovienById(int id)
        {
            return _db.Table<Giaovien>().FirstOrDefault(g => g.Id == id);
        }

        public void AddGiaovien(Giaovien gv)
        {
            _db.Insert(gv);
        }

        public void UpdateGiaovien(Giaovien gv)
        {
            _db.Update(gv);
        }

        public void DeleteGiaovien(Giaovien gv)
        {
            _db.Delete(gv);
        }
        #endregion

        #region Hocsinh
        public List<Hocsinh> GetAllHocsinh()
        {
            return _db.Table<Hocsinh>().ToList();
        }

        public Hocsinh GetHocsinhById(int id)
        {
            return _db.Table<Hocsinh>().FirstOrDefault(h => h.Id == id);
        }

        public List<Hocsinh> GetHocsinhByLop(int lophocId)
        {
            return _db.Table<Hocsinh>().Where(h => h.LophocId == lophocId).ToList();
        }

        public void AddHocsinh(Hocsinh hs)
        {
            _db.Insert(hs);
        }

        public void UpdateHocsinh(Hocsinh hs)
        {
            _db.Update(hs);
        }

        public void DeleteHocsinh(Hocsinh hs)
        {
            _db.Delete(hs);
        }
        #endregion

        #region Phancong
        public List<Phancong> GetAllPhancong()
        {
            return _db.Table<Phancong>().ToList();
        }

        public Phancong GetPhancongById(int id)
        {
            return _db.Table<Phancong>().FirstOrDefault(p => p.Id == id);
        }

        public bool CheckPhancongTonTai(int gvId, int lopId, int monId)
        {
            return _db.Table<Phancong>().Any(p => p.GiaovienId == gvId && p.LophocId == lopId && p.MonhocId == monId);
        }

        public List<Phancong> GetPhancongByGiaovien(int gvId)
        {
            return _db.Table<Phancong>().Where(p => p.GiaovienId == gvId).ToList();
        }

        public List<Phancong> GetPhancongByLop(int lopId)
        {
            return _db.Table<Phancong>().Where(p => p.LophocId == lopId).ToList();
        }

        public void AddPhancong(Phancong pc)
        {
            _db.Insert(pc);
        }

        public void UpdatePhancong(Phancong pc)
        {
            _db.Update(pc);
        }

        public void DeletePhancong(Phancong pc)
        {
            _db.Delete(pc);
        }
        #endregion

        #region Namhoc
        public List<Namhoc> GetAllNamhoc()
        {
            return _db.Table<Namhoc>().OrderByDescending(n => n.Tungay).ToList();
        }

        public Namhoc GetNamhocById(int id)
        {
            return _db.Table<Namhoc>().FirstOrDefault(n => n.Id == id);
        }

        public Namhoc GetNamhocActive()
        {
            return _db.Table<Namhoc>().FirstOrDefault(n => n.IsActive);
        }

        public void AddNamhoc(Namhoc namhoc)
        {
            _db.Insert(namhoc);
        }

        public void UpdateNamhoc(Namhoc namhoc)
        {
            _db.Update(namhoc);
        }

        public void DeleteNamhoc(Namhoc namhoc)
        {
            _db.Delete(namhoc);
        }
        #endregion

        #region Hocky
        public List<Hocky> GetAllHocky()
        {
            return _db.Table<Hocky>().ToList();
        }

        public Hocky GetHockyById(int id)
        {
            return _db.Table<Hocky>().FirstOrDefault(h => h.Id == id);
        }

        public Hocky GetHockyActive()
        {
            return _db.Table<Hocky>().FirstOrDefault(h => h.IsActive);
        }

        public List<Hocky> GetHockyByNamhoc(int namhocId)
        {
            return _db.Table<Hocky>().Where(h => h.NamhocId == namhocId).ToList();
        }

        public void AddHocky(Hocky hocky)
        {
            _db.Insert(hocky);
        }

        public void UpdateHocky(Hocky hocky)
        {
            _db.Update(hocky);
        }

        public void DeleteHocky(Hocky hocky)
        {
            _db.Delete(hocky);
        }
        #endregion

        #region Diem
        public List<Diem> GetAllDiem()
        {
            return _db.Table<Diem>().ToList();
        }

        public Diem GetDiemById(int id)
        {
            return _db.Table<Diem>().FirstOrDefault(d => d.Id == id);
        }

        public List<Diem> GetDiemByHocsinh(int hocsinhId, int? hockyId = null)
        {
            var query = _db.Table<Diem>().Where(d => d.HocsinhId == hocsinhId);
            if (hockyId.HasValue)
                query = query.Where(d => d.HockyId == hockyId.Value);
            return query.ToList();
        }

        public List<Diem> GetDiemByHocsinhMon(int hocsinhId, int monhocId, int hockyId)
        {
            return _db.Table<Diem>()
                .Where(d => d.HocsinhId == hocsinhId && d.MonhocId == monhocId && d.HockyId == hockyId)
                .ToList();
        }

        public List<Diem> GetDiemByLopMon(int lophocId, int monhocId, int hockyId)
        {
            var hocsinhIds = _db.Table<Hocsinh>().Where(h => h.LophocId == lophocId).Select(h => h.Id).ToList();
            return _db.Table<Diem>()
                .Where(d => hocsinhIds.Contains(d.HocsinhId) && d.MonhocId == monhocId && d.HockyId == hockyId)
                .ToList();
        }

        public void AddDiem(Diem diem)
        {
            _db.Insert(diem);
        }

        public void UpdateDiem(Diem diem)
        {
            _db.Update(diem);
        }

        public void DeleteDiem(Diem diem)
        {
            _db.Delete(diem);
        }

        // Tính điểm trung bình môn theo học kỳ
        public double? TinhDiemTrungBinhMon(int hocsinhId, int monhocId, int hockyId)
        {
            var diems = GetDiemByHocsinhMon(hocsinhId, monhocId, hockyId)
                .Where(d => d.DiemSo.HasValue)
                .ToList();

            if (!diems.Any()) return null;

            double tongDiem = 0;
            int tongHeSo = 0;

            foreach (var d in diems)
            {
                tongDiem += d.DiemSo.Value * d.HeSo;
                tongHeSo += d.HeSo;
            }

            return tongHeSo > 0 ? Math.Round(tongDiem / tongHeSo, 2) : null;
        }

        // Tính điểm trung bình học kỳ (tất cả môn)
        public double? TinhDiemTrungBinhHK(int hocsinhId, int hockyId)
        {
            var monhocIds = _db.Table<Phancong>()
                .Join(_db.Table<Hocsinh>(), pc => pc.LophocId, hs => hs.LophocId, (pc, hs) => new { pc.MonhocId, hs.Id })
                .Where(x => x.Id == hocsinhId)
                .Select(x => x.MonhocId)
                .Distinct()
                .ToList();

            if (!monhocIds.Any()) return null;

            double tongDiemTB = 0;
            int soMon = 0;

            foreach (var monId in monhocIds)
            {
                var diemTB = TinhDiemTrungBinhMon(hocsinhId, monId, hockyId);
                if (diemTB.HasValue)
                {
                    tongDiemTB += diemTB.Value;
                    soMon++;
                }
            }

            return soMon > 0 ? Math.Round(tongDiemTB / soMon, 2) : null;
        }
        #endregion

        #region Thongbao
        public List<Thongbao> GetAllThongbao()
        {
            return _db.Table<Thongbao>().OrderByDescending(t => t.NgayTao).ToList();
        }

        public Thongbao GetThongbaoById(int id)
        {
            return _db.Table<Thongbao>().FirstOrDefault(t => t.Id == id);
        }

        public List<Thongbao> GetThongbaoByHocsinh(int hocsinhId)
        {
            return _db.Table<Thongbao>()
                .Where(t => t.HocsinhId == null || t.HocsinhId == hocsinhId)
                .OrderByDescending(t => t.NgayTao)
                .ToList();
        }

        public List<Thongbao> GetThongbaoByLop(int lophocId)
        {
            return _db.Table<Thongbao>()
                .Where(t => t.LophocId == null || t.LophocId == lophocId)
                .OrderByDescending(t => t.NgayTao)
                .ToList();
        }

        public void AddThongbao(Thongbao tb)
        {
            _db.Insert(tb);
        }

        public void UpdateThongbao(Thongbao tb)
        {
            _db.Update(tb);
        }

        public void DeleteThongbao(Thongbao tb)
        {
            _db.Delete(tb);
        }
        #endregion

        #region Config
        public Config GetConfig(string key)
        {
            return _db.Table<Config>().FirstOrDefault(c => c.Key == key);
        }

        public List<Config> GetAllConfig()
        {
            return _db.Table<Config>().ToList();
        }

        public void UpdateConfig(Config config)
        {
            _db.Update(config);
        }
        #endregion
    }
}

