using MySql.Data.MySqlClient;
using QLyNhaHang.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QlyNhaHang.Models;

namespace QlyNhaHang.DAL
{
    public class TableDAL
    {
        /// <summary>
        /// Lấy tất cả bàn ăn
        /// </summary>
        public List<BanAn> GetAll()
        {
            List<BanAn> list = new List<BanAn>();

            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                string query = @"SELECT MaBan, TenBan, SoCho, TrangThai 
                               FROM BanAn 
                               ORDER BY MaBan";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new BanAn
                            {
                                MaBan = Convert.ToInt32(reader["MaBan"]),
                                TenBan = reader["TenBan"].ToString(),
                                SoCho = Convert.ToInt32(reader["SoCho"]),
                                TrangThai = reader["TrangThai"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Thêm mới bàn ăn
        /// </summary>
        public bool Insert(BanAn ban)
        {
            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                string query = "INSERT INTO BanAn (TenBan, SoCho, TrangThai) VALUES (@TenBan, @SoCho, @TrangThai)";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TenBan", ban.TenBan);
                    cmd.Parameters.AddWithValue("@SoCho", ban.SoCho);
                    cmd.Parameters.AddWithValue("@TrangThai", ban.TrangThai);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Cập nhật bàn ăn
        /// </summary>
        public bool Update(BanAn ban)
        {
            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                string query = @"UPDATE BanAn 
                               SET TenBan = @TenBan, SoCho = @SoCho, TrangThai = @TrangThai 
                               WHERE MaBan = @MaBan";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TenBan", ban.TenBan);
                    cmd.Parameters.AddWithValue("@SoCho", ban.SoCho);
                    cmd.Parameters.AddWithValue("@TrangThai", ban.TrangThai);
                    cmd.Parameters.AddWithValue("@MaBan", ban.MaBan);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Xóa bàn ăn
        /// </summary>
        public bool Delete(int maBan)
        {
            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                string query = "DELETE FROM BanAn WHERE MaBan = @MaBan";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaBan", maBan);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
