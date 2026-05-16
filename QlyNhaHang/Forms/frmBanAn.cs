using QlyNhaHang.BLL;
using QlyNhaHang.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace QlyNhaHang.Forms
{
    public partial class frmBanAn : Form
    {
        private readonly BanAnService _banAnService = new BanAnService();
        private readonly BLL.HoaDonService _hoaDonService = new BLL.HoaDonService();

        private List<BanAn> danhSachBan;
        private int _maBanHienTai = 0;   // Quan trọng: lưu bàn đang chọn

        public frmBanAn()
        {
            InitializeComponent();
        }

        private void frmBanAn_Load(object sender, EventArgs e)
        {
            this.Text = "Sơ Đồ Quản Lý Bàn Ăn";
            SetupForm();
            LoadDanhSachBan();
        }

        private void SetupForm()
        {
            flpBanAn.AutoScroll = true;
            flpBanAn.FlowDirection = FlowDirection.LeftToRight;
            flpBanAn.WrapContents = true;
            flpBanAn.Padding = new Padding(10);
        }

        // ====================== TẢI DANH SÁCH BÀN ======================
        private void LoadDanhSachBan()
        {
            flpBanAn.Controls.Clear();
            danhSachBan = _banAnService.GetAllBanAn();

            foreach (var ban in danhSachBan)
            {
                Button btnBan = TaoButtonBan(ban);
                flpBanAn.Controls.Add(btnBan);
            }
        }

        private Button TaoButtonBan(BanAn ban)
        {
            Button btn = new Button
            {
                Text = $"{ban.TenBan}\n\n{ban.TrangThai}",
                Width = 140,
                Height = 115,
                Margin = new Padding(8),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 }
            };

            // Màu sắc theo trạng thái
            switch (ban.TrangThai.ToLower())
            {
                case "trống":
                    btn.BackColor = Color.FromArgb(135, 206, 235);   // Xanh dương nhạt
                    btn.ForeColor = Color.Black;
                    break;

                case "dangsudung":
                case "đang sử dụng":
                    btn.BackColor = Color.FromArgb(255, 51, 51);     // Đỏ
                    btn.ForeColor = Color.White;
                    break;

                case "đặt trước":
                case "dattruoc":
                    btn.BackColor = Color.FromArgb(255, 215, 0);     // Vàng
                    btn.ForeColor = Color.Black;
                    break;

                default:
                    btn.BackColor = Color.Gray;
                    btn.ForeColor = Color.White;
                    break;
            }

            btn.Click += (s, ev) => Ban_Click(ban);
            return btn;
        }

        // ====================== CLICK VÀO BÀN ======================
        private void Ban_Click(BanAn ban)
        {
            _maBanHienTai = ban.MaBan;
            lblBanHienTai.Text = $"Bàn: {ban.TenBan}";
            lblTrangThai.Text = $"Trạng thái: {ban.TrangThai}";

            btnThanhToan.Enabled = false;

            if (ban.TrangThai.ToLower() == "trống")
            {
                dgvHoaDon.DataSource = null;
                lblTongTien.Text = "0 VNĐ";
                // TODO: Sau này mở form gọi món
                // new frmOrder(ban.MaBan).ShowDialog();
            }
            else if (ban.TrangThai.ToLower() == "dangsudung" ||
                     ban.TrangThai.ToLower().Contains("đang"))
            {
                LoadChiTietHoaDon(ban.MaBan);
                btnThanhToan.Enabled = true;
            }
            else
            {
                dgvHoaDon.DataSource = null;
                lblTongTien.Text = "0 VNĐ";
            }
        }

        // ====================== TẢI CHI TIẾT HÓA ĐƠN ======================
        private void LoadChiTietHoaDon(int maBan)
        {
            try
            {
                List<ChiTietHoaDon> danhSach = _hoaDonService.GetChiTietHoaDonByBan(maBan);

                DataTable dt = new DataTable();
                dt.Columns.Add("Tên món");
                dt.Columns.Add("Số lượng", typeof(int));
                dt.Columns.Add("Đơn giá", typeof(decimal));
                dt.Columns.Add("Thành tiền", typeof(decimal));

                decimal tongTien = 0;

                foreach (var ct in danhSach)
                {
                    dt.Rows.Add(ct.TenMon, ct.SoLuong, ct.DonGia, ct.ThanhTien);
                    tongTien += ct.ThanhTien;
                }

                dgvHoaDon.DataSource = dt;

                // Format tiền tệ
                if (dgvHoaDon.Columns["Đơn giá"] != null)
                    dgvHoaDon.Columns["Đơn giá"].DefaultCellStyle.Format = "N0";

                if (dgvHoaDon.Columns["Thành tiền"] != null)
                    dgvHoaDon.Columns["Thành tiền"].DefaultCellStyle.Format = "N0";

                dgvHoaDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                lblTongTien.Text = tongTien.ToString("N0") + " VNĐ";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải hóa đơn: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ====================== NÚT CHỨC NĂNG ======================
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadDanhSachBan();
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (_maBanHienTai == 0)
            {
                MessageBox.Show("Vui lòng chọn bàn cần thanh toán!", "Thông báo");
                return;
            }

            if (MessageBox.Show($"Xác nhận thanh toán cho bàn {_maBanHienTai}?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                bool result = _hoaDonService.ThanhToanHoaDon(_maBanHienTai, nudGiamGia?.Value ?? 0);

                if (result)
                {
                    MessageBox.Show("Thanh toán thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSachBan(); // Refresh lại sơ đồ bàn
                }
                else
                {
                    MessageBox.Show("Thanh toán thất bại!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void hoaDonStripMenu_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmQuanLyHoaDon fQuanLyHoaDon = new frmQuanLyHoaDon();
            fQuanLyHoaDon.ShowDialog();
            this.Close();
        }

        private void exitStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void logOutStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmLogin fLogin = new frmLogin();
            fLogin.ShowDialog();
            this.Close();
        }
    }
}