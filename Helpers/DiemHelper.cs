using System.Collections.Generic;
using System.Linq;
using BTL_QLDiem_HSPTTH.Data.Models;

namespace BTL_QLDiem_HSPTTH.Helpers
{
    public class DiemMonDisplay
    {
        public Monhoc Mon { get; set; }
        public List<double?> DiemMiemg { get; set; }
        public List<double?> Diem15Phut { get; set; }
        public List<double?> Diem1Tiet { get; set; }
        public double? DiemThi { get; set; }
        public double? DiemTrungBinh { get; set; }
        
        public string DiemMiemgStr => DiemMiemg != null && DiemMiemg.Any(d => d.HasValue)
            ? string.Join(", ", DiemMiemg.Where(d => d.HasValue).Select(d => d!.Value.ToString("0.0"))) 
            : "--";
            
        public string Diem15PhutStr => Diem15Phut != null && Diem15Phut.Any(d => d.HasValue)
            ? string.Join(", ", Diem15Phut.Where(d => d.HasValue).Select(d => d!.Value.ToString("0.0"))) 
            : "--";
            
        public string Diem1TietStr => Diem1Tiet != null && Diem1Tiet.Any(d => d.HasValue)
            ? string.Join(", ", Diem1Tiet.Where(d => d.HasValue).Select(d => d!.Value.ToString("0.0"))) 
            : "--";
            
        public string DiemThiStr => DiemThi.HasValue ? DiemThi.Value.ToString("0.0") : "--";
        public string DiemTrungBinhStr => DiemTrungBinh.HasValue ? DiemTrungBinh.Value.ToString("0.0") : "--";
    }

    public static class DiemHelper
    {
        public static DiemMonDisplay GroupDiemByMon(Monhoc mon, List<Diem> diems, double? diemTB)
        {
            var display = new DiemMonDisplay
            {
                Mon = mon,
                DiemMiemg = new List<double?>(),
                Diem15Phut = new List<double?>(),
                Diem1Tiet = new List<double?>(),
                DiemTrungBinh = diemTB
            };

            var miemgList = diems.Where(d => d.LoaiDiem == (int)LoaiDiem.Miemg && d.DiemSo.HasValue)
                .Select(d => (double?)d.DiemSo.Value).ToList();
            display.DiemMiemg = miemgList;

            var phut15List = diems.Where(d => d.LoaiDiem == (int)LoaiDiem.MuoiLamPhut && d.DiemSo.HasValue)
                .Select(d => (double?)d.DiemSo.Value).ToList();
            display.Diem15Phut = phut15List;

            var tiet1List = diems.Where(d => d.LoaiDiem == (int)LoaiDiem.MotTiet && d.DiemSo.HasValue)
                .Select(d => (double?)d.DiemSo.Value).ToList();
            display.Diem1Tiet = tiet1List;

            var thi = diems.FirstOrDefault(d => d.LoaiDiem == (int)LoaiDiem.Thi && d.DiemSo.HasValue);
            display.DiemThi = thi?.DiemSo;

            return display;
        }
    }
}

