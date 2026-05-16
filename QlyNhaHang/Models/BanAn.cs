using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QlyNhaHang.Models
{
    /// <summary>
    /// Lớp đại diện cho Bàn ăn
    /// </summary>
    public class BanAn
    {
        public int MaBan { get; set; }
        public string TenBan { get; set; }
        public int SoCho { get; set; }
        public string TrangThai { get; set; }   // Trong, DangSuDung, DatTruoc

        public BanAn() { }

        public BanAn(int maBan, string tenBan, int soCho, string trangThai)
        {
            MaBan = maBan;
            TenBan = tenBan;
            SoCho = soCho;
            TrangThai = trangThai;
        }
    }
}
