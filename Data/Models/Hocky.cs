namespace BTL_QLDiem_HSPTTH.Data.Models
{
    public class Hocky
    {
        public int Id { get; set; }
        public int NamhocId { get; set; }
        public string Tenhocky { get; set; } // Học kỳ 1, Học kỳ 2
        public int So { get; set; } // 1 hoặc 2
        public DateTime Tungay { get; set; }
        public DateTime Denngay { get; set; }
        public bool IsActive { get; set; } // Học kỳ hiện tại
    }
}

