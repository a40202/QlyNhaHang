using MySql.Data.MySqlClient;
using QlyNhaHang.Models;
using QLyNhaHang.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                    SELECT m.MaMon, m.TenMon, m.MaDanhMuc, m.Gia, 
                           m.MoTa, m.HinhAnh, m.TrangThai,
                           dm.TenDanhMuc
                    FROM MonAn m
                    LEFT JOIN DanhMuc dm ON m.MaDanhMuc = dm.MaDanhMuc
                    WHERE m.TrangThai = 1 
                    ORDER BY m.TenMon ASC";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    try
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
                                    TenDanhMuc = reader["TenDanhMuc"]?.ToString() ?? "Không phân loại",
                                    Gia = Convert.ToDecimal(reader["Gia"]),
                                    MoTa = reader["MoTa"]?.ToString(),
                                    HinhAnh = reader["HinhAnh"]?.ToString(),
                                    TrangThai = Convert.ToBoolean(reader["TrangThai"])
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi kết nối database trong MonAnDAL.GetAll():\n" + ex.Message,
                            "Lỗi DAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Tìm kiếm món ăn theo từ khóa và danh mục
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
                    WHERE m.TrangThai = 1";

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
                                TenDanhMuc = reader["TenDanhMuc"]?.ToString() ?? "",
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
        /// Lấy món ăn theo ID
        /// </summary>
        public MonAn GetById(int maMon)
        {
            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                string query = @"
                    SELECT m.*, dm.TenDanhMuc 
                    FROM MonAn m 
                    LEFT JOIN DanhMuc dm ON m.MaDanhMuc = dm.MaDanhMuc 
                    WHERE m.MaMon = @MaMon";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaMon", maMon);
                    conn.Open();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new MonAn
                            {
                                MaMon = Convert.ToInt32(reader["MaMon"]),
                                TenMon = reader["TenMon"].ToString(),
                                MaDanhMuc = Convert.ToInt32(reader["MaDanhMuc"]),
                                TenDanhMuc = reader["TenDanhMuc"]?.ToString() ?? "",
                                Gia = Convert.ToDecimal(reader["Gia"]),
                                MoTa = reader["MoTa"]?.ToString(),
                                HinhAnh = reader["HinhAnh"]?.ToString(),
                                TrangThai = Convert.ToBoolean(reader["TrangThai"])
                            };
                        }
                    }
                }
            }
            return null;
        }

        // Bạn có thể thêm Insert, Update, Delete sau...
    }
}