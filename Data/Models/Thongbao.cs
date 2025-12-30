namespace BTL_QLDiem_HSPTTH.Data.Models
{
    public class Thongbao
    {
        public int Id { get; set; }
        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
        public DateTime NgayTao { get; set; }
        public int? NguoiGuiId { get; set; } // Id của người gửi (GV hoặc Admin)
        public Role NguoiGuiRole { get; set; }
        public int? HocsinhId { get; set; } // null = gửi tất cả, có giá trị = gửi riêng
        public int? LophocId { get; set; } // null = gửi tất cả, có giá trị = gửi lớp
        public bool IsRead { get; set; }
    }
}

