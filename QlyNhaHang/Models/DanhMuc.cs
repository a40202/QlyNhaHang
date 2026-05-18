using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QlyNhaHang.Models
{
    internal class DanhMuc
    {
        public int MaDanhMuc { get; set; }
        public string TenDanhMuc { get; set; }

        public DanhMuc() { }

        public DanhMuc(int maDanhMuc, string tenDanhMuc)
        {
            MaDanhMuc = maDanhMuc;
            TenDanhMuc = tenDanhMuc;
        }

    }
}
