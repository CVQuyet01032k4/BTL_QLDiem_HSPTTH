namespace BTL_QLDiem_HSPTTH.Data.Models
{
    public class Config
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }

    // Cấu hình hệ số điểm
    public static class DiemConfig
    {
        public const string HeSoMiemg = "HE_SO_MIENG";
        public const string HeSo15Phut = "HE_SO_15_PHUT";
        public const string HeSo1Tiet = "HE_SO_1_TIET";
        public const string HeSoThi = "HE_SO_THI";
    }

    // Xếp loại học lực
    public static class XepLoai
    {
        public static string GetHocLuc(double diemTB)
        {
            if (diemTB >= 8.0) return "Giỏi";
            if (diemTB >= 6.5) return "Khá";
            if (diemTB >= 5.0) return "Trung bình";
            if (diemTB >= 3.5) return "Yếu";
            return "Kém";
        }

        public static string GetHocLucHK(double diemTB)
        {
            if (diemTB >= 8.0) return "Giỏi";
            if (diemTB >= 6.5) return "Khá";
            if (diemTB >= 5.0) return "Trung bình";
            if (diemTB >= 3.5) return "Yếu";
            return "Kém";
        }
    }
}

