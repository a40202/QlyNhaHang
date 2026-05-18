using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QlyNhaHang.DAL;
using QlyNhaHang.Models;

namespace QlyNhaHang.BLL
{
    internal class DanhMucService
    {
        private readonly DanhMucDAL _dal = new DanhMucDAL();

        public List<DanhMuc> GetAll()
        {
            return _dal.GetAll();
        }

        public bool ThemDanhMuc(DanhMuc dm)
        {
            if (string.IsNullOrWhiteSpace(dm.TenDanhMuc))
                throw new Exception("Tên danh mục không được để trống");

            return _dal.ThemDanhMuc(dm);
        }

        public bool CapNhatDanhMuc(DanhMuc dm)
        {
            if (dm.MaDanhMuc <= 0)
                throw new Exception("Mã danh mục không hợp lệ");

            if (string.IsNullOrWhiteSpace(dm.TenDanhMuc))
                throw new Exception("Tên danh mục không được để trống");

            return _dal.CapNhatDanhMuc(dm);
        }
    }
}
