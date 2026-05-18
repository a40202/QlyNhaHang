using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QlyNhaHang.Models
{
    public class NhanVien
    {
        public int MaNV { get; set; }
        public string HoTen { get; set; }
        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }        
        public string VaiTro { get; set; }         
        public string DienThoai { get; set; }
        public NhanVien() { }
        public NhanVien(int maNV, string hoTen, string taiKhoan, string vaiTro)
        {
            MaNV = maNV;
            HoTen = hoTen;
            TaiKhoan = taiKhoan;
            VaiTro = vaiTro;
        }
    }
}
