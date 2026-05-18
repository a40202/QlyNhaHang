using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QlyNhaHang.BLL;
using QlyNhaHang.Models;


namespace QlyNhaHang.Forms
{
    public partial class frmSuaNhanVien : Form
    {
        private readonly NhanVienService _nhanVienService = new NhanVienService();
        private readonly int _maNV;
        private NhanVien _nhanVienHienTai;
        public frmSuaNhanVien(int maNV)
        {
            InitializeComponent();
            _maNV = maNV;
        }

        private void frmSuaNhanVien_Load(object sender, EventArgs e)
        {
            this.Text = $"Sửa Nhân Viên - Mã {_maNV}";
            SetupForm();
            LoadThongTinNhanVien();
            LoadComboBoxVaiTro();
        }

        private void SetupForm()
        {
            cboVaiTro.DropDownStyle = ComboBoxStyle.DropDownList;
            txtMatKhau.UseSystemPasswordChar = true;        
        }
        private void LoadComboBoxVaiTro()
        {
            cboVaiTro.Items.Clear();
            cboVaiTro.Items.AddRange(new string[] { "Admin", "ThuNgan", "PhucVu", "Bep" });
            cboVaiTro.SelectedIndex = 0;
        }
        private void LoadThongTinNhanVien()
        {
            try
            {
                _nhanVienHienTai = _nhanVienService.GetById(_maNV);

                if (_nhanVienHienTai == null)
                {
                    MessageBox.Show("Không tìm thấy nhân viên!", "Lỗi");
                    this.Close();
                    return;
                }

                txtTenNhanVien.Text = _nhanVienHienTai.HoTen;
                txtTaiKhoan.Text = _nhanVienHienTai.TaiKhoan;
                txtMatKhau.Text = _nhanVienHienTai.MatKhau;        // Có thể để trống hoặc hiển thị *
                txtDienThoai.Text = _nhanVienHienTai.DienThoai;
                cboVaiTro.Text = _nhanVienHienTai.VaiTro;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thông tin: " + ex.Message);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenNhanVien.Text) || string.IsNullOrWhiteSpace(txtTaiKhoan.Text))
            {
                MessageBox.Show("Họ tên và Tài khoản không được để trống!", "Thông báo");
                return;
            }

            try
            {
                var nhanVienCapNhat = new NhanVien
                {
                    MaNV = _maNV,
                    HoTen = txtTenNhanVien.Text.Trim(),
                    TaiKhoan = txtTaiKhoan.Text.Trim(),
                    MatKhau = txtMatKhau.Text.Trim(),
                    VaiTro = cboVaiTro.Text,
                    DienThoai = txtDienThoai.Text.Trim(),
                };

                bool success = _nhanVienService.SuaNhanVien(nhanVienCapNhat);

                if (success)
                {
                    MessageBox.Show("Cập nhật nhân viên thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại!", "Lỗi");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {

        }

        private void chkHienMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            txtMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked;
        }
    }
}
