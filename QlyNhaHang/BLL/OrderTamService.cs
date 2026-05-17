using QlyNhaHang.DAL;
using QlyNhaHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QlyNhaHang.BLL
{
    public class OrderTamService
    {
        private readonly OrderTamDAL _dal = new OrderTamDAL();
        private readonly BanAnService _banAnService = new BanAnService();
        public List<OrderTam> GetByBan(int maBan)
        {
            return _dal.GetByBan(maBan);
        }
        // -------------Chuyen ban--------------
        public bool ChuyenBan(int maBanCu, int maBanMoi)
        {
            bool success = _dal.ChuyenBan(maBanCu, maBanMoi);

            if (success)
            {
                // Cập nhật trạng thái bàn cũ về Trống
                _banAnService.CapNhatTrangThaiBan(maBanCu, "Trống");

                // Cập nhật trạng thái bàn mới thành Đang sử dụng
                _banAnService.CapNhatTrangThaiBan(maBanMoi, "Đang Sử Dụng");
            }

            return success;
        }

        public bool XoaOrderTamByBan(int maBan)
        {
            return _dal.XoaOrderTamByBan(maBan);
        }
        public bool ThemMon(int maBan, int maMon, int soLuong, decimal donGia)
        {
            bool ok = _dal.ThemMon(maBan, maMon, soLuong, donGia);

            if (ok)
            {
                // Tự động chuyển trạng thái bàn
                _banAnService.CapNhatTrangThaiBan(maBan, "Đang Sử Dụng");
            }

            return ok;
        }
    }
}