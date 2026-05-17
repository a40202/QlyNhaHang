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
    public class OrderTamDAL
    {
        public List<OrderTam> GetByBan(int maBan)
        {
            List<OrderTam> list = new List<OrderTam>();

            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                string query = @"
                    SELECT ot.*, m.TenMon 
                    FROM OrderTam ot
                    JOIN MonAn m ON ot.MaMon = m.MaMon
                    WHERE ot.MaBan = @MaBan
                    ORDER BY ot.ThoiGian";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaBan", maBan);
                    conn.Open();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new OrderTam
                            {
                                MaOrder = Convert.ToInt32(reader["MaOrder"]),
                                MaBan = Convert.ToInt32(reader["MaBan"]),
                                MaMon = Convert.ToInt32(reader["MaMon"]),
                                TenMon = reader["TenMon"].ToString(),
                                SoLuong = Convert.ToInt32(reader["SoLuong"]),
                                DonGia = Convert.ToDecimal(reader["DonGia"]),
                                ThanhTien = Convert.ToDecimal(reader["SoLuong"]) * Convert.ToDecimal(reader["DonGia"]),
                                ThoiGian = Convert.ToDateTime(reader["ThoiGian"])
                            });
                        }
                    }
                }
            }
            return list;
        }
        //---------------------------------------------------
        public bool ThemMon(int maBan, int maMon, int soLuong, decimal donGia)
        {
            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                string query = @"
                    INSERT INTO OrderTam (MaBan, MaMon, SoLuong, DonGia)
                    VALUES (@MaBan, @MaMon, @SoLuong, @DonGia)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaBan", maBan);
                    cmd.Parameters.AddWithValue("@MaMon", maMon);
                    cmd.Parameters.AddWithValue("@SoLuong", soLuong);
                    cmd.Parameters.AddWithValue("@DonGia", donGia);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        //---------------------------------------------------
        public bool XoaOrderTamByBan(int maBan)
        {
            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                string query = "DELETE FROM OrderTam WHERE MaBan = @MaBan";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaBan", maBan);
                    conn.Open();
                    return cmd.ExecuteNonQuery() >= 0;
                }
            }
        }
        //---------------------------------------------------
        public bool CapNhatTrangThaiBan(int maBan, string trangThai)
        {
            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                string query = "UPDATE BanAn SET TrangThai = @TrangThai WHERE MaBan = @MaBan";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TrangThai", trangThai);
                    cmd.Parameters.AddWithValue("@MaBan", maBan);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        //---------------------------------------------------   
        public bool ChuyenBan(int maBanCu, int maBanMoi)
        {
            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                conn.Open();
                using (MySqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        // Chuyển order sang bàn mới
                        string updateQuery = "UPDATE OrderTam SET MaBan = @MaBanMoi WHERE MaBan = @MaBanCu";
                        using (MySqlCommand cmd = new MySqlCommand(updateQuery, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@MaBanMoi", maBanMoi);
                            cmd.Parameters.AddWithValue("@MaBanCu", maBanCu);
                            cmd.ExecuteNonQuery();
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