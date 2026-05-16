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
    public class MonAnDAL
    {
        /// <summary>
        /// Lấy tất cả món ăn
        /// </summary>
        public List<MonAn> GetAll()
        {
            List<MonAn> list = new List<MonAn>();

            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                string query = @"
                    SELECT m.*, dm.TenDanhMuc 
                    FROM MonAn m 
                    LEFT JOIN DanhMuc dm ON m.MaDanhMuc = dm.MaDanhMuc 
                    ORDER BY m.MaMon DESC";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new MonAn
                            {
                                MaMon = Convert.ToInt32(reader["MaMon"]),
                                TenMon = reader["TenMon"].ToString(),
                                MaDanhMuc = Convert.ToInt32(reader["MaDanhMuc"]),
                                TenDanhMuc = reader["TenDanhMuc"].ToString(),
                                Gia = Convert.ToDecimal(reader["Gia"]),
                                MoTa = reader["MoTa"]?.ToString(),
                                HinhAnh = reader["HinhAnh"]?.ToString(),
                                TrangThai = Convert.ToBoolean(reader["TrangThai"])
                            });
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Tìm kiếm món ăn
        /// </summary>
        public List<MonAn> Search(string keyword, int maDanhMuc = 0)
        {
            List<MonAn> list = new List<MonAn>();

            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                string query = @"
                    SELECT m.*, dm.TenDanhMuc 
                    FROM MonAn m 
                    LEFT JOIN DanhMuc dm ON m.MaDanhMuc = dm.MaDanhMuc 
                    WHERE 1=1";

                if (!string.IsNullOrEmpty(keyword))
                    query += " AND m.TenMon LIKE @keyword";

                if (maDanhMuc > 0)
                    query += " AND m.MaDanhMuc = @maDanhMuc";

                query += " ORDER BY m.MaMon DESC";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    if (!string.IsNullOrEmpty(keyword))
                        cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

                    if (maDanhMuc > 0)
                        cmd.Parameters.AddWithValue("@maDanhMuc", maDanhMuc);

                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new MonAn
                            {
                                MaMon = Convert.ToInt32(reader["MaMon"]),
                                TenMon = reader["TenMon"].ToString(),
                                MaDanhMuc = Convert.ToInt32(reader["MaDanhMuc"]),
                                TenDanhMuc = reader["TenDanhMuc"].ToString(),
                                Gia = Convert.ToDecimal(reader["Gia"]),
                                MoTa = reader["MoTa"]?.ToString(),
                                TrangThai = Convert.ToBoolean(reader["TrangThai"])
                            });
                        }
                    }
                }
            }
            return list;
        }

        // Bạn có thể thêm Insert, Update, Delete sau...
    }
}