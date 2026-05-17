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
    public partial class frmChiTietHoaDon : Form
    {
        private readonly HoaDonService _hoaDonService = new HoaDonService();
        private readonly int _maHD;
        private HoaDon _hoaDon;
        public frmChiTietHoaDon(int maHD)
        {
            InitializeComponent();
            _maHD = maHD;
        }

        private void frmChiTietHoaDon_Load(object sender, EventArgs e)
        {
            this.Text = $"Chi Tiết Hóa Đơn #{_maHD}";
            SetupForm();
            LoadChiTietHoaDon();
        }
        private void SetupForm()
        {
            dgvChiTiet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvChiTiet.AllowUserToAddRows = false;
            dgvChiTiet.ReadOnly = true;
            dgvChiTiet.RowHeadersVisible = false;
        }
        private void LoadChiTietHoaDon()
        {
            try
            {
                _hoaDon = _hoaDonService.GetHoaDonById(_maHD);

                if (_hoaDon == null)
                {
                    MessageBox.Show("Không tìm thấy hóa đơn!", "Lỗi");
                    this.Close();
                    return;
                }

                // Thông tin hóa đơn
                lblMaHD.Text = $"Mã Hóa Đơn: {_maHD}";
                lblBan.Text = $"Bàn: {_hoaDon.MaBan}";
                lblNhanVien.Text = $"Nhân viên: {_hoaDon.TenNhanVien ?? "Không rõ"}";
                lblNgayLap.Text = $"Ngày lập: {_hoaDon.NgayLap:dd/MM/yyyy HH:mm}";
                lblTrangThai.Text = $"Trạng thái: {_hoaDon.TrangThai}";

                // Tải chi tiết món ăn
                var danhSachChiTiet = _hoaDonService.GetChiTietByMaHD(_maHD);

                DataTable dt = new DataTable();
                dt.Columns.Add("Tên món");
                dt.Columns.Add("Số lượng", typeof(int));
                dt.Columns.Add("Đơn giá", typeof(decimal));
                dt.Columns.Add("Thành tiền", typeof(decimal));

                decimal tongTien = 0;

                foreach (var ct in danhSachChiTiet)
                {
                    dt.Rows.Add(ct.TenMon, ct.SoLuong, ct.DonGia, ct.ThanhTien);
                    tongTien += ct.ThanhTien;
                }

                dgvChiTiet.DataSource = dt;

                // Format cột
                dgvChiTiet.Columns["Đơn giá"].DefaultCellStyle.Format = "N0";
                dgvChiTiet.Columns["Thành tiền"].DefaultCellStyle.Format = "N0";

                // Hiển thị tổng tiền
                lblTongTien.Text = $"Tổng tiền: {tongTien:N0} VNĐ";
                lblGiamGia.Text = $"Giảm giá: {_hoaDon.GiamGia:N0} %";
                lblThanhToan.Text = $"Tổng tiền thanh toán: {_hoaDon.ThanhToan:N0} VNĐ";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết hóa đơn: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
