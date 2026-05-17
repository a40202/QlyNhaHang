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
        public bool CapNhatHoaDon(int maHD, int maBanMoi, decimal tongTien, decimal giamGia, decimal thanhToan, string trangThai)
        {
            return _dal.CapNhatHoaDon(maHD, maBanMoi, tongTien, giamGia, thanhToan, trangThai);
        }
        public bool ThanhToanHoaDon(int maBan, decimal giamGia = 0)
        {
            return _dal.ThanhToanHoaDon(maBan, giamGia);
        }
        public List<HoaDon> TimKiemHoaDon(string keyword, DateTime tuNgay, DateTime denNgay, string trangThai = "Tất cả")
        {
            return _dal.TimKiemHoaDon(keyword, tuNgay, denNgay, trangThai);
        }
        public bool ThanhToanHoaDonByMaHD(int maHD)
        {
            return _dal.ThanhToanHoaDonByMaHD(maHD);
        }    
    }
}