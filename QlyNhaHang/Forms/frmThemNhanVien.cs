using QlyNhaHang.BLL;
using QlyNhaHang.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QlyNhaHang.Forms
{
    public partial class frmThemNhanVien : Form
    {
        private readonly NhanVienService _service = new NhanVienService();
        private NhanVien _nv;
        private bool _isEdit;
        public frmThemNhanVien()
        {
            InitializeComponent();
            this.Text = "Thêm nhân viên mới";
        }

        private void frmThemNhanVien_Load(object sender, EventArgs e)
        {
            cboVaiTro.Items.Clear();
            cboVaiTro.Items.AddRange(new[] { "Admin", "Thu ngân", "Phục vụ", "Bếp" });
            cboVaiTro.SelectedIndex = 0;
            LoadComboBoxVaiTro();          
            txtMatKhau.UseSystemPasswordChar = true;

            if (_isEdit)
            {
                txtTenNhanVien.Text = _nv.HoTen;
                txtTaiKhoan.Text = _nv.TaiKhoan;
                txtDienThoai.Text = _nv.DienThoai;
                cboVaiTro.SelectedItem = _nv.VaiTro;
            }
        }
        private void LoadComboBoxVaiTro()
        {
            cboVaiTro.Items.Clear();
            cboVaiTro.Items.AddRange(new string[] { "Admin", "ThuNgan", "PhucVu", "Bep" });
            cboVaiTro.SelectedIndex = 0;
        }
        private void cboDanhMuc_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenNhanVien.Text) ||
                string.IsNullOrWhiteSpace(txtTaiKhoan.Text) ||
                string.IsNullOrWhiteSpace(txtMatKhau.Text))
            {
                MessageBox.Show("Họ tên, Tài khoản và Mật khẩu không được để trống!", "Thông báo");
                return;
            }

            try
            {
                var nhanVienMoi = new NhanVien
                {
                    HoTen = txtTenNhanVien.Text.Trim(),
                    TaiKhoan = txtTaiKhoan.Text.Trim(),
                    MatKhau = txtMatKhau.Text.Trim(),
                    VaiTro = cboVaiTro.Text,
                    DienThoai = txtDienThoai.Text.Trim(),
                };

                bool success = _service.ThemNhanVien(nhanVienMoi);

                if (success)
                {
                    MessageBox.Show("Thêm nhân viên thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Thêm nhân viên thất bại!", "Lỗi");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm nhân viên:\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkHienMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            txtMatKhau.PasswordChar = chkHienMatKhau.Checked ? '\0' : '*';
        }
    }
}
