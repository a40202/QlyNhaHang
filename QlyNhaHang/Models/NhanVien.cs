using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QlyNhaHang.Models
{
    /// <summary>
    /// Lớp đại diện cho thông tin Nhân viên
    /// </summary>
    public class NhanVien
    {
        public int MaNV { get; set; }
        public string HoTen { get; set; }
        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }        // Chỉ dùng trong DAL, không nên expose ra ngoài
        public string VaiTro { get; set; }         // Admin, ThuNgan, PhucVu, Bep
        public string DienThoai { get; set; }
        public bool TrangThai { get; set; }

        public NhanVien() { }

        /// <summary>
        /// Constructor dùng khi đăng nhập thành công
        /// </summary>
        public NhanVien(int maNV, string hoTen, string taiKhoan, string vaiTro)
        {
            MaNV = maNV;
            HoTen = hoTen;
            TaiKhoan = taiKhoan;
            VaiTro = vaiTro;
            TrangThai = true;
        }
    }
}
