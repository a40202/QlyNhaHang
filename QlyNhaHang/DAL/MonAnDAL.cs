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
        /// Lấy tất cả món ăn

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
        public bool CapNhatMonAn(MonAn monAn)
        {
            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                string query = @"UPDATE MonAn 
                               SET TenMon = @TenMon, Gia = @Gia, MoTa = @MoTa, 
                                   MaDanhMuc = @MaDanhMuc, TrangThai = @TrangThai 
                               WHERE MaMon = @MaMon";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaMon", monAn.MaMon);
                    cmd.Parameters.AddWithValue("@TenMon", monAn.TenMon);
                    cmd.Parameters.AddWithValue("@Gia", monAn.Gia);
                    cmd.Parameters.AddWithValue("@MoTa", monAn.MoTa ?? "");
                    cmd.Parameters.AddWithValue("@MaDanhMuc", monAn.MaDanhMuc);
                    cmd.Parameters.AddWithValue("@TrangThai", monAn.TrangThai);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool ThemMonAn(MonAn monAn)
        {
            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                string query = @"INSERT INTO MonAn (TenMon, Gia, MoTa, MaDanhMuc, TrangThai) 
                               VALUES (@TenMon, @Gia, @MoTa, @MaDanhMuc, @TrangThai)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TenMon", monAn.TenMon);
                    cmd.Parameters.AddWithValue("@Gia", monAn.Gia);
                    cmd.Parameters.AddWithValue("@MoTa", monAn.MoTa ?? "");
                    cmd.Parameters.AddWithValue("@MaDanhMuc", monAn.MaDanhMuc);
                    cmd.Parameters.AddWithValue("@TrangThai", monAn.TrangThai);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        /// Tìm kiếm món ăn theo từ khóa và danh mục
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
                    using (MySqlDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            return new MonAn
                            {
                                MaMon = Convert.ToInt32(r["MaMon"]),
                                TenMon = r["TenMon"].ToString(),
                                Gia = Convert.ToDecimal(r["Gia"]),
                                MoTa = r["MoTa"]?.ToString(),
                                MaDanhMuc = Convert.ToInt32(r["MaDanhMuc"]),
                                TenDanhMuc = r["TenDanhMuc"]?.ToString(),
                                TrangThai = Convert.ToBoolean(r["TrangThai"])
                            };
                        }
                    }
                }
            }
            return null;
        }
        public List<MonAn> GetAllBy(string tuKhoa = "")
        {
            var list = new List<MonAn>();

            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                string query = @"
                    SELECT m.*, dm.TenDanhMuc 
                    FROM MonAn m
                    LEFT JOIN DanhMuc dm ON m.MaDanhMuc = dm.MaDanhMuc
                    WHERE 1=1";

                if (!string.IsNullOrWhiteSpace(tuKhoa))
                {
                    query += " AND (m.TenMon LIKE @TuKhoa OR m.MoTa LIKE @TuKhoa)";
                }

                query += " ORDER BY m.TenMon ASC";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    if (!string.IsNullOrWhiteSpace(tuKhoa))
                        cmd.Parameters.AddWithValue("@TuKhoa", "%" + tuKhoa + "%");

                    conn.Open();
                    using (MySqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            list.Add(new MonAn
                            {
                                MaMon = Convert.ToInt32(r["MaMon"]),
                                TenMon = r["TenMon"].ToString(),
                                Gia = Convert.ToDecimal(r["Gia"]),
                                MoTa = r["MoTa"]?.ToString(),
                                MaDanhMuc = Convert.ToInt32(r["MaDanhMuc"]),
                                TenDanhMuc = r["TenDanhMuc"]?.ToString(),
                                TrangThai = Convert.ToBoolean(r["TrangThai"])
                            });
                        }
                    }
                }
            }
            return list;
        }
        public bool XoaMonAn(int maMon)
        {
            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                string query = "DELETE FROM MonAn WHERE MaMon = @MaMon";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaMon", maMon);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}