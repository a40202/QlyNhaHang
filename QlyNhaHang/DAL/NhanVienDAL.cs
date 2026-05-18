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
    public class NhanVienDAL
    {
        /// <summary>
        /// Đăng nhập hệ thống
        /// </summary>
        public NhanVien DangNhap(string taiKhoan, string matKhau)
        {
            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                string query = @"SELECT MaNV, HoTen, TaiKhoan, VaiTro 
                               FROM NhanVien 
                               WHERE TaiKhoan = @TaiKhoan 
                                 AND MatKhau = @MatKhau 
                                 ";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TaiKhoan", taiKhoan);
                    cmd.Parameters.AddWithValue("@MatKhau", matKhau); 

                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new NhanVien
                            {
                                MaNV = Convert.ToInt32(reader["MaNV"]),
                                HoTen = reader["HoTen"].ToString(),
                                TaiKhoan = reader["TaiKhoan"].ToString(),
                                VaiTro = reader["VaiTro"].ToString()
                            };
                        }
                    }
                }
            }
            return null; 
        }
        public bool DoiMatKhau(int maNV, string matKhauCu, string matKhauMoi)
        {
            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                string query = @"
            UPDATE NhanVien 
            SET MatKhau = @MatKhauMoi 
            WHERE MaNV = @MaNV AND MatKhau = @MatKhauCu";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", maNV);
                    cmd.Parameters.AddWithValue("@MatKhauCu", matKhauCu);
                    cmd.Parameters.AddWithValue("@MatKhauMoi", matKhauMoi);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }


        public List<NhanVien> GetAll()
        {
            List<NhanVien> list = new List<NhanVien>();

            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                string query = @"
                    SELECT MaNV, HoTen, TaiKhoan, VaiTro, DienThoai
                    FROM NhanVien 
                    ORDER BY MaNV DESC";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new NhanVien
                            {
                                MaNV = Convert.ToInt32(reader["MaNV"]),
                                HoTen = reader["HoTen"].ToString(),
                                TaiKhoan = reader["TaiKhoan"].ToString(),
                                VaiTro = reader["VaiTro"].ToString(),
                                DienThoai = reader["DienThoai"].ToString(),
                            });
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Tìm kiếm nhân viên
        /// </summary>
        public List<NhanVien> TimKiem(string keyword)
        {
            List<NhanVien> list = new List<NhanVien>();

            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                string query = @"
                    SELECT MaNV, HoTen, TaiKhoan, VaiTro, DienThoai
                    FROM NhanVien 
                    WHERE HoTen LIKE @keyword 
                       OR TaiKhoan LIKE @keyword 
                       OR DienThoai LIKE @keyword
                    ORDER BY MaNV DESC";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                    conn.Open();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new NhanVien
                            {
                                MaNV = Convert.ToInt32(reader["MaNV"]),
                                HoTen = reader["HoTen"].ToString(),
                                TaiKhoan = reader["TaiKhoan"].ToString(),
                                VaiTro = reader["VaiTro"].ToString(),
                                DienThoai = reader["DienThoai"].ToString(),
                            });
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Xóa nhân viên (cập nhật trạng thái)
        /// </summary>
        public bool XoaNhanVien(int maNV)
        {
            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                string query = "DELETE FROM NhanVien WHERE MaNV = @MaNV";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", maNV);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public NhanVien GetById(int maNV)
        {
            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                string query = @"
            SELECT MaNV, HoTen, TaiKhoan, MatKhau, VaiTro, DienThoai
            FROM NhanVien WHERE MaNV = @MaNV";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", maNV);
                    conn.Open();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new NhanVien
                            {
                                MaNV = Convert.ToInt32(reader["MaNV"]),
                                HoTen = reader["HoTen"].ToString(),
                                TaiKhoan = reader["TaiKhoan"].ToString(),
                                MatKhau = reader["MatKhau"].ToString(),
                                VaiTro = reader["VaiTro"].ToString(),
                                DienThoai = reader["DienThoai"].ToString()
                            };
                        }
                    }
                }
            }
            return null;
        }

        public bool SuaNhanVien(NhanVien nhanVien)
        {
            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                string query = @"
            UPDATE NhanVien 
            SET HoTen = @HoTen,
                TaiKhoan = @TaiKhoan,
                MatKhau = @MatKhau,
                VaiTro = @VaiTro,
                DienThoai = @DienThoai
            WHERE MaNV = @MaNV";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", nhanVien.MaNV);
                    cmd.Parameters.AddWithValue("@HoTen", nhanVien.HoTen);
                    cmd.Parameters.AddWithValue("@TaiKhoan", nhanVien.TaiKhoan);
                    cmd.Parameters.AddWithValue("@MatKhau", nhanVien.MatKhau);
                    cmd.Parameters.AddWithValue("@VaiTro", nhanVien.VaiTro);
                    cmd.Parameters.AddWithValue("@DienThoai", nhanVien.DienThoai);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool ThemNhanVien(NhanVien nv)
        {
            using (var conn = DataAccess.GetConnection())
            {
                conn.Open();
                string query = @"INSERT INTO NhanVien (HoTen, TaiKhoan, MatKhau, VaiTro, DienThoai)
                             VALUES (@HoTen, @TaiKhoan, @MatKhau, @VaiTro, @DienThoai)";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@HoTen", nv.HoTen);
                    cmd.Parameters.AddWithValue("@TaiKhoan", nv.TaiKhoan);
                    cmd.Parameters.AddWithValue("@MatKhau", nv.MatKhau);
                    cmd.Parameters.AddWithValue("@VaiTro", nv.VaiTro);
                    cmd.Parameters.AddWithValue("@DienThoai", nv.DienThoai);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
