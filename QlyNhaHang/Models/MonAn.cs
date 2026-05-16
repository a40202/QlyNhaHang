using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QlyNhaHang.Models
{
    /// <summary>
    /// Lớp đại diện cho Món ăn
    /// </summary>
    public class MonAn
    {
        public int MaMon { get; set; }
        public string TenMon { get; set; }
        public int MaDanhMuc { get; set; }
        public string TenDanhMuc { get; set; }   // Dùng để hiển thị
        public decimal Gia { get; set; }
        public string MoTa { get; set; }
        public string HinhAnh { get; set; }
        public bool TrangThai { get; set; }

        public MonAn() { }

        public MonAn(int maMon, string tenMon, decimal gia)
        {
            MaMon = maMon;
            TenMon = tenMon;
            Gia = gia;
            TrangThai = true;
        }
    }
}