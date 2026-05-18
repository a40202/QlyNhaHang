using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QlyNhaHang.DAL;
using QlyNhaHang.Models;

namespace QlyNhaHang.BLL
{
    public class NhanVienService
    {
        private readonly NhanVienDAL _dal;

        public NhanVienService()
        {
            _dal = new NhanVienDAL();
        }
        public bool ThemNhanVien(NhanVien nhanVien)
        {
            if (string.IsNullOrWhiteSpace(nhanVien.HoTen) ||
                string.IsNullOrWhiteSpace(nhanVien.TaiKhoan) ||
                string.IsNullOrWhiteSpace(nhanVien.MatKhau))
            {
                throw new Exception("Họ tên, Tài khoản và Mật khẩu không được để trống!");
            }

            return _dal.ThemNhanVien(nhanVien);
        }
        public NhanVien GetById(int maNV)
        {
            return _dal.GetById(maNV);
        }

        public bool SuaNhanVien(NhanVien nhanVien)
        {
            return _dal.SuaNhanVien(nhanVien);
        }
        public NhanVien DangNhap(string taiKhoan, string matKhau)
        {           
            if (string.IsNullOrWhiteSpace(taiKhoan) || string.IsNullOrWhiteSpace(matKhau))
                return null;

            return _dal.DangNhap(taiKhoan, matKhau);
        }
        public bool DoiMatKhau(int maNV, string matKhauCu, string matKhauMoi)
        {
            return _dal.DoiMatKhau(maNV, matKhauCu, matKhauMoi);
        }
        public List<NhanVien> GetAllNhanVien()
        {
            return _dal.GetAll();
        }

        public List<NhanVien> TimKiemNhanVien(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return GetAllNhanVien();

            return _dal.TimKiem(keyword);
        }
        public bool XoaNhanVien(int maNV)
        {
            return _dal.XoaNhanVien(maNV);
        }     
    }
}
