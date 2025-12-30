namespace BTL_QLDiem_HSPTTH.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public int? HocsinhId { get; set; } // Link đến học sinh nếu là Student hoặc Parent
        public int? GiaovienId { get; set; } // Link đến giáo viên nếu là Teacher
    }
}

