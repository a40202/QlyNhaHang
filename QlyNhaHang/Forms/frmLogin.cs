using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QlyNhaHang.BLL;
using QlyNhaHang.Models;
using QlyNhaHang.Helpers;
using QlyNhaHang.DAL;


namespace QlyNhaHang.Forms
{
    public partial class frmLogin : Form
    {
        private readonly NhanVienService _nhanVienService;
        public frmLogin()
        {
            InitializeComponent();
            _nhanVienService = new NhanVienService();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string taiKhoan = txtUsername.Text.Trim();
            string matKhau = txtPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(taiKhoan) || string.IsNullOrWhiteSpace(matKhau))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Tài khoản và Mật khẩu!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                NhanVien nhanVien = _nhanVienService.DangNhap(taiKhoan, matKhau);

                if (nhanVien != null)
                {
                    CurrentUser.SetCurrentUser(nhanVien);

                    MessageBox.Show($"Đăng nhập thành công!\n\n" +
                                  $"Xin chào: {nhanVien.HoTen}\n" +
                                  $"Vai trò: {nhanVien.VaiTro}",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Hide();

                    frmMain mainForm = new frmMain();
                    mainForm.ShowDialog();

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Tài khoản hoặc mật khẩu không chính xác!",
                        "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            txtUsername.Focus();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
