using QlyNhaHang.DAL;
using QlyNhaHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QlyNhaHang.BLL
{
    public class BaoCaoService
    {
        private readonly BaoCaoDAL _dal = new BaoCaoDAL();

        public BaoCaoHomNay GetBaoCaoHomNay()
        {
            return _dal.GetBaoCaoHomNay();
        }

        public BaoCaoTheoNgay GetBaoCaoTheoNgay(DateTime tuNgay, DateTime denNgay)
        {
            return _dal.GetBaoCaoTheoNgay(tuNgay, denNgay);
        }
        public BaoCaoTongHop GetBaoCaoTongHop()
        {
            return _dal.GetBaoCaoTongHop();
        }
        public List<BaoCaoThang> GetBaoCaoTheoNamm(int nam, int? thang = null)
        {
            return _dal.GetBaoCaoTheoNamm(nam, thang);
        }
        public List<BaoCaoNgay> GetBaoCaoTheoNgayTrongThang(int nam, int thang)
        {
            return _dal.GetBaoCaoTheoNgayTrongThang(nam, thang);
        }
    }
}