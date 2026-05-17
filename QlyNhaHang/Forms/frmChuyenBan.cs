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
    public partial class frmChuyenBan : Form
    {
        private readonly int _maBanCu;
        private readonly OrderTamService _orderTamService = new OrderTamService();
        private readonly BanAnService _banAnService = new BanAnService();
        public frmChuyenBan(int maBanCu)
        {
            InitializeComponent();
            _maBanCu = maBanCu;
        }
        private void LoadComboBoxBanDich()
        {
            try
            {
                var dsBan = _banAnService.GetAllBanAn();

                // Lọc bỏ bàn hiện tại
                var danhSachBanDich = dsBan.Where(b => b.MaBan != _maBanCu).ToList();

                cboBanAn.DataSource = danhSachBanDich;
                cboBanAn.DisplayMember = "TenBan";
                cboBanAn.ValueMember = "MaBan";

                if (danhSachBanDich.Count == 0)
                {
                    MessageBox.Show("Không còn bàn nào để chuyển!", "Thông báo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách bàn đích: " + ex.Message);
            }
        }
        private void frmChuyenBan_Load(object sender, EventArgs e)
        {
            this.Text = $"Chuyển bàn {_maBanCu}";
            LoadComboBoxBanDich();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (cboBanAn.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn bàn đích!", "Thông báo");
                return;
            }

            int maBanMoi = Convert.ToInt32(cboBanAn.SelectedValue);

            if (MessageBox.Show($"Chuyển toàn bộ order từ Bàn {_maBanCu} sang Bàn {maBanMoi}?",
                "Xác nhận chuyển bàn", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.Yes)
            {
                bool success = _orderTamService.ChuyenBan(_maBanCu, maBanMoi);

                if (success)
                {
                    MessageBox.Show("Chuyển bàn thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Chuyển bàn thất bại!", "Lỗi");
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
