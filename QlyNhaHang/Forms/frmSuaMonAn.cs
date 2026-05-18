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
    public partial class frmSuaMonAn : Form
    {
        private readonly MonAnService _monAnService = new MonAnService();
        private readonly DanhMucService _danhMucService = new DanhMucService();
        private readonly int _maMon;
        public frmSuaMonAn(int maMon)
        {
            InitializeComponent();
            _maMon = maMon;
        }

        private void frmSuaMonAn_Load(object sender, EventArgs e)
        {
            this.Text = "Sửa Thông Tin Món Ăn";
            LoadDanhMucVaoComboBox();
            LoadThongTinMonAn();
        }

        private void LoadDanhMucVaoComboBox()
        {
            try
            {
                var danhMucList = _danhMucService.GetAll();
                cboDanhMuc.DataSource = danhMucList;
                cboDanhMuc.DisplayMember = "TenDanhMuc";
                cboDanhMuc.ValueMember = "MaDanhMuc";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh mục: " + ex.Message);
            }
        }

        private void LoadThongTinMonAn()
        {
            try
            {              
                MonAn monAn = _monAnService.GetById(_maMon);

                if (monAn != null)
                {
                    txtTenMon.Text = monAn.TenMon;
                    txtGia.Text = monAn.Gia.ToString();
                    txtMoTa.Text = monAn.MoTa ?? "";
                    chkTrangThai.Checked = monAn.TrangThai;

                    if (monAn.MaDanhMuc > 0)
                    {
                        cboDanhMuc.SelectedValue = monAn.MaDanhMuc;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thông tin món ăn: " + ex.Message);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenMon.Text))
            {
                MessageBox.Show("Vui lòng nhập tên món ăn!", "Thông báo");
                txtTenMon.Focus();
                return;
            }

            if (!decimal.TryParse(txtGia.Text, out decimal gia) || gia <= 0)
            {
                MessageBox.Show("Giá món ăn phải lớn hơn 0!", "Thông báo");
                txtGia.Focus();
                return;
            }

            try
            {
                var monAn = new MonAn
                {
                    MaMon = _maMon,
                    TenMon = txtTenMon.Text.Trim(),
                    Gia = gia,
                    MoTa = txtMoTa.Text.Trim(),
                    MaDanhMuc = Convert.ToInt32(cboDanhMuc.SelectedValue),
                    TrangThai = chkTrangThai.Checked
                };

                bool success = _monAnService.CapNhatMonAn(monAn);

                if (success)
                {
                    MessageBox.Show("Cập nhật món ăn thành công!", "Thành công");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
