using QlyNhaHang.BLL;
using QlyNhaHang.Helpers;
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
    public partial class frmThemHoaDon : Form
    {
        private readonly HoaDonService _hoaDonService = new HoaDonService();
        private readonly BanAnService _banAnService = new BanAnService();
        public frmThemHoaDon()
        {
            InitializeComponent();
        }

        private void frmThemHoaDon_Load(object sender, EventArgs e)
        {
            this.Text = "Thêm Hóa Đơn Mới";
            LoadThongTinNhanVien();
            LoadComboBoxBanAn();
            dtpNgayLap.Value = DateTime.Now;
            txtNhanVien.Text = CurrentUser.HoTen;

        }
        private void TxtNumberOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Chỉ cho phép nhập số và phím Backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void txtTongTien_TextChanged(object sender, EventArgs e)
        {
            TinhThanhToan();
        }
        private void txtGiamGia_TextChanged(object sender, EventArgs e)
        {
            TinhThanhToan();
        }
        private void TinhThanhToan()
        {
            try
            {
                decimal tongTien = decimal.TryParse(txtTongTien.Text, out decimal tt) ? tt : 0;
                decimal giamGia = decimal.TryParse(txtGiamGia.Text, out decimal gg) ? gg : 0;

                decimal thanhToan = tongTien - giamGia;
                if (thanhToan < 0) thanhToan = 0;

                txtThanhToan.Text = thanhToan.ToString("N0");
            }
            catch
            {
                txtThanhToan.Text = "0";
            }
        }
        private void LoadThongTinNhanVien()
        {
            txtNhanVien.Text = CurrentUser.HoTen + " (" + CurrentUser.VaiTro + ")";
        }
        private void label5_Click(object sender, EventArgs e)
        {

        }
        private void LoadComboBoxBanAn()
        {
            var dsBan = _banAnService.GetAllBanAn();
            cboBanAn.DataSource = dsBan;
            cboBanAn.DisplayMember = "TenBan";
            cboBanAn.ValueMember = "MaBan";
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (cboBanAn.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn bàn ăn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txtTongTien.Text) || decimal.Parse(txtTongTien.Text) <= 0)
            {
                MessageBox.Show("Vui lòng nhập Tổng tiền hợp lệ!", "Thông báo");
                return;
            }

            try
            {
                decimal tongTien = decimal.Parse(txtTongTien.Text);
                decimal giamGia = string.IsNullOrEmpty(txtGiamGia.Text) ? 0 : decimal.Parse(txtGiamGia.Text);
                decimal thanhToan = tongTien - giamGia;

                var hoaDonMoi = new HoaDon
                {
                    MaBan = Convert.ToInt32(cboBanAn.SelectedValue),
                    MaNV = CurrentUser.MaNV,
                    NgayLap = dtpNgayLap.Value,
                    TongTien = tongTien,
                    GiamGia = giamGia,
                    ThanhToan = thanhToan,
                    TrangThai = cboTrangThai.SelectedItem?.ToString() ?? "ChuaThanhToan"
                };

                bool success = _hoaDonService.ThemHoaDon(hoaDonMoi);

                if (success)
                {
                    MessageBox.Show("Thêm hóa đơn thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Thêm hóa đơn thất bại!", "Lỗi");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm hóa đơn:\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);    
            }
        }

        private void nudGiamGia_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
