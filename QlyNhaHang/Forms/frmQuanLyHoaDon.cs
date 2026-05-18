using ClosedXML.Excel;
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
            SetupDataGridView();
            SetupSearchControls();
            LoadAllHoaDon();
        }
        private void SetupDataGridView()
        {
            dgvHoaDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHoaDon.AllowUserToAddRows = false;
            dgvHoaDon.ReadOnly = true;
            dgvHoaDon.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHoaDon.RowHeadersVisible = false;
        }   
        private void LoadAllHoaDon()
        {
            var danhSach = _hoaDonService.GetAllHoaDon();
            LoadDataToGrid(danhSach);
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
                    LoadAllHoaDon(); 
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
            txtTimKiem.Clear();
            cboTrangThai.SelectedIndex = 0;
            dtpTuNgay.Value = DateTime.Now.AddDays(-7);
            dtpDenNgay.Value = DateTime.Now;
            LoadAllHoaDon();
        }
        private void dgvHoaDon_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int maHD = Convert.ToInt32(dgvHoaDon.Rows[e.RowIndex].Cells["Mã HD"].Value);      
            frmSuaHoaDon f = new frmSuaHoaDon(maHD);
            if (f.ShowDialog() == DialogResult.OK)
            {
                LoadAllHoaDon();
            }
        }
        private void SetupSearchControls()
        {
            // Thiết lập ComboBox Trạng thái
            cboTrangThai.Items.Add("Tất cả");
            cboTrangThai.Items.Add("ChuaThanhToan");
            cboTrangThai.Items.Add("DaThanhToan");
            cboTrangThai.SelectedIndex = 0;

            dtpTuNgay.Value = DateTime.Now.AddDays(-7);
            dtpDenNgay.Value = DateTime.Now;
        }
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvHoaDon.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần sửa!", "Thông báo");
                return;
            }

            int maHD = Convert.ToInt32(dgvHoaDon.CurrentRow.Cells["Mã HD"].Value);
            frmSuaHoaDon fSuaHoaDon = new frmSuaHoaDon(maHD);
            fSuaHoaDon.ShowDialog();
            LoadAllHoaDon(); 
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            frmThemHoaDon fThemHoaDon = new frmThemHoaDon();
            fThemHoaDon.ShowDialog();
            LoadAllHoaDon(); 
        }

        private void dgvHoaDon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtTimKiem.Text.Trim();
                DateTime tuNgay = dtpTuNgay.Value.Date;
                DateTime denNgay = dtpDenNgay.Value.Date.AddDays(1).AddSeconds(-1);
                string trangThai = cboTrangThai.SelectedItem?.ToString() ?? "Tất cả";

                var danhSach = _hoaDonService.TimKiemHoaDon(keyword, tuNgay, denNgay, trangThai);

                LoadDataToGrid(danhSach);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void xuatExcel()
        {
            if (dgvHoaDon.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo");
                return;
            }

            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Excel Files|*.xlsx";
                    sfd.Title = "Xuất danh sách hóa đơn";
                    sfd.FileName = $"HoaDon_{DateTime.Now:yyyyMMdd_HHmmss}";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        using (var workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("Danh sách Hóa đơn");
                            
                            worksheet.Cell(1, 1).Value = "DANH SÁCH HÓA ĐƠN";
                            worksheet.Range("A1:H1").Merge();
                            worksheet.Cell(1, 1).Style.Font.Bold = true;
                            worksheet.Cell(1, 1).Style.Font.FontSize = 16;
                            worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                            for (int i = 0; i < dgvHoaDon.Columns.Count; i++)
                            {
                                worksheet.Cell(3, i + 1).Value = dgvHoaDon.Columns[i].HeaderText;
                                worksheet.Cell(3, i + 1).Style.Font.Bold = true;
                            }

                            for (int i = 0; i < dgvHoaDon.Rows.Count; i++)
                            {
                                for (int j = 0; j < dgvHoaDon.Columns.Count; j++)
                                {
                                    worksheet.Cell(i + 4, j + 1).Value = dgvHoaDon.Rows[i].Cells[j].Value?.ToString();
                                }
                            }
                            worksheet.Columns().AdjustToContents();

                            workbook.SaveAs(sfd.FileName);
                        }

                        MessageBox.Show("Xuất file Excel thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất Excel:\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadDataToGrid(List<HoaDon> danhSach)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Mã HD", typeof(int));
            dt.Columns.Add("Bàn", typeof(string));
            dt.Columns.Add("Nhân viên", typeof(string));
            dt.Columns.Add("Ngày lập", typeof(string));
            dt.Columns.Add("Tổng tiền", typeof(decimal));
            dt.Columns.Add("Giảm giá", typeof(decimal));
            dt.Columns.Add("Thanh toán", typeof(decimal));
            dt.Columns.Add("Trạng thái", typeof(string));

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
                    hd.TrangThai
                );
            }

            dgvHoaDon.DataSource = dt;

            if (dgvHoaDon.Columns["Tổng tiền"] != null) dgvHoaDon.Columns["Tổng tiền"].DefaultCellStyle.Format = "N0";
            if (dgvHoaDon.Columns["Giảm giá"] != null) dgvHoaDon.Columns["Giảm giá"].DefaultCellStyle.Format = "N0";
            if (dgvHoaDon.Columns["Thanh toán"] != null) dgvHoaDon.Columns["Thanh toán"].DefaultCellStyle.Format = "N0";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void logOutStripMenuItem_Click(object sender, EventArgs e)
        {   
            this.Hide();
            frmLogin fLogin = new frmLogin();
            fLogin.ShowDialog();
            this.Close();
        }

        private void exitStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void changePasswordSMI_Click(object sender, EventArgs e)
        {
            frmDoiMatKhau fDoiMatKhau = new frmDoiMatKhau();
            fDoiMatKhau.ShowDialog();
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (dgvHoaDon.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần thanh toán!", "Thông báo");
                return;
            }

            string trangThai = dgvHoaDon.CurrentRow.Cells["Trạng thái"].Value.ToString();

            if (trangThai == "DaThanhToan")
            {
                MessageBox.Show("Hóa đơn này đã được thanh toán!", "Thông báo");
                return;
            }

            int maHD = Convert.ToInt32(dgvHoaDon.CurrentRow.Cells["Mã HD"].Value);

            if (MessageBox.Show($"Xác nhận thanh toán hóa đơn #{maHD}?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.Yes)
            {
                bool success = _hoaDonService.ThanhToanHoaDonByMaHD(maHD);

                if (success)
                {
                    MessageBox.Show("Thanh toán hóa đơn thành công!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAllHoaDon(); 
                }
                else
                {
                    MessageBox.Show("Thanh toán thất bại!", "Lỗi");
                }
            }
        }

        private void btnXemChiTiet_Click(object sender, EventArgs e)
        {
            if (dgvHoaDon.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn để xem chi tiết!", "Thông báo");
                return;
            }

            int maHD = Convert.ToInt32(dgvHoaDon.CurrentRow.Cells["Mã HD"].Value);

            frmChiTietHoaDon f = new frmChiTietHoaDon(maHD);
            f.ShowDialog();
        }

        private void excelStripMenuItem_Click(object sender, EventArgs e)
        {
            xuatExcel();
        }

        private void btnXuatFileExcel_Click(object sender, EventArgs e)
        {
            xuatExcel();
        }
    }
}
