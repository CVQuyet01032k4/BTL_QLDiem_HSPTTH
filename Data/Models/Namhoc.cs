namespace BTL_QLDiem_HSPTTH.Data.Models
{
    public class Namhoc
    {
        public int Id { get; set; }
        public string Tennamhoc { get; set; } // 2023-2024
        public DateTime Tungay { get; set; }
        public DateTime Denngay { get; set; }
        public bool IsActive { get; set; } // Năm học hiện tại
    }
}

