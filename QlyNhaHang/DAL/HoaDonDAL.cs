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
    public class HoaDonDAL
    {
        /// <summary>
        /// Lấy chi tiết hóa đơn theo bàn
        /// </summary>
        /// 
        public List<HoaDon> GetAllHoaDon()
        {
            List<HoaDon> list = new List<HoaDon>();

            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                string query = @"
                    SELECT hd.*, nv.HoTen AS TenNhanVien, ba.TenBan 
                    FROM HoaDon hd
                    LEFT JOIN NhanVien nv ON hd.MaNV = nv.MaNV
                    LEFT JOIN BanAn ba ON hd.MaBan = ba.MaBan
                    ORDER BY hd.MaHD DESC";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new HoaDon
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
            return list;
        }
        public List<ChiTietHoaDon> GetChiTietByMaHD(int maHD)
        {
            List<ChiTietHoaDon> list = new List<ChiTietHoaDon>();

            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                string query = @"
                    SELECT ct.*, m.TenMon 
                    FROM ChiTietHoaDon ct
                    JOIN MonAn m ON ct.MaMon = m.MaMon
                    WHERE ct.MaHD = @MaHD";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaHD", maHD);
                    conn.Open();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new ChiTietHoaDon
                            {
                                MaCT = Convert.ToInt32(reader["MaCT"]),
                                MaHD = Convert.ToInt32(reader["MaHD"]),
                                MaMon = Convert.ToInt32(reader["MaMon"]),
                                TenMon = reader["TenMon"].ToString(),
                                SoLuong = Convert.ToInt32(reader["SoLuong"]),
                                DonGia = Convert.ToDecimal(reader["DonGia"]),
                                ThanhTien = Convert.ToDecimal(reader["ThanhTien"])
                            });
                        }
                    }
                }
            }
            return list;
        }
        public List<ChiTietHoaDon> GetChiTietByBan(int maBan)
        {
            List<ChiTietHoaDon> list = new List<ChiTietHoaDon>();

            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                string query = @"
                    SELECT ct.MaCT, ct.MaHD, ct.MaMon, m.TenMon, ct.SoLuong, 
                           ct.DonGia, ct.ThanhTien
                    FROM ChiTietHoaDon ct
                    JOIN HoaDon hd ON ct.MaHD = hd.MaHD
                    JOIN MonAn m ON ct.MaMon = m.MaMon
                    WHERE hd.MaBan = @MaBan 
                      AND hd.TrangThai = 'ChuaThanhToan'";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaBan", maBan);
                    conn.Open();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new ChiTietHoaDon
                            {
                                MaCT = Convert.ToInt32(reader["MaCT"]),
                                MaHD = Convert.ToInt32(reader["MaHD"]),
                                MaMon = Convert.ToInt32(reader["MaMon"]),
                                TenMon = reader["TenMon"].ToString(),
                                SoLuong = Convert.ToInt32(reader["SoLuong"]),
                                DonGia = Convert.ToDecimal(reader["DonGia"]),
                                ThanhTien = Convert.ToDecimal(reader["ThanhTien"])
                            });
                        }
                    }
                }
            }
            return list;
        }
        public HoaDon GetHoaDonById(int maHD)
        {
            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                string query = @"
            SELECT hd.*, nv.HoTen AS TenNhanVien 
            FROM HoaDon hd 
            LEFT JOIN NhanVien nv ON hd.MaNV = nv.MaNV 
            WHERE hd.MaHD = @MaHD";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaHD", maHD);
                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new HoaDon
                            {
                                MaHD = Convert.ToInt32(reader["MaHD"]),
                                MaBan = Convert.ToInt32(reader["MaBan"]),
                                MaNV = Convert.ToInt32(reader["MaNV"]),
                                NgayLap = Convert.ToDateTime(reader["NgayLap"]),
                                TongTien = Convert.ToDecimal(reader["TongTien"]),
                                GiamGia = Convert.ToDecimal(reader["GiamGia"]),
                                ThanhToan = Convert.ToDecimal(reader["ThanhToan"]),
                                TrangThai = reader["TrangThai"].ToString(),
                                TenNhanVien = reader["TenNhanVien"].ToString()
                            };
                        }
                    }
                }
            }
            return null;
        }
        public bool ThemHoaDon(HoaDon hoaDon)
        {
            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                string query = @"
            INSERT INTO HoaDon (MaBan, MaNV, NgayLap, TongTien, GiamGia, ThanhToan, TrangThai) 
            VALUES (@MaBan, @MaNV, @NgayLap, 0, @GiamGia, 0, 'ChuaThanhToan')";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaBan", hoaDon.MaBan);
                    cmd.Parameters.AddWithValue("@MaNV", hoaDon.MaNV);
                    cmd.Parameters.AddWithValue("@NgayLap", hoaDon.NgayLap);
                    cmd.Parameters.AddWithValue("@GiamGia", hoaDon.GiamGia);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool CapNhatGiamGia(int maHD, decimal giamGia)
        {
            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                string query = "UPDATE HoaDon SET GiamGia = @GiamGia WHERE MaHD = @MaHD";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@GiamGia", giamGia);
                    cmd.Parameters.AddWithValue("@MaHD", maHD);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool HuyHoaDon(int maHD)
        {
            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                conn.Open();
                using (MySqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. Lấy MaBan của hóa đơn
                        int maBan = 0;
                        string getMaBan = "SELECT MaBan FROM HoaDon WHERE MaHD = @MaHD";
                        using (MySqlCommand cmd1 = new MySqlCommand(getMaBan, conn, tran))
                        {
                            cmd1.Parameters.AddWithValue("@MaHD", maHD);
                            object result = cmd1.ExecuteScalar();
                            if (result != null)
                                maBan = Convert.ToInt32(result);
                        }

                        // 2. Xóa chi tiết hóa đơn
                        string deleteChiTiet = "DELETE FROM ChiTietHoaDon WHERE MaHD = @MaHD";
                        using (MySqlCommand cmd2 = new MySqlCommand(deleteChiTiet, conn, tran))
                        {
                            cmd2.Parameters.AddWithValue("@MaHD", maHD);
                            cmd2.ExecuteNonQuery();
                        }

                        // 3. Xóa hóa đơn
                        string deleteHoaDon = "DELETE FROM HoaDon WHERE MaHD = @MaHD AND TrangThai = 'ChuaThanhToan'";
                        using (MySqlCommand cmd3 = new MySqlCommand(deleteHoaDon, conn, tran))
                        {
                            cmd3.Parameters.AddWithValue("@MaHD", maHD);
                            int rows = cmd3.ExecuteNonQuery();

                            if (rows == 0) // Không tìm thấy hoặc đã thanh toán
                            {
                                tran.Rollback();
                                return false;
                            }
                        }

                        // 4. Cập nhật lại trạng thái bàn thành Trống
                        if (maBan > 0)
                        {
                            string updateBan = "UPDATE BanAn SET TrangThai = 'Trống' WHERE MaBan = @MaBan";
                            using (MySqlCommand cmd4 = new MySqlCommand(updateBan, conn, tran))
                            {
                                cmd4.Parameters.AddWithValue("@MaBan", maBan);
                                cmd4.ExecuteNonQuery();
                            }
                        }

                        tran.Commit();
                        return true;
                    }
                    catch
                    {
                        tran.Rollback();
                        return false;
                    }
                }
            }
        }
        /// <summary>
        /// Thanh toán hóa đơn
        /// </summary>
        public bool ThanhToan(int maBan, decimal giamGia = 0)
        {
            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                conn.Open();
                using (MySqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        // Cập nhật trạng thái hóa đơn
                        string query = @"UPDATE HoaDon 
                                       SET TrangThai = 'DaThanhToan', 
                                           ThanhToan = TongTien - @GiamGia,
                                           GiamGia = @GiamGia
                                       WHERE MaBan = @MaBan AND TrangThai = 'ChuaThanhToan'";

                        using (MySqlCommand cmd = new MySqlCommand(query, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@MaBan", maBan);
                            cmd.Parameters.AddWithValue("@GiamGia", giamGia);
                            cmd.ExecuteNonQuery();
                        }

                        // Cập nhật trạng thái bàn thành Trống
                        string updateBan = "UPDATE BanAn SET TrangThai = 'Trống' WHERE MaBan = @MaBan";
                        using (MySqlCommand cmd2 = new MySqlCommand(updateBan, conn, tran))
                        {
                            cmd2.Parameters.AddWithValue("@MaBan", maBan);
                            cmd2.ExecuteNonQuery();
                        }

                        tran.Commit();
                        return true;
                    }
                    catch
                    {
                        tran.Rollback();
                        return false;
                    }
                }
            }
        }
    }
}