namespace BTL_QLDiem_HSPTTH.Data.Models
{
    public class Lophoc
    {
        public int Id { get; set; }
        public string Tenlop { get; set; } // 10A1, 10A2, ...
        public int KhoidId { get; set; } // Khối 10, 11, 12
        public int? GiaovienCNId { get; set; } // Giáo viên chủ nhiệm
    }
}

