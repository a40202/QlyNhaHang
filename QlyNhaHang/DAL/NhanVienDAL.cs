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
                    cmd.Parameters.AddWithValue("@MatKhau", matKhau); // Nên hash password trong thực tế

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
            return null; // Đăng nhập thất bại
        }
    }
}
