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
    public partial class frmQuanLyHoaDon : Form
    {
        private readonly HoaDonService _hoaDonService = new HoaDonService();
        public frmQuanLyHoaDon()
        {
            InitializeComponent();
        }
        private void frmQuanLyHoaDon_Load(object sender, EventArgs e)
        {   
            this.Text = "Quản Lý Hóa Đơn";
            LoadAllHoaDon();
        }
        // ====================== HIỂN THỊ DANH SÁCH HÓA ĐƠN ======================
        private void LoadAllHoaDon()
        {
            try
            {
                List<HoaDon> danhSach = _hoaDonService.GetAllHoaDon();

                DataTable dt = new DataTable();
                dt.Columns.Add("Mã HD", typeof(int));
                dt.Columns.Add("Bàn", typeof(string));
                dt.Columns.Add("Nhân viên", typeof(string));
                dt.Columns.Add("Ngày lập", typeof(string));
                dt.Columns.Add("Tổng tiền", typeof(decimal));
                dt.Columns.Add("Giảm giá", typeof(decimal));
                dt.Columns.Add("Thanh toán", typeof(decimal));
                dt.Columns.Add("Trạng thái", typeof(string));   // ← Phải là string

                foreach (var hd in danhSach)
                {
                    dt.Rows.Add(
                        hd.MaHD,
                        $"Bàn {hd.MaBan}",
                        hd.TenNhanVien ?? "Không rõ",
                        hd.NgayLap.ToString("dd/MM/yyyy HH:mm"),
                        hd.TongTien,
                        hd.GiamGia,
                        hd.ThanhToan,
                        hd.TrangThai                      // ← Trạng thái là string
                    );
                }

                dgvHoaDon.DataSource = dt;

                // Format tiền tệ
                if (dgvHoaDon.Columns["Tổng tiền"] != null)
                    dgvHoaDon.Columns["Tổng tiền"].DefaultCellStyle.Format = "N0";

                if (dgvHoaDon.Columns["Giảm giá"] != null)
                    dgvHoaDon.Columns["Giảm giá"].DefaultCellStyle.Format = "N0";

                if (dgvHoaDon.Columns["Thanh toán"] != null)
                    dgvHoaDon.Columns["Thanh toán"].DefaultCellStyle.Format = "N0";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách hóa đơn:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void banAnStripMenu_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmBanAn fBanAn = new frmBanAn();
            fBanAn.ShowDialog();
            this.Close();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvHoaDon.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần hủy!", "Thông báo");
                return;
            }

            int maHD = Convert.ToInt32(dgvHoaDon.CurrentRow.Cells["Mã HD"].Value);
            string trangThai = dgvHoaDon.CurrentRow.Cells["Trạng thái"].Value?.ToString();

            if (trangThai == "DaThanhToan")
            {
                MessageBox.Show("Không thể hủy hóa đơn đã thanh toán!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Bạn có chắc muốn HỦY hóa đơn #{maHD}?\n\nHành động này không thể hoàn tác!",
                "Xác nhận hủy hóa đơn", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                == DialogResult.Yes)
            {
                bool success = _hoaDonService.HuyHoaDon(maHD);

                if (success)
                {
                    MessageBox.Show("Hủy hóa đơn thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAllHoaDon(); // Refresh danh sách
                }
                else
                {
                    MessageBox.Show("Hủy hóa đơn thất bại!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadAllHoaDon();
        }
        private void dgvHoaDon_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int maHD = Convert.ToInt32(dgvHoaDon.Rows[e.RowIndex].Cells["Mã HD"].Value);
            MessageBox.Show($"Xem chi tiết hóa đơn #{maHD}", "Chi tiết");
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvHoaDon.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần sửa!", "Thông báo");
                return;
            }

            int maHD = Convert.ToInt32(dgvHoaDon.CurrentRow.Cells["Mã HD"].Value);
            frmSuaHoaDon fSuaHoaDon = new frmSuaHoaDon();
            fSuaHoaDon.ShowDialog();
            LoadAllHoaDon(); 
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            frmThemHoaDon fThemHoaDon = new frmThemHoaDon();
            fThemHoaDon.ShowDialog();
            LoadAllHoaDon(); 
        }
    }
}
