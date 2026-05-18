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
        private readonly OrderTamService _orderTamService = new OrderTamService();
        private readonly MonAnService _monAnService = new MonAnService();
        private readonly BanAnService _banAnService = new BanAnService();
        private readonly BLL.HoaDonService _hoaDonService = new BLL.HoaDonService();


        private List<BanAn> danhSachBan;
        private int _maBanHienTai = 0;  

        public frmBanAn()
        {
            InitializeComponent();
        }

        private void frmBanAn_Load(object sender, EventArgs e)
        {
            this.Text = "Sơ Đồ Quản Lý Bàn Ăn";
            SetupForm();
            SetupDataGridView();
            LoadDanhSachBan();
        }   
        private void SetupForm()
        {
            flpBanAn.AutoScroll = true;
            flpBanAn.FlowDirection = FlowDirection.LeftToRight;
            flpBanAn.WrapContents = true;
            flpBanAn.Padding = new Padding(10);
        }

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
        private void SetupDataGridView()
        {
            dgvHoaDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHoaDon.AllowUserToAddRows = false;
            dgvHoaDon.ReadOnly = true;
            dgvHoaDon.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHoaDon.RowHeadersVisible = false;
        }
        private Button TaoButtonBan(BanAn ban)
        {
            Button btn = new Button
            {
                Text = $"{ban.TenBan}\n\n{ban.TrangThai}",
                Width = 85,
                Height = 85,
                Margin = new Padding(8),            
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 }
            };

            // set màu theo trạng thái
            string trangThai = ban.TrangThai.ToLower().Trim();

            if (trangThai.Contains("trống") || trangThai == "trong")
            {
                btn.BackColor = Color.FromArgb(79, 126, 247);   // Xanh 
                btn.ForeColor = Color.White;
            }
            else if (trangThai.Contains("đang") || trangThai.Contains("dangsudung"))
            {
                btn.BackColor = Color.FromArgb(255, 51, 51);     // Đỏ
                btn.ForeColor = Color.White;
            }          
            else
            {
                // Mặc định 
                btn.BackColor = Color.FromArgb(128, 128, 128);   // Xám
                btn.ForeColor = Color.White;
            }

            btn.Click += (s, ev) => Ban_Click(ban);
            return btn;
        }

        // ================== CLICK VÀO BÀN =================
        private void Ban_Click(BanAn ban)
        {
            _maBanHienTai = ban.MaBan;
            lblBanHienTai.Text = $"Bàn: {ban.TenBan}";
            lblTrangThai.Text = $"Trạng thái: {ban.TrangThai}";

            dgvHoaDon.DataSource = null;
            lblTongTien.Text = "0 VNĐ";
            btnThanhToan.Enabled = false;
            btnChuyenBan.Enabled = false;

            if (ban.TrangThai.ToLower().Contains("trống"))
            {
                // Bàn trống - Mở form gọi món
                frmOrder f = new frmOrder(ban.MaBan);
                f.ShowDialog();
                LoadDanhSachBan();
            }
            else
            {
                // Bàn đang có khách - hiển thị danh sách món đã order
                LoadChiTietHoaDon(ban.MaBan);
                btnChuyenBan.Enabled = true;
                btnThanhToan.Enabled = true;
            }
        }

        // ====================== TẢI CHI TIẾT HÓA ĐƠN ======================
        private void LoadChiTietHoaDon(int maBan)
        {
            try
            {
                List<OrderTam> danhSach = _orderTamService.GetByBan(maBan);

                if (danhSach.Count == 0)
                {
                    MessageBox.Show($"Bàn {maBan} hiện chưa có món ăn nào.", "Thông báo");
                    dgvHoaDon.DataSource = null;
                    lblTongTien.Text = "0 VNĐ";
                    return;
                }

                DataTable dt = new DataTable();
                dt.Columns.Add("Tên món");
                dt.Columns.Add("Số lượng", typeof(int));
                dt.Columns.Add("Đơn giá", typeof(decimal));
                dt.Columns.Add("Thành tiền", typeof(decimal));

                decimal tongTien = 0;

                foreach (var item in danhSach)
                {
                    dt.Rows.Add(item.TenMon, item.SoLuong, item.DonGia, item.ThanhTien);
                    tongTien += item.ThanhTien;
                }

                dgvHoaDon.DataSource = dt;
                dgvHoaDon.Columns["Đơn giá"].DefaultCellStyle.Format = "N0";
                dgvHoaDon.Columns["Thành tiền"].DefaultCellStyle.Format = "N0";

                lblTongTien.Text = tongTien.ToString("N0") + " VNĐ";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải order: " + ex.Message);
            }
        }

        // ================ NÚT CHỨC NĂNG ===================
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadDanhSachBan();
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
        private void changePasswordSMI_Click(object sender, EventArgs e)
        {
            frmDoiMatKhau fDoiMatKhau = new frmDoiMatKhau();
            fDoiMatKhau.ShowDialog();       
        }

        private void btnThanhToan_Click_1(object sender, EventArgs e)
        {   

            if (_maBanHienTai == 0)
            {
                MessageBox.Show("Vui lòng chọn bàn cần thanh toán!", "Thông báo");
                return;
            }

            decimal giamGia = nudGiamGia.Value;

            var result = MessageBox.Show(
                $"Xác nhận thanh toán cho bàn {_maBanHienTai}?\n" +
                $"Giảm giá: {giamGia:N0} %",
                "Xác nhận thanh toán",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                bool success = _hoaDonService.ThanhToanHoaDon(_maBanHienTai, giamGia);

                if (success)
                {
                    MessageBox.Show("Thanh toán thành công!\n" +
                                  "Hóa đơn đã được lưu và bàn đã trở về trạng thái Trống.",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvHoaDon.DataSource = null;
                    lblTongTien.Text = "0 VNĐ";
                    nudGiamGia.Value = 0;
                    LoadDanhSachBan();
                }
                else
                {
                    MessageBox.Show("Thanh toán thất bại! Dữ liệu đã được khôi phục.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_maBanHienTai == 0)
            {
                MessageBox.Show("Vui lòng chọn một bàn trước khi thêm món!", "Thông báo");
                return;
            }
            frmOrder f = new frmOrder(_maBanHienTai);
            f.ShowDialog();
            LoadDanhSachBan();           
            LoadChiTietHoaDon(_maBanHienTai); 
        }

        private void dgvHoaDon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnLamMoi_Click_1(object sender, EventArgs e)
        {
            LoadDanhSachBan();
        }

        private void btnChuyenBan_Click(object sender, EventArgs e)
        {
            if (_maBanHienTai == 0)
            {
                MessageBox.Show("Vui lòng chọn bàn hiện tại!", "Thông báo");
                return;
            }

            frmChuyenBan f = new frmChuyenBan(_maBanHienTai);
            if (f.ShowDialog() == DialogResult.OK)
            {
                LoadDanhSachBan();        
                dgvHoaDon.DataSource = null;
                lblTongTien.Text = "0 VNĐ";
                _maBanHienTai = 0;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void nudGiamGia_ValueChanged(object sender, EventArgs e)
        {

        }

        private void flpBanAn_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void mainSMI_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmMain fMain = new frmMain();
            fMain.ShowDialog();
            this.Close();
        }
    }
}