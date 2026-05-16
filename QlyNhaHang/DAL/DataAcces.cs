using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace QLyNhaHang.DAL
{
    /// <summary>
    /// Quản lý kết nối cơ sở dữ liệu
    /// </summary>
    public static class DataAccess
    {
        private static string _connectionString;

        static DataAccess()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        }

        /// <summary>
        /// Lấy kết nối MySQL
        /// </summary>
        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}