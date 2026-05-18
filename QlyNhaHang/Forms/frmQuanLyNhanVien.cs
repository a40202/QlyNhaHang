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
    public partial class frmQuanLyNhanVien : Form
    {
        private readonly NhanVienService _nhanVienService = new NhanVienService();
        private int? maNhanVienDangChon = null;
        public frmQuanLyNhanVien()
        {
            InitializeComponent();
        }

        private void frmQuanLyNhanVien_Load(object sender, EventArgs e)
        {
            this.Text = "Quản Lý Nhân Viên";
            SetupDataGridView();
            LoadAllNhanVien();
        }
        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvNhanVien.Rows[e.RowIndex];
            if (row.Cells["MaNhanVien"].Value != null)
            {
                maNhanVienDangChon = Convert.ToInt32(row.Cells["MaNhanVien"].Value);
            }
        }
        private void SetupDataGridView()
        {
            dgvNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvNhanVien.AllowUserToAddRows = false;
            dgvNhanVien.ReadOnly = true;
            dgvNhanVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvNhanVien.RowHeadersVisible = false;
        }
        private void LoadAllNhanVien()
        {
            try
            {
                List<NhanVien> danhSach = _nhanVienService.GetAllNhanVien();

                DataTable dt = new DataTable();
                dt.Columns.Add("Mã NV", typeof(int));
                dt.Columns.Add("Họ Tên");
                dt.Columns.Add("Tài Khoản");
                dt.Columns.Add("Vai Trò");
                dt.Columns.Add("Điện Thoại");

                foreach (var nv in danhSach)
                {
                    dt.Rows.Add(
                        nv.MaNV,
                        nv.HoTen,
                        nv.TaiKhoan,
                        nv.VaiTro,
                        nv.DienThoai
                    );
                }

                dgvNhanVien.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách nhân viên: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDataToGrid(List<NhanVien> danhSach)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Mã NV", typeof(int));
            dt.Columns.Add("Họ Tên");
            dt.Columns.Add("Tài Khoản");
            dt.Columns.Add("Vai Trò");
            dt.Columns.Add("Điện Thoại");
            foreach (var nv in danhSach)
            {
                dt.Rows.Add(nv.MaNV, nv.HoTen, nv.TaiKhoan, nv.VaiTro, nv.DienThoai );
            }

            dgvNhanVien.DataSource = dt;
        }
        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtTimKiem.Text.Trim();
                var danhSach = _nhanVienService.TimKiemNhanVien(keyword);
                LoadDataToGrid(danhSach);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message);
            }
        }


        private void dgvNhanVien_SelectionChanged(object sender, EventArgs e)
        {
            
        }
        private void dgvNhanVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
             frmThemNhanVien f = new frmThemNhanVien();
             if (f.ShowDialog() == DialogResult.OK)
             LoadAllNhanVien();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvNhanVien.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần sửa!");
                return;
            }

            int maNV = Convert.ToInt32(dgvNhanVien.CurrentRow.Cells["Mã NV"].Value);
            frmSuaNhanVien f = new frmSuaNhanVien(maNV);

            if (f.ShowDialog() == DialogResult.OK)
            {
                LoadAllNhanVien();   
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvNhanVien.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần xóa!", "Thông báo");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa nhân viên này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                int maNV = Convert.ToInt32(dgvNhanVien.CurrentRow.Cells["Mã NV"].Value);
                bool success = _nhanVienService.XoaNhanVien(maNV);

                if (success)
                {
                    MessageBox.Show("Xóa nhân viên thành công!", "Thành công");
                    LoadAllNhanVien();
                }
                else
                {
                    MessageBox.Show("Xóa thất bại!", "Lỗi");
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            LoadAllNhanVien();
        }
    }
}
