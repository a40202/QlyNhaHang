using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QlyNhaHang.DAL;
using QlyNhaHang.Models;

namespace QlyNhaHang.BLL
{
    /// <summary>
    /// Xử lý logic nghiệp vụ liên quan đến Nhân viên
    /// </summary>
    public class NhanVienService
    {
        private readonly NhanVienDAL _dal;

        public NhanVienService()
        {
            _dal = new NhanVienDAL();
        }

        /// <summary>
        /// Xử lý đăng nhập
        /// </summary>
        /// <param name="taiKhoan">Tài khoản</param>
        /// <param name="matKhau">Mật khẩu</param>
        /// <returns>Thông tin nhân viên nếu thành công, ngược lại null</returns>
        public NhanVien DangNhap(string taiKhoan, string matKhau)
        {
            // Có thể thêm: kiểm tra độ dài, trim, kiểm tra tài khoản bị khóa...
            if (string.IsNullOrWhiteSpace(taiKhoan) || string.IsNullOrWhiteSpace(matKhau))
                return null;

            return _dal.DangNhap(taiKhoan, matKhau);
        }
    }
}
