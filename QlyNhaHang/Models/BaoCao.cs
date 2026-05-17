using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QlyNhaHang.Models
{
    // Báo cáo hôm nay
    public class BaoCaoHomNay
    {
        public decimal TongDoanhThu { get; set; }
        public int SoHoaDon { get; set; }
        public int SoBanDaSuDung { get; set; }
        public List<TopMonAn> Top5MonAn { get; set; } = new List<TopMonAn>();
    }

    // Báo cáo theo khoảng thời gian
    public class BaoCaoTheoNgay
    {
        public decimal TongDoanhThu { get; set; }
        public int SoHoaDon { get; set; }
        public List<HoaDon> DanhSachHoaDon { get; set; } = new List<HoaDon>();
    }

    // Top món ăn bán chạy
    public class TopMonAn
    {
        public string TenMon { get; set; }
        public int SoLuong { get; set; }
        public decimal ThanhTien { get; set; }
    }
    public class BaoCaoTongHop
    {
        public decimal TongDoanhThu { get; set; }
        public int SoHoaDon { get; set; }
        public decimal TrungBinhHoaDon { get; set; }
        public decimal TongGiamGia { get; set; }

        public List<DoanhThuThang> DoanhThuTheoThang { get; set; } = new List<DoanhThuThang>();
        public List<CoCauDoanhThu> CoCauDoanhThu { get; set; } = new List<CoCauDoanhThu>();
        public List<ChiTietThang> ChiTietTheoThang { get; set; } = new List<ChiTietThang>();
        public List<TopMonAn> TopMonAn { get; set; } = new List<TopMonAn>();
    }
    public class DoanhThuThang
    {
        public string Thang { get; set; }           // Ví dụ: "T1/2025"
        public decimal DoanhThu { get; set; }
        public int SoHoaDon { get; set; }
        public decimal TangTruong { get; set; }     // % tăng/giảm so với tháng trước
    }
    public class CoCauDoanhThu
    {
        public string Loai { get; set; }            // Món chính, Đồ uống, Khai vị...
        public decimal TyLe { get; set; }           // Phần trăm
        public decimal DoanhThu { get; set; }
    }
    public class ChiTietThang
    {
        public string Thang { get; set; }
        public decimal DoanhThu { get; set; }
        public int SoHoaDon { get; set; }
        public decimal TrungBinhHD { get; set; }
        public decimal TangTruong { get; set; }     // % so với tháng trước
    }
    public class BaoCaoThang
    {
        public int Thang { get; set; }
        public string TenThang { get; set; }        // Ví dụ: "Tháng 1"
        public decimal DoanhThu { get; set; }
        public int SoHoaDon { get; set; }
        public decimal TangTruong { get; set; }     // % so với tháng trước
    }

    public class BaoCaoNgay
    {
        public DateTime Ngay { get; set; }
        public string NgayHienThi { get; set; }
        public decimal DoanhThu { get; set; }
        public int SoHoaDon { get; set; }
        public decimal TangTruong { get; set; } // ← thêm nếu chưa có
    }
}