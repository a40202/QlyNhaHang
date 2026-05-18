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
    internal class DanhMucDAL
    {
        public List<DanhMuc> GetAll()
        {
            var list = new List<DanhMuc>();

            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                string query = "SELECT MaDanhMuc, TenDanhMuc FROM DanhMuc ORDER BY TenDanhMuc";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    conn.Open();
                    using (MySqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            list.Add(new DanhMuc
                            {
                                MaDanhMuc = Convert.ToInt32(r["MaDanhMuc"]),
                                TenDanhMuc = r["TenDanhMuc"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }

        public bool ThemDanhMuc(DanhMuc dm)
        {
            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                string query = @"INSERT INTO DanhMuc (TenDanhMuc) 
                               VALUES (@TenDanhMuc)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TenDanhMuc", dm.TenDanhMuc);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool CapNhatDanhMuc(DanhMuc dm)
        {
            using (MySqlConnection conn = DataAccess.GetConnection())
            {
                string query = @"UPDATE DanhMuc 
                               SET TenDanhMuc = @TenDanhMuc,                            
                               WHERE MaDanhMuc = @MaDanhMuc";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaDanhMuc", dm.MaDanhMuc);
                    cmd.Parameters.AddWithValue("@TenDanhMuc", dm.TenDanhMuc);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
