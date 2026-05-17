using QlyNhaHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QlyNhaHang.Helpers
{
    public static class CurrentUser
    {
        public static int MaNV { get; private set; }
        public static string HoTen { get; private set; }
        public static string TaiKhoan { get; private set; }
        public static string VaiTro { get; private set; }

        public static void SetCurrentUser(NhanVien nv)
        {
            MaNV = nv.MaNV;
            HoTen = nv.HoTen;
            TaiKhoan = nv.TaiKhoan;
            VaiTro = nv.VaiTro;
        }

        public static void Logout()
        {
            MaNV = 0;
            HoTen = "";
            TaiKhoan = "";
            VaiTro = "";
        }

        public static bool IsAdmin() => VaiTro?.ToUpper() == "ADMIN";
        public static bool IsPhucVu() => VaiTro == "Phục vụ";
        public static bool IsThuNgan() => VaiTro == "Thu ngân";
        public static bool IsBep() => VaiTro == "Bếp";
    }
}