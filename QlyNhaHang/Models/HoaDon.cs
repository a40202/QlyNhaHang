using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QlyNhaHang.Models
{
    public class HoaDon
    {
        public int MaHD { get; set; }
        public int MaBan { get; set; }
        public int MaNV { get; set; }
        public decimal TongTien { get; set; }
        public decimal GiamGia { get; set; }
        public decimal ThanhToan { get; set; }
        public string TrangThai { get; set; }
        public DateTime NgayLap { get; set; }
        public string TenNhanVien { get; set; }
        public string TenBan { get; set; }
    }
}