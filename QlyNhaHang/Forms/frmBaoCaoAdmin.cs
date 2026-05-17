using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using QlyNhaHang.BLL;
using QlyNhaHang.Models;

namespace QlyNhaHang.Forms
{
    public partial class frmBaoCaoAdmin : Form
    {
        private readonly BaoCaoService _baoCaoService = new BaoCaoService();
        public frmBaoCaoAdmin()
        {
            InitializeComponent();
        }

        private void frmBaoCaoAdmin_Load(object sender, EventArgs e)
        {
            this.Text = "Báo Cáo Admin - Doanh Thu Theo Tháng";

            // Thiết lập NumericUpDown Năm
            nudNam.Minimum = 2020;
            nudNam.Maximum = 2030;
            nudNam.Value = DateTime.Now.Year;

            // Thiết lập ComboBox Tháng
            cboThang.Items.Clear();
            cboThang.Items.Add("Tất cả các tháng");
            for (int i = 1; i <= 12; i++)
            {
                cboThang.Items.Add($"Tháng {i}");
            }
            cboThang.SelectedIndex = 0;

            SetupDataGridView();
            LoadBaoCao();
        }

        private void SetupDataGridView()
        {
            dgvBaoCao.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBaoCao.ColumnHeadersHeight = 40;
            dgvBaoCao.RowTemplate.Height = 25;
            dgvBaoCao.AutoGenerateColumns = true;
        }


        //======================
        private void VeChart(List<string> nhanX, List<decimal> doanhThu, List<decimal> tangTruong)
        {
            chartBaoCao.Series.Clear();
            chartBaoCao.ChartAreas.Clear();
            chartBaoCao.Legends.Clear();

            // ========== 2 CHART AREA: trên = DoanhThu, dưới = TangTruong ==========
            var areaDoanhThu = new ChartArea("areaDoanhThu");
            var areaTangTruong = new ChartArea("areaTangTruong");

            // Chia tỉ lệ: DoanhThu 65%, TangTruong 35%
            areaDoanhThu.Position = new ElementPosition(0, 0, 100, 65);
            areaTangTruong.Position = new ElementPosition(0, 65, 100, 35);

            // Style chung
            foreach (var area in new[] { areaDoanhThu, areaTangTruong })
            {
                area.BackColor = Color.White;
                area.AxisX.MajorGrid.LineColor = Color.FromArgb(230, 230, 230);
                area.AxisY.MajorGrid.LineColor = Color.FromArgb(230, 230, 230);
                area.AxisX.LabelStyle.Font = new Font("Segoe UI", 8f);
                area.AxisY.LabelStyle.Font = new Font("Segoe UI", 8f);
                area.AxisX.LineColor = Color.FromArgb(200, 200, 200);
                area.AxisY.LineColor = Color.FromArgb(200, 200, 200);
            }

            areaTangTruong.AxisY.LabelStyle.Format = "+0.#'%';-0.#'%'";

            chartBaoCao.ChartAreas.Add(areaDoanhThu);
            chartBaoCao.ChartAreas.Add(areaTangTruong);

            // ========== SERIES DOANH THU ==========
            var seriesDoanhThu = new Series("Doanh Thu");
            seriesDoanhThu.ChartType = SeriesChartType.Column;
            seriesDoanhThu.ChartArea = "areaDoanhThu";
            seriesDoanhThu.Color = Color.FromArgb(0, 120, 215);
            seriesDoanhThu.LabelFormat = "N0";
            seriesDoanhThu.IsValueShownAsLabel = true;
            seriesDoanhThu.Font = new Font("Segoe UI", 7f);
            seriesDoanhThu.LabelForeColor = Color.FromArgb(0, 100, 180);

            for (int i = 0; i < nhanX.Count; i++)
                seriesDoanhThu.Points.AddXY(nhanX[i], (double)doanhThu[i]);

            // ========== SERIES TĂNG TRƯỞNG ==========
            var seriesTangTruong = new Series("Tăng Trưởng %");
            seriesTangTruong.ChartType = SeriesChartType.Column;
            seriesTangTruong.ChartArea = "areaTangTruong";
            seriesTangTruong.IsValueShownAsLabel = true;
            seriesTangTruong.Font = new Font("Segoe UI", 7f);
            seriesTangTruong.LabelFormat = "+0.#'%';-0.#'%'";


            for (int i = 0; i < nhanX.Count; i++)
            {
                var point = seriesTangTruong.Points.Add((double)tangTruong[i]);
                point.AxisLabel = nhanX[i];
                point.Color = tangTruong[i] >= 0 ? Color.FromArgb(0, 180, 0) : Color.Red;
                point.LabelForeColor = tangTruong[i] >= 0 ? Color.FromArgb(0, 150, 0) : Color.Red;
            }

            chartBaoCao.Series.Add(seriesDoanhThu);
            chartBaoCao.Series.Add(seriesTangTruong);

            // Legend
            var legend = new Legend();
            legend.Font = new Font("Segoe UI", 9f);
            legend.Docking = Docking.Top;
            chartBaoCao.Legends.Add(legend);
            seriesTangTruong.Color = Color.FromArgb(0, 180, 0);

            chartBaoCao.BackColor = Color.White;
            chartBaoCao.BorderlineColor = Color.FromArgb(220, 220, 220);
        }
        // ===================
        private void LoadBaoCao()
        {
            try
            {
                int nam = (int)nudNam.Value;
                int? thang = null;

                if (cboThang.SelectedIndex > 0)
                    thang = cboThang.SelectedIndex;

                decimal tongDT = 0;
                int tongHD = 0;
                decimal tangTruong = 0;

                if (thang.HasValue)
                {
                    // Chọn tháng cụ thể → hiện doanh thu theo ngày
                    var danhSachNgay = _baoCaoService.GetBaoCaoTheoNgayTrongThang(nam, thang.Value);
                    VeChart(
                        danhSachNgay.Select(x => x.NgayHienThi).ToList(),
                        danhSachNgay.Select(x => x.DoanhThu).ToList(),
                        danhSachNgay.Select(x => x.TangTruong).ToList()
                    );
                    dgvBaoCao.DataSource = danhSachNgay;

                    if (dgvBaoCao.Columns["Ngay"] != null)
                        dgvBaoCao.Columns["Ngay"].Visible = false;

                    if (dgvBaoCao.Columns["DoanhThu"] != null)
                        dgvBaoCao.Columns["DoanhThu"].DefaultCellStyle.Format = "N0";

                    if (dgvBaoCao.Columns["TangTruong"] != null)
                        dgvBaoCao.Columns["TangTruong"].DefaultCellStyle.Format = "+0.00'%';-0.00'%'";

                    tongDT = danhSachNgay.Sum(x => x.DoanhThu);
                    tongHD = danhSachNgay.Sum(x => x.SoHoaDon);

                    var coData = danhSachNgay.Where(x => x.TangTruong != 0).ToList();
                    tangTruong = coData.Any() ? coData.Average(x => x.TangTruong) : 0;
                }
                else
                {
                    // Tất cả tháng → hiện doanh thu theo tháng
                    var danhSachThang = _baoCaoService.GetBaoCaoTheoNamm(nam, null);
                    VeChart(
                        danhSachThang.Select(x => x.TenThang).ToList(),
                        danhSachThang.Select(x => x.DoanhThu).ToList(),
                        danhSachThang.Select(x => x.TangTruong).ToList()
                    );
                    dgvBaoCao.DataSource = danhSachThang;

                    if (dgvBaoCao.Columns["DoanhThu"] != null)
                        dgvBaoCao.Columns["DoanhThu"].DefaultCellStyle.Format = "N0";

                    if (dgvBaoCao.Columns["TangTruong"] != null)
                        dgvBaoCao.Columns["TangTruong"].DefaultCellStyle.Format = "+0.00'%';-0.00'%'";

                    tongDT = danhSachThang.Sum(x => x.DoanhThu);
                    tongHD = danhSachThang.Sum(x => x.SoHoaDon);

                    var coData = danhSachThang.Where(x => x.TangTruong != 0).ToList();
                    tangTruong = coData.Any() ? coData.Average(x => x.TangTruong) : 0;
                }

                // ========== TỔNG ==========
                lblTongDoanhThuNam.Text = tongDT.ToString("N0") + " VNĐ";
                lblTongHoaDonNam.Text = tongHD.ToString();

                // ========== TĂNG TRƯỞNG ==========
                string muiTen = tangTruong >= 0 ? "▲" : "▼";
                string dau = tangTruong >= 0 ? "+" : "";
                lblTangTruong.Text = $"{muiTen} {dau}{tangTruong:0.0}% so tháng trước";
                lblTangTruong.ForeColor = tangTruong >= 0 ? Color.FromArgb(0, 180, 0) : Color.Red;

                FormatDataGridView();
                dgvBaoCao.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải báo cáo: " + ex.Message);
            }
        }

     
        //==========================================

        private void DgvBaoCao_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;

            string colName = dgvBaoCao.Columns[e.ColumnIndex].Name;

            if (colName == "TangTruong" && e.Value != null)
            {
                decimal val = Convert.ToDecimal(e.Value);
                e.CellStyle.ForeColor = val > 0 ? Color.Green : val < 0 ? Color.Red : Color.Gray;
                e.CellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            }

            if (colName == "DoanhThu" && e.Value != null)
            {
                e.CellStyle.ForeColor = Color.FromArgb(0, 100, 180);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void FormatDataGridView()
        {
            if (dgvBaoCao.Columns["DoanhThu"] != null)
                dgvBaoCao.Columns["DoanhThu"].DefaultCellStyle.Format = "N0";

            if (dgvBaoCao.Columns["NgayHienThi"] != null)
                dgvBaoCao.Columns["NgayHienThi"].HeaderText = "Ngày";

            dgvBaoCao.CellFormatting -= DgvBaoCao_CellFormatting;
            dgvBaoCao.CellFormatting += DgvBaoCao_CellFormatting;


        }
        //==============================
        private void btnXem_Click(object sender, EventArgs e)
        {
            LoadBaoCao();
        }

        private void nudNam_ValueChanged(object sender, EventArgs e)
        {
            LoadBaoCao();
        }

        private void cboThang_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadBaoCao();
        }

        //===================
    }
}
