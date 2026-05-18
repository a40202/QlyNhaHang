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
    public partial class frmThemMonAn : Form
    {
        private readonly MonAnService _monAnService = new MonAnService();
        private readonly DanhMucService _danhMucService = new DanhMucService();
        public frmThemMonAn()
        {
            InitializeComponent();
        }

        private void frmThemMonAn_Load(object sender, EventArgs e)
        {
            this.Text = "Thêm Món Ăn Mới";
            LoadDanhMucVaoComboBox();
            chkTrangThai.Checked = true;   
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenMon.Text))
            {
                MessageBox.Show("Vui lòng nhập tên món ăn!", "Thông báo");
                txtTenMon.Focus();
                return;
            }

            if (cboDanhMuc.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn danh mục!", "Thông báo");
                cboDanhMuc.Focus();
                return;
            }

            if (!decimal.TryParse(txtGia.Text, out decimal gia) || gia <= 0)
            {
                MessageBox.Show("Giá món ăn phải là số lớn hơn 0!", "Thông báo");
                txtGia.Focus();
                return;
            }

            try
            {
                var monAnMoi = new MonAn
                {
                    TenMon = txtTenMon.Text.Trim(),
                    Gia = gia,
                    MoTa = txtMoTa.Text.Trim(),
                    MaDanhMuc = Convert.ToInt32(cboDanhMuc.SelectedValue),
                    TrangThai = chkTrangThai.Checked
                };

                bool success = _monAnService.ThemMonAn(monAnMoi);

                if (success)
                {
                    MessageBox.Show("Thêm món ăn thành công!", "Thành công");

                    if (MessageBox.Show("Tiếp tục thêm món ăn khác?", "Tiếp tục",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        ClearForm();
                        txtTenMon.Focus();
                    }
                    else
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm món ăn:\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadDanhMucVaoComboBox()
        {
            try
            {
                var danhMucList = _danhMucService.GetAll();

                cboDanhMuc.DataSource = danhMucList;
                cboDanhMuc.DisplayMember = "TenDanhMuc";
                cboDanhMuc.ValueMember = "MaDanhMuc";
                cboDanhMuc.SelectedIndex = 0; // Chọn danh mục đầu tiên
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh mục: " + ex.Message);
            }
        }
        private void ClearForm()
        {
            txtTenMon.Clear();
            txtGia.Clear();
            txtMoTa.Clear();
            chkTrangThai.Checked = true;
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
                this.Close();
        }

        private void cboDanhMuc_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
