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
    public partial class frmSuaHoaDon : Form
    {
        private readonly HoaDonService _hoaDonService = new HoaDonService();
        private readonly BanAnService _banAnService = new BanAnService();
        private readonly int _maHD;
        private HoaDon _hoaDonHienTai;
        public frmSuaHoaDon(int maHD)
        {
            InitializeComponent();
            _maHD = maHD;
        }

        private void frmSuaHoaDon_Load(object sender, EventArgs e)
        {
            this.Text = $"Sửa Hóa Đơn #{_maHD}";          
            LoadThongTinHoaDon();
            LoadComboBoxBanAn();
        }
        private void LoadComboBoxBanAn()
        {
            var dsBan = _banAnService.GetAllBanAn();
            cboBanAn.DataSource = dsBan;
            cboBanAn.DisplayMember = "TenBan";
            cboBanAn.ValueMember = "MaBan";
        }
        private void LoadThongTinHoaDon()
        {
            try
            {
                _hoaDonHienTai = _hoaDonService.GetHoaDonById(_maHD);

                if (_hoaDonHienTai == null)
                {
                    MessageBox.Show("Không tìm thấy hóa đơn!", "Lỗi");
                    this.Close();
                    return;
                }

                lblMaHD.Text = $"Mã HD: {_maHD}";
                cboBanAn.Text = $"{_hoaDonHienTai.MaBan}";
                txtMaNhanVien.Text = _hoaDonHienTai.MaNV.ToString();
                txtNhanVien.Text = $"{_hoaDonHienTai.TenNhanVien ?? "Không rõ"}";
                dtpNgayLap.Value = _hoaDonHienTai.NgayLap;

                txtTongTien.Text = _hoaDonHienTai.TongTien.ToString("N0");
                nudGiamGiaPhanTram.Value = 0;

                
                if (_hoaDonHienTai.TongTien > 0)
                {
                    decimal phanTram = (_hoaDonHienTai.GiamGia / _hoaDonHienTai.TongTien) * 100;
                    nudGiamGiaPhanTram.Value = Math.Min(100, (decimal)phanTram);
                }

                cboTrangThai.Text = _hoaDonHienTai.TrangThai;

                TinhThanhToan();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thông tin hóa đơn: " + ex.Message);
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

                txtThanhToan.Text = Math.Max(thanhToan, 0).ToString("N0");
            }
            catch
            {
                txtThanhToan.Text = "0";
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (cboBanAn.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn bàn!", "Thông báo");
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

                bool success = _hoaDonService.CapNhatHoaDon(
                    _maHD,
                    Convert.ToInt32(cboBanAn.SelectedValue),
                    tongTien,
                    tienGiam,
                    thanhToan,
                    cboTrangThai.Text);

                if (success)
                {
                    MessageBox.Show("Cập nhật hóa đơn thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Cập nhật hóa đơn thất bại!", "Lỗi");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
