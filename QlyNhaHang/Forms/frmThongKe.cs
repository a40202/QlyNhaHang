using System;
using System.Windows.Forms;
using QlyNhaHang.DAL;

namespace QlyNhaHang
{
    public partial class frmThongKe : Form
    {
        ThongKeDAL dal = new ThongKeDAL();

        public frmThongKe()
        {
            InitializeComponent();
        }

        private void frmThongKe_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            dgvThongKe.DataSource = dal.GetDoanhThuTheoNgay();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
