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
    public partial class frmQuanLyMonAn : Form
    {
        private readonly MonAnService _monAnService = new MonAnService();
        private int? maMonDangChon = null;
        public frmQuanLyMonAn()
        {
            InitializeComponent();
        }

        private void frmQuanLyMonAn_Load(object sender, EventArgs e)
        {
            this.Text = "Quản Lý Món Ăn";
            LoadDanhSachMonAn();
            SetupDataGridView();
        }
        private void SetupDataGridView()
        {
            dgvMonAn.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMonAn.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMonAn.ReadOnly = true;
            dgvMonAn.MultiSelect = false;
        }      
        private void LoadDanhSachMonAn(string tuKhoa = "")
        {
            try
            {
                var danhSach = _monAnService.GetAllBy(tuKhoa);
                dgvMonAn.DataSource = danhSach;

                if (dgvMonAn.Columns["Gia"] != null)
                    dgvMonAn.Columns["Gia"].DefaultCellStyle.Format = "N0";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách: " + ex.Message);
            }
        }
        private void dgvMonAn_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
        
            var cellMaMon = dgvMonAn.Rows[e.RowIndex].Cells["MaMon"];

            if (cellMaMon?.Value != null)
            {
                maMonDangChon = Convert.ToInt32(cellMaMon.Value);
            }


        }
        private void dgvMonAn_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            frmThemMonAn fThemMonAn = new frmThemMonAn();
            fThemMonAn.ShowDialog();
            LoadDanhSachMonAn();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (maMonDangChon == null)
            {
                MessageBox.Show("Vui lòng click chọn một món ăn trong bảng trước khi sửa!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (frmSuaMonAn frm = new frmSuaMonAn(maMonDangChon.Value))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    LoadDanhSachMonAn(txtTimKiem.Text.Trim());
                    maMonDangChon = null; 
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (maMonDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn món ăn cần xóa trong bảng!", "Thông báo");
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa món ăn này?", "Xác nhận xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    bool success = _monAnService.XoaMonAn(maMonDangChon.Value);
                    if (success)
                    {
                        MessageBox.Show("Xóa món ăn thành công!", "Thành công");
                        LoadDanhSachMonAn(txtTimKiem.Text.Trim());
                        maMonDangChon = null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa: " + ex.Message);
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            maMonDangChon = null;
            txtTimKiem.Clear();           // Xóa từ khóa tìm kiếm
            LoadDanhSachMonAn();
        }
        

        private void btnTimKiem_Click(object sender, EventArgs e)
        {

        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            LoadDanhSachMonAn(txtTimKiem.Text.Trim());
        }

        private void btnXuatFileExcel_Click(object sender, EventArgs e)
        {
            if (dgvMonAn.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo");
                return;
            }

            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Danh Sách Món Ăn");

                    // Tiêu đề báo cáo
                    worksheet.Cell(1, 1).Value = "DANH SÁCH MÓN ĂN";
                    worksheet.Range("A1:E1").Merge();
                    worksheet.Cell(1, 1).Style.Font.Bold = true;
                    worksheet.Cell(1, 1).Style.Font.FontSize = 16;
                    worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    // Chuyển List sang DataTable để xuất
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Mã Món", typeof(int));
                    dt.Columns.Add("Tên Món", typeof(string));
                    dt.Columns.Add("Giá", typeof(decimal));
                    dt.Columns.Add("Mô Tả", typeof(string));
                    dt.Columns.Add("Danh Mục", typeof(string));
                    dt.Columns.Add("Trạng Thái", typeof(string));

                    foreach (DataGridViewRow row in dgvMonAn.Rows)
                    {
                        if (row.IsNewRow) continue;

                        dt.Rows.Add(
                            row.Cells["MaMon"].Value,
                            row.Cells["TenMon"].Value,
                            row.Cells["Gia"].Value,
                            row.Cells["MoTa"].Value ?? "",
                            row.Cells["TenDanhMuc"]?.Value ?? "",
                            row.Cells["TrangThai"].Value != null && Convert.ToBoolean(row.Cells["TrangThai"].Value) ? "Đang bán" : "Ngừng bán"
                        );
                    }

                    // Insert DataTable vào Excel
                    var table = worksheet.Cell(3, 1).InsertTable(dt, "MonAnTable", true);

                    // Định dạng bảng
                    table.Theme = XLTableTheme.None;
                    worksheet.Columns().AdjustToContents();

                    // Lưu file
                    SaveFileDialog saveDialog = new SaveFileDialog
                    {
                        Filter = "Excel Files|*.xlsx",
                        Title = "Xuất danh sách món ăn",
                        FileName = $"DanhSachMonAn_{DateTime.Now:yyyyMMdd_HHmm}.xlsx"
                    };

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        workbook.SaveAs(saveDialog.FileName);
                        MessageBox.Show("Xuất file Excel thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất Excel: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
