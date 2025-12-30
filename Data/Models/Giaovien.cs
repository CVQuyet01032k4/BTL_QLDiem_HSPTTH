namespace BTL_QLDiem_HSPTTH.Data.Models
{
    public class Giaovien
    {
        public int Id { get; set; }
        public string Hoten { get; set; }
        public string Magiaovien { get; set; }
        public string Sodienthoai { get; set; }
        public string Email { get; set; }
        public int? MonhocId { get; set; } // Môn chuyên dạy
    }
}

