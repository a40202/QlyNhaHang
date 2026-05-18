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
        private void nudGiamGiaPhanTram_ValueChanged(object sender, EventArgs e)
        {
            TinhThanhToan();
        }
        private void TinhThanhToan()
        {
            try
            {
                decimal tongTien = decimal.TryParse(txtTongTien.Text, out decimal tt) ? tt : 0;
                decimal phanTramGiam = nudGiamGiaPhanTram.Value;

                decimal tienGiam = tongTien * (phanTramGiam / 100);
                decimal thanhToan = tongTien - tienGiam;

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
            txtMaNhanVien.Text = CurrentUser.MaNV.ToString();
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
                MessageBox.Show("Vui lòng chọn bàn ăn!", "Thông báo");
                return;
            }

            if (!decimal.TryParse(txtTongTien.Text, out decimal tongTien) || tongTien <= 0)
            {
                MessageBox.Show("Vui lòng nhập Tổng tiền hợp lệ!", "Thông báo");
                return;
            }

            try
            {
                decimal phanTramGiam = nudGiamGiaPhanTram.Value;
                decimal tienGiam = tongTien * (phanTramGiam / 100);
                decimal thanhToan = tongTien - tienGiam;

                var hoaDonMoi = new HoaDon
                {
                    MaBan = Convert.ToInt32(cboBanAn.SelectedValue),
                    MaNV = CurrentUser.MaNV,
                    NgayLap = dtpNgayLap.Value,
                    TongTien = tongTien,
                    GiamGia = tienGiam,           
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
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
   
        private void btnHuy_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
