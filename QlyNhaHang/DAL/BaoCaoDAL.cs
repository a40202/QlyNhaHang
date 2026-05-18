using MySql.Data.MySqlClient;
using QlyNhaHang.Models;
using QLyNhaHang.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QlyNhaHang.DAL
{
    internal class BaoCaoDAL
    {
        public BaoCaoHomNay GetBaoCaoHomNay()
        {
            var bc = new BaoCaoHomNay();

            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                conn.Open();
                string sql = @"
                    SELECT SUM(ThanhToan) as TongDoanhThu, COUNT(MaHD) as SoHoaDon 
                    FROM HoaDon 
                    WHERE DATE(NgayLap) = CURDATE() AND TrangThai = 'DaThanhToan'";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    using (MySqlDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            bc.TongDoanhThu = r["TongDoanhThu"] != DBNull.Value ? Convert.ToDecimal(r["TongDoanhThu"]) : 0;
                            bc.SoHoaDon = r["SoHoaDon"] != DBNull.Value ? Convert.ToInt32(r["SoHoaDon"]) : 0;
                        }
                    }
                }
                string sqlBan = "SELECT COUNT(DISTINCT MaBan) FROM HoaDon WHERE DATE(NgayLap) = CURDATE()";
                using (MySqlCommand cmd2 = new MySqlCommand(sqlBan, conn))
                {
                    object rs = cmd2.ExecuteScalar();
                    bc.SoBanDaSuDung = rs != DBNull.Value ? Convert.ToInt32(rs) : 0;
                }

                string sqlTop = @"
                    SELECT m.TenMon, SUM(ct.SoLuong) as SoLuong, SUM(ct.ThanhTien) as ThanhTien
                    FROM ChiTietHoaDon ct
                    JOIN HoaDon hd ON ct.MaHD = hd.MaHD
                    JOIN MonAn m ON ct.MaMon = m.MaMon
                    WHERE DATE(hd.NgayLap) = CURDATE()
                    GROUP BY m.MaMon, m.TenMon
                    ORDER BY SUM(ct.SoLuong) DESC LIMIT 5";

                using (MySqlCommand cmd3 = new MySqlCommand(sqlTop, conn))
                {
                    using (MySqlDataReader r = cmd3.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            bc.Top5MonAn.Add(new TopMonAn
                            {
                                TenMon = r["TenMon"].ToString(),
                                SoLuong = Convert.ToInt32(r["SoLuong"]),
                                ThanhTien = Convert.ToDecimal(r["ThanhTien"])
                            });
                        }
                    }
                }
            }
            return bc;
        }
        //==================================================
        public List<BaoCaoNgay> GetBaoCaoTheoNgayTrongThang(int nam, int thang)
        {
            var list = new List<BaoCaoNgay>();

            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                string query = @"
            SELECT 
                DATE(NgayLap) as Ngay,
                SUM(ThanhToan) as DoanhThu,
                COUNT(MaHD) as SoHoaDon
            FROM HoaDon 
            WHERE YEAR(NgayLap) = @Nam 
              AND MONTH(NgayLap) = @Thang 
              AND TrangThai = 'DaThanhToan'
            GROUP BY DATE(NgayLap)
            ORDER BY Ngay";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nam", nam);
                    cmd.Parameters.AddWithValue("@Thang", thang);

                    conn.Open();
                    using (MySqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            DateTime ngay = Convert.ToDateTime(r["Ngay"]);
                            list.Add(new BaoCaoNgay
                            {
                                Ngay = ngay,
                                NgayHienThi = ngay.ToString("dd/MM/yyyy"),
                                DoanhThu = Convert.ToDecimal(r["DoanhThu"]),
                                SoHoaDon = Convert.ToInt32(r["SoHoaDon"]),
                                TangTruong = 0
                            });
                        }
                    }
                }
                var dictDoanhThu = list.ToDictionary(x => x.Ngay.Date, x => x.DoanhThu);

                foreach (var item in list)
                {
                    DateTime ngayTruoc = item.Ngay.Date.AddDays(-1);

                    if (dictDoanhThu.TryGetValue(ngayTruoc, out decimal prevDoanhThu))
                    {
                        item.TangTruong = prevDoanhThu > 0
                            ? (item.DoanhThu - prevDoanhThu) / prevDoanhThu * 100
                            : 0;
                    }
                    else
                    {
                        item.TangTruong = 0;
                    }
                }
            }

            return list;
        }
        //================================================
        public BaoCaoTongHop GetBaoCaoTongHop()
        {
            var bc = new BaoCaoTongHop();

            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                conn.Open();         
                string sqlTong = @"
                    SELECT 
                        SUM(ThanhToan) as TongDoanhThu,
                        COUNT(MaHD) as SoHoaDon,
                        SUM(GiamGia) as TongGiamGia
                    FROM HoaDon 
                    WHERE TrangThai = 'DaThanhToan' 
                      AND MONTH(NgayLap) = MONTH(CURDATE()) 
                      AND YEAR(NgayLap) = YEAR(CURDATE())";

                using (MySqlCommand cmd = new MySqlCommand(sqlTong, conn))
                {
                    using (MySqlDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            bc.TongDoanhThu = r["TongDoanhThu"] != DBNull.Value ? Convert.ToDecimal(r["TongDoanhThu"]) : 0;
                            bc.SoHoaDon = r["SoHoaDon"] != DBNull.Value ? Convert.ToInt32(r["SoHoaDon"]) : 0;
                            bc.TongGiamGia = r["TongGiamGia"] != DBNull.Value ? Convert.ToDecimal(r["TongGiamGia"]) : 0;
                        }
                    }
                }
                string sqlThang = @"
                    SELECT 
                        CONCAT('T', MONTH(NgayLap)) as Thang,
                        SUM(ThanhToan) as DoanhThu,
                        COUNT(MaHD) as SoHoaDon
                    FROM HoaDon 
                    WHERE TrangThai = 'DaThanhToan' 
                      AND NgayLap >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH)
                    GROUP BY YEAR(NgayLap), MONTH(NgayLap)
                    ORDER BY YEAR(NgayLap), MONTH(NgayLap)";

                using (MySqlCommand cmd2 = new MySqlCommand(sqlThang, conn))
                {
                    using (MySqlDataReader r = cmd2.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            bc.DoanhThuTheoThang.Add(new DoanhThuThang
                            {
                                Thang = r["Thang"].ToString(),
                                DoanhThu = Convert.ToDecimal(r["DoanhThu"]),
                                SoHoaDon = Convert.ToInt32(r["SoHoaDon"])
                            });
                        }
                    }
                }
                string sqlTopMon = @"
                    SELECT m.TenMon, SUM(ct.SoLuong) as SoLuong, SUM(ct.ThanhTien) as ThanhTien
                    FROM ChiTietHoaDon ct
                    JOIN MonAn m ON ct.MaMon = m.MaMon
                    JOIN HoaDon hd ON ct.MaHD = hd.MaHD
                    WHERE MONTH(hd.NgayLap) = MONTH(CURDATE())
                    GROUP BY m.MaMon, m.TenMon
                    ORDER BY SUM(ct.SoLuong) DESC LIMIT 10";

                using (MySqlCommand cmd3 = new MySqlCommand(sqlTopMon, conn))
                {
                    using (MySqlDataReader r = cmd3.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            bc.TopMonAn.Add(new TopMonAn
                            {
                                TenMon = r["TenMon"].ToString(),
                                SoLuong = Convert.ToInt32(r["SoLuong"]),
                                ThanhTien = Convert.ToDecimal(r["ThanhTien"])
                            });
                        }
                    }
                }
            }
            return bc;
        }
        // ============================================
        public BaoCaoTheoNgay GetBaoCaoTheoNgay(DateTime tuNgay, DateTime denNgay)
        {
            var bc = new BaoCaoTheoNgay();

            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                string query = @"
                    SELECT hd.*, nv.HoTen AS TenNhanVien 
                    FROM HoaDon hd
                    LEFT JOIN NhanVien nv ON hd.MaNV = nv.MaNV
                    WHERE DATE(hd.NgayLap) BETWEEN @TuNgay AND @DenNgay
                    ORDER BY hd.MaHD DESC";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TuNgay", tuNgay.Date);
                    cmd.Parameters.AddWithValue("@DenNgay", denNgay.Date);

                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bc.DanhSachHoaDon.Add(new HoaDon
                            {
                                MaHD = Convert.ToInt32(reader["MaHD"]),
                                MaBan = Convert.ToInt32(reader["MaBan"]),
                                MaNV = Convert.ToInt32(reader["MaNV"]),
                                TenNhanVien = reader["TenNhanVien"]?.ToString(),
                                NgayLap = Convert.ToDateTime(reader["NgayLap"]),
                                TongTien = Convert.ToDecimal(reader["TongTien"]),
                                GiamGia = Convert.ToDecimal(reader["GiamGia"]),
                                ThanhToan = Convert.ToDecimal(reader["ThanhToan"]),
                                TrangThai = reader["TrangThai"].ToString()
                            });
                        }
                    }
                }
            }
            return bc;
        }   
 
        //======================
        public List<BaoCaoThang> GetBaoCaoTheoNamm(int nam, int? thang = null)
        {
            var list = new List<BaoCaoThang>();

            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                conn.Open();

                string query = @"
            SELECT 
                MONTH(hd.NgayLap) as Thang,
                SUM(hd.ThanhToan) as DoanhThu,
                COUNT(hd.MaHD) as SoHoaDon
            FROM HoaDon hd
            WHERE YEAR(hd.NgayLap) = @Nam 
              AND hd.TrangThai = 'DaThanhToan'";

                if (thang.HasValue)
                    query += " AND MONTH(hd.NgayLap) = @Thang";

                query += " GROUP BY MONTH(hd.NgayLap) ORDER BY MONTH(hd.NgayLap)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nam", nam);
                    if (thang.HasValue)
                        cmd.Parameters.AddWithValue("@Thang", thang.Value);

                    using (MySqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            list.Add(new BaoCaoThang
                            {
                                Thang = Convert.ToInt32(r["Thang"]),
                                TenThang = "Tháng " + r["Thang"],
                                DoanhThu = Convert.ToDecimal(r["DoanhThu"]),
                                SoHoaDon = Convert.ToInt32(r["SoHoaDon"]),
                                TangTruong = 0
                            });
                        }
                    }
                }

                // =========================================
                if (thang.HasValue && list.Count == 1)
                {           
                    int thangTruoc = thang.Value - 1;
                    int namTruoc = nam;
                    if (thangTruoc == 0) { thangTruoc = 12; namTruoc--; }

                    string prevQuery = @"
                SELECT SUM(hd.ThanhToan) as DoanhThu
                FROM HoaDon hd
                WHERE YEAR(hd.NgayLap) = @Nam
                  AND MONTH(hd.NgayLap) = @Thang
                  AND hd.TrangThai = 'DaThanhToan'";

                    using (MySqlCommand cmd2 = new MySqlCommand(prevQuery, conn))
                    {
                        cmd2.Parameters.AddWithValue("@Nam", namTruoc);
                        cmd2.Parameters.AddWithValue("@Thang", thangTruoc);

                        object result = cmd2.ExecuteScalar();
                        decimal doanhThuTruoc = (result != null && result != DBNull.Value)
                            ? Convert.ToDecimal(result) : 0;

                        if (doanhThuTruoc > 0)
                            list[0].TangTruong = (list[0].DoanhThu - doanhThuTruoc) / doanhThuTruoc * 100;
                    }
                }
                else
                {
                    var dictDoanhThu = list.ToDictionary(x => x.Thang, x => x.DoanhThu);

                    foreach (var item in list)
                    {
                        int thangTruoc = item.Thang - 1;
                        if (thangTruoc == 0) thangTruoc = 12;

                        if (dictDoanhThu.TryGetValue(thangTruoc, out decimal prevDoanhThu))
                        {
                            item.TangTruong = prevDoanhThu > 0
                                ? (item.DoanhThu - prevDoanhThu) / prevDoanhThu * 100
                                : 0;
                        }
                        else
                        {
                            item.TangTruong = 0;
                        }
                    }
                }
            }

            return list;
        }
    }
}
