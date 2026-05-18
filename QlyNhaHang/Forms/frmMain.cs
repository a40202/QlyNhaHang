using QlyNhaHang.BLL;
using QlyNhaHang.BLL;
using QlyNhaHang.Helpers;
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
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        private void sysStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            LoadUserInfo();
            CustomizeDesign();
            ApplyRolePermissions();
        }

        private void SetupSidebarButtons()
        {     
            foreach (Control ctrl in panelSidebar.Controls)
            {
                if (ctrl is Button btn)
                {
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.BackColor = Color.FromArgb(33, 47, 60);
                    btn.ForeColor = Color.White;
                    btn.TextAlign = ContentAlignment.MiddleLeft;
                    btn.Padding = new Padding(20, 0, 0, 0);
                    btn.Height = 50;
                    btn.Font = new Font("Segoe UI", 10.5F);

                    // Hover effect
                    btn.MouseEnter += (s, e) =>
                    {
                        if (btn != currentButton)
                            btn.BackColor = Color.FromArgb(44, 62, 80);
                    };
                    btn.MouseLeave += (s, e) =>
                    {
                        if (btn != currentButton)
                            btn.BackColor = Color.FromArgb(33, 47, 60);
                    };
                }
            }
        }

        private Button currentButton;

        private void ActivateButton(Button btn)
        {
            if (currentButton != null)
                currentButton.BackColor = Color.FromArgb(33, 47, 60);

            currentButton = btn;
            btn.BackColor = Color.FromArgb(0, 122, 204);
        }

        //======================custin design =================//
        private void CustomizeDesign()
        {
            this.WindowState = FormWindowState.Normal;
            this.Text = "Phần Mềm Quản Lý Nhà Hàng";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;       

            panelSidebar.Width = 230;
            panelSidebar.BackColor = Color.FromArgb(33, 47, 60);

            // Khu vực hiển thị form con
            panelContent.BackColor = Color.FromArgb(240, 240, 240);

            SetupSidebarButtons();
            LoadUserInfo();
            ApplyRolePermissions();
        }
        //=======================phân quyền vai trò=================//
        private void ApplyRolePermissions()
        {
            // Ẩn tất cả trước
            btnQuanLyBanAn.Visible = false;
            btnQuanLyMonAn.Visible = false;
            btnQuanLyHoaDon.Visible = false;
            btnBaoCao.Visible = false;
            btnQuanLyNhanVien.Visible = false;

            if (CurrentUser.IsAdmin())
            {
                btnQuanLyBanAn.Visible = true;
                btnQuanLyMonAn.Visible = true;
                btnQuanLyHoaDon.Visible = true;
                btnBaoCao.Visible = true;
                btnQuanLyNhanVien.Visible = true;
            }
            else if (CurrentUser.IsThuNgan())
            {
                btnQuanLyHoaDon.Visible = true;
                btnQuanLyBanAn.Visible = true;
            }
            else if (CurrentUser.IsBep())
            {
                btnQuanLyMonAn.Visible = true;
            }
            else if (CurrentUser.IsPhucVu())
            {
                btnQuanLyBanAn.Visible = true;
                btnQuanLyHoaDon.Visible = true;
            }
            else
            {
                MessageBox.Show("Vai trò không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        //=======================Load Thông tin User=================//
        private void LoadUserInfo()
        {
            lblUserInfo.Text = $"Xin chào: {CurrentUser.HoTen} \n{CurrentUser.VaiTro}";
        }
        private void OpenChildForm(Form childForm)
        {
            panelContent.Controls.Clear();
            childForm.TopLevel = false;
            childForm.Dock = DockStyle.Fill;
            childForm.FormBorderStyle = FormBorderStyle.None;
            panelContent.Controls.Add(childForm);
            childForm.Show();
        }
        //===========================================
        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ActivateButton(btnQuanLyHoaDon);
            OpenChildForm(new frmQuanLyHoaDon());
        }

        private void btnFormBanAn_Click(object sender, EventArgs e)
        {
            ActivateButton(btnQuanLyBanAn);
            OpenChildForm(new frmBanAn());
        }

        private void btnQuanLyMonAn_Click(object sender, EventArgs e)
        {
            ActivateButton(btnQuanLyMonAn);
            OpenChildForm(new frmQuanLyMonAn());
        }

        private void btnQuanLyNhanVien_Click(object sender, EventArgs e)
        {
            ActivateButton(btnQuanLyNhanVien);
            OpenChildForm(new frmQuanLyNhanVien());
        }

        private void btnBaoCao_Click(object sender, EventArgs e)
        {
            ActivateButton(btnBaoCao);
            OpenChildForm(new frmBaoCaoAdmin());
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {   
            this.Hide();
            frmLogin fLogin = new frmLogin();
            fLogin.ShowDialog();
            this.Close();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDoiMatKhau fDoiMatKhau = new frmDoiMatKhau();
            fDoiMatKhau.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
