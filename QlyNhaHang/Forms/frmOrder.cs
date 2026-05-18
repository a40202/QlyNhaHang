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
    public partial class frmOrder : Form
    {
        private readonly int _maBan;
        private readonly BanAnService _banAnService = new BanAnService();
        private readonly HoaDonService _hoaDonService = new HoaDonService();
        private readonly OrderTamService _orderTamService = new OrderTamService();
        private readonly MonAnService _monAnService = new MonAnService();
        private List<OrderTam> _gioHang = new List<OrderTam>();
        public frmOrder(int maBan)
        {
            InitializeComponent();
            _maBan = maBan;
        }
        
        private void LoadDanhSachMonAn()
        {
            try
            {
                var danhSach = _monAnService.GetAll();

                dgvMonAn.DataSource = danhSach;

                if (dgvMonAn.Columns["Gia"] != null)
                    dgvMonAn.Columns["Gia"].DefaultCellStyle.Format = "N0";

                if (dgvMonAn.Columns["TrangThai"] != null)
                    dgvMonAn.Columns["TrangThai"].Visible = false;

                dgvMonAn.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvMonAn.AllowUserToAddRows = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách món ăn: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
       

        private void frmOrder_Load(object sender, EventArgs e)
        {
            this.Text = $"Gọi Món - Bàn {_maBan}";
            LoadGioHang();
            LoadDanhSachMonAn();
        }
        private void LoadGioHang()
        {
            dgvGioHang.DataSource = null;
            dgvGioHang.DataSource = _gioHang;

            if (dgvGioHang.Columns["ThanhTien"] != null)
                dgvGioHang.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";


        }
        private void btnThemMon_Click_1(object sender, EventArgs e)
        {
            if (dgvMonAn.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn món ăn!", "Thông báo");
                return;
            }

            int maMon = Convert.ToInt32(dgvMonAn.CurrentRow.Cells["MaMon"].Value);
            string tenMon = dgvMonAn.CurrentRow.Cells["TenMon"].Value.ToString();
            decimal gia = Convert.ToDecimal(dgvMonAn.CurrentRow.Cells["Gia"].Value);
            int soLuong = (int)nudSoLuong.Value;

            if (soLuong <= 0)
            {
                MessageBox.Show("Số lượng phải lớn hơn 0!", "Thông báo");
                return;
            }

            // Thêm vào giỏ hàng tạm
            var item = new OrderTam
            {
                MaBan = _maBan,
                MaMon = maMon,
                TenMon = tenMon,
                SoLuong = soLuong,
                DonGia = gia,
                ThanhTien = soLuong * gia,
                ThoiGian = DateTime.Now
            };

            _gioHang.Add(item);
            LoadGioHang();

            nudSoLuong.Value = 1;
        }
        

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (_gioHang.Count == 0)
            {
                MessageBox.Show("Giỏ hàng trống!", "Thông báo");
                return;
            }

            try
            {
                foreach (var item in _gioHang)
                {
                    _orderTamService.ThemMon(item.MaBan, item.MaMon, item.SoLuong, item.DonGia);
                }

                // Cập nhật trạng thái bàn
                _banAnService.CapNhatTrangThaiBan(_maBan, "Đang Sử Dụng");

                MessageBox.Show($"Đã thêm {_gioHang.Count} món vào bàn {_maBan}", "Thành công");

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu order: " + ex.Message, "Lỗi");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvGioHang.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn món cần xóa trong giỏ hàng!", "Thông báo");
                return;
            }

            try
            {
                int index = dgvGioHang.CurrentRow.Index;

                if (index >= 0 && index < _gioHang.Count)
                {
                    string tenMon = _gioHang[index].TenMon;
                    _gioHang.RemoveAt(index);

                    LoadGioHang();   

                    MessageBox.Show($"Đã xóa {tenMon} khỏi giỏ hàng.", "Đã xóa",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa món: " + ex.Message);
            }
        }
        private void dgvGioHang_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvGioHang.CurrentRow == null)
            {
                return;
            }

            try
            {
                int index = e.RowIndex;
                string tenMon = _gioHang[index].TenMon;

                MessageBox.Show($"Món: {tenMon}\n" +
                               $"Số lượng: {_gioHang[index].SoLuong}\n" +
                               $"Đơn giá: {_gioHang[index].DonGia:N0} VNĐ",
                               "Chi tiết món",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xem chi tiết: " + ex.Message);
            }
        }
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadGioHang();
            LoadDanhSachMonAn();
        }
    }
}
