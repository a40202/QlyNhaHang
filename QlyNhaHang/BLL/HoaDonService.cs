using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QlyNhaHang.DAL;
using QlyNhaHang.Models;

namespace QlyNhaHang.BLL
{
    public class HoaDonService
    {
        private readonly HoaDonDAL _dal;

        public HoaDonService()
        {
            _dal = new HoaDonDAL();
        }
        public List<ChiTietHoaDon> GetChiTietHoaDonByBan(int maBan)
        {
            return _dal.GetChiTietByBan(maBan);
        }
        public List<ChiTietHoaDon> GetChiTietByMaHD(int maHD)
        {
            return _dal.GetChiTietByMaHD(maHD);
        }
        public List<HoaDon> GetAllHoaDon()
        {
            return _dal.GetAllHoaDon();
        }
        public bool ThemHoaDon(HoaDon hoaDon)
        {
            return _dal.ThemHoaDon(hoaDon);
        }
        public HoaDon GetHoaDonById(int maHD)
        {
            return _dal.GetHoaDonById(maHD);
        }
        public bool HuyHoaDon(int maHD)
        {
            return _dal.HuyHoaDon(maHD);
        }
        public bool CapNhatGiamGia(int maHD, decimal giamGia)
        {
            return _dal.CapNhatGiamGia(maHD, giamGia);
        }
        public bool ThanhToanHoaDon(int maBan, decimal giamGia = 0)
        {
            return _dal.ThanhToan(maBan, giamGia);
        }
    }
}