using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QlyNhaHang.DAL;
using QlyNhaHang.Models;
    
namespace QlyNhaHang.BLL
{
    public class MonAnService
    {
        private readonly MonAnDAL _dal;

        public MonAnService()
        {
            _dal = new MonAnDAL();
        }

        public List<MonAn> GetAll()
        {
            return _dal.GetAll();
        }
        public MonAn GetById(int maMon)
        {
            if (maMon <= 0)
                throw new Exception("Mã món ăn không hợp lệ");

            return _dal.GetById(maMon);
        }
        public List<MonAn> TimKiemMonAn(string keyword, int maDanhMuc = 0)
        {
            return _dal.Search(keyword, maDanhMuc);
        }
        public bool CapNhatMonAn(MonAn monAn)
        {
            if (monAn.MaMon <= 0)
                throw new Exception("Mã món ăn không hợp lệ");

            if (string.IsNullOrWhiteSpace(monAn.TenMon))
                throw new Exception("Tên món ăn không được để trống");

            return _dal.CapNhatMonAn(monAn);
        }
        public bool XoaMonAn(int maMon)
        {
            if (maMon <= 0)
                throw new Exception("Mã món ăn không hợp lệ");

            return _dal.XoaMonAn(maMon);
        }
        public bool ThemMonAn(MonAn monAn)
        {
            if (string.IsNullOrWhiteSpace(monAn.TenMon))
                throw new Exception("Tên món ăn không được để trống");

            if (monAn.Gia <= 0)
                throw new Exception("Giá món ăn phải lớn hơn 0");

            return _dal.ThemMonAn(monAn);
        }
        public List<MonAn> GetAllBy(string tuKhoa = "")
        {
            return _dal.GetAllBy(tuKhoa);
        }
    }
}