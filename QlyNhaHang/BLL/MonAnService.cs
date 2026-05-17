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
            return _dal.GetById(maMon);
        }
        public List<MonAn> TimKiemMonAn(string keyword, int maDanhMuc = 0)
        {
            return _dal.Search(keyword, maDanhMuc);
        }

        // Thêm sau: Insert, Update, Delete...
    }
}