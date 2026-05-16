using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QlyNhaHang.DAL;
using QlyNhaHang.Models;

namespace QlyNhaHang.BLL
{
    public class BanAnService
    {
        private readonly TableDAL _dal;

        public BanAnService()
        {
            _dal = new TableDAL();
        }

        public List<BanAn> GetAllBanAn()
        {
            return _dal.GetAll();
        }

        public bool ThemBan(BanAn ban)
        {
            return _dal.Insert(ban);
        }

        public bool SuaBan(BanAn ban)
        {
            return _dal.Update(ban);
        }

        public bool XoaBan(int maBan)
        {
            return _dal.Delete(maBan);
        }
    }
}
