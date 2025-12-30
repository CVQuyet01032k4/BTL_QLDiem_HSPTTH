namespace BTL_QLDiem_HSPTTH.Data.Models
{
    public class Diem
    {
        public int Id { get; set; }
        public int HocsinhId { get; set; }
        public int MonhocId { get; set; }
        public int HockyId { get; set; }
        public int LoaiDiem { get; set; } // 1: Miệng, 2: 15 phút, 3: 1 tiết, 4: Thi
        public int HeSo { get; set; } // 1, 2, 3
        public double? DiemSo { get; set; }
        public DateTime NgayNhap { get; set; }
        public int GiaovienId { get; set; } // Giáo viên nhập điểm
        public string GhiChu { get; set; }
    }

    public enum LoaiDiem
    {
        Miemg = 1,
        MuoiLamPhut = 2,
        MotTiet = 3,
        Thi = 4
    }
}

