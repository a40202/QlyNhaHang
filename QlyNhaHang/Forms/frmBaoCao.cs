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
using System.Windows.Forms.DataVisualization.Charting;
using QlyNhaHang.BLL;

namespace QlyNhaHang.Forms
{
    public partial class frmBaoCao : Form
    {   
        private readonly BaoCaoService _baoCaoService = new BaoCaoService();
        public frmBaoCao()
        {
            InitializeComponent();
        }
        //================================
        private void frmBaoCao_Load(object sender, EventArgs e)
        {
            foreach (Control c in this.Controls)
                if (c is BindingNavigator) c.Visible = false;
            this.Text = "Báo Cáo & Thống Kê Doanh Thu";
            SetupUI();
            SetupForm();
            LoadBaoCaoTongHop();       
        }
        //===================
        private void SetupUI()
        {
            // Card style
            SetupCardStyle(panelDoanhThu);
            SetupCardStyle(panelSoHoaDon);
            SetupCardStyle(panelTrungBinh);
            SetupCardStyle(panelGiamGia);
        }
        //======================
        private void SetupCardStyle(Panel panel)
        {
            panel.BackColor = Color.White;
            panel.BorderStyle = BorderStyle.FixedSingle;
        }
        //================================
        private void LoadBaoCaoTongHop()
        {
            try
            {
                BaoCaoTongHop bc = _baoCaoService.GetBaoCaoTongHop();

                // 4 card
                lblTongDoanhThu.Text = bc.TongDoanhThu.ToString("N0") + " đ";
                lblSoHoaDon.Text = bc.SoHoaDon.ToString("N0");
                lblTrungBinhHD.Text = bc.TrungBinhHoaDon.ToString("N0") + " đ";
                lblTongGiamGia.Text = bc.TongGiamGia.ToString("N0") + " đ";

                // Biểu đồ cột doanh thu
                DrawDoanhThuTheoThang(chartDoanhThu, bc.DoanhThuTheoThang);

                // Biểu đồ tròn cơ cấu
                DrawCoCauChart(chartCoCau, bc.CoCauDoanhThu);

                // Bảng chi tiết theo tháng
                //BindChiTietThang(bc.ChiTietTheoThang);

                // Top món bán chạy
                //BindTopMonAn(bc.TopMonAn);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải báo cáo: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //================================
        private void SetupForm()
        {
            cboLoaiThongKe.Items.Clear();
            cboLoaiThongKe.Items.AddRange(new string[] { "Theo Ngày", "Theo Tháng", "Theo Năm" });
            cboLoaiThongKe.SelectedIndex = 1;           // mặc định Theo Tháng

            dtpTuNgay.Value = new DateTime(DateTime.Now.Year, 1, 1);
            dtpDenNgay.Value = DateTime.Now;

            SetupChartStyle();
        }
        private void SetupChartStyle()
        {
            // chartDoanhThu — cột
            ConfigChartArea(chartDoanhThu);
            chartDoanhThu.ChartAreas[0].AxisX.Interval = 1;
            chartDoanhThu.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
            chartDoanhThu.ChartAreas[0].AxisY.LabelStyle.Format = "N0";

            // chartCoCau — tròn
            ConfigChartArea(chartCoCau);
        }
        private void ConfigChartArea(Chart chart)
        {
            chart.BackColor = Color.White;
            chart.ChartAreas[0].BackColor = Color.White;
            chart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.FromArgb(220, 220, 220);
            chart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(220, 220, 220);
            chart.Legends.Clear();
        }
        //================================
        private void DrawDoanhThuTheoThang(Chart chart, List<DoanhThuThang> data)
        {
            chart.Series.Clear();

            // Cột doanh thu
            var seriesDT = new Series("Doanh thu")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.FromArgb(0, 122, 204),
                IsValueShownAsLabel = false,
                BorderWidth = 1,
                YAxisType = AxisType.Primary
            };

            // Đường số hoá đơn (trục Y phụ)
            var seriesHD = new Series("Số HĐ")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.FromArgb(16, 124, 16),
                BorderWidth = 2,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 7,
                YAxisType = AxisType.Secondary
            };

            foreach (var item in data)
            {
                seriesDT.Points.AddXY(item.Thang, item.DoanhThu);
                seriesHD.Points.AddXY(item.Thang, item.SoHoaDon);
            }

            chart.Series.Add(seriesDT);
            chart.Series.Add(seriesHD);

            // Trục Y phụ
            if (chart.ChartAreas[0].AxisY2 != null)
            {
                chart.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
                chart.ChartAreas[0].AxisY2.LabelStyle.ForeColor = Color.FromArgb(16, 124, 16);
                chart.ChartAreas[0].AxisY2.MajorGrid.Enabled = false;
            }

            // Legend
            chart.Legends.Clear();
            var leg = new Legend { Docking = Docking.Bottom, Font = new Font("Segoe UI", 8f) };
            chart.Legends.Add(leg);
            seriesDT.Legend = leg.Name;
            seriesHD.Legend = leg.Name;
            seriesDT.IsVisibleInLegend = true;
            seriesHD.IsVisibleInLegend = true;
        }
        //================================

        private void DrawCoCauChart(Chart chart, List<CoCauDoanhThu> data)
        {
            chart.Series.Clear();

            Series series = new Series("CoCau")
            {
                ChartType = SeriesChartType.Pie,
                IsValueShownAsLabel = true,
                LabelForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };

            foreach (var item in data)
            {
                series.Points.AddXY(item.Loai, item.TyLe);
            }

            chart.Series.Add(series);

            // Tùy chỉnh hiển thị
            chart.Series[0]["PieLabelStyle"] = "Outside";
            chart.Series[0]["PieLineColor"] = "Black";
        }
        //=====================================
      
        private void btnXem_Click(object sender, EventArgs e)
        {
            DateTime tuNgay = dtpTuNgay.Value.Date;
            DateTime denNgay = dtpDenNgay.Value.Date;

            switch (cboLoaiThongKe.SelectedIndex)
            {
                case 0: // Theo Ngày — dùng đúng khoảng đã chọn
                    break;

                case 1: // Theo Tháng — đầu tháng → cuối tháng của tuNgay
                    tuNgay = new DateTime(tuNgay.Year, tuNgay.Month, 1);
                    denNgay = tuNgay.AddMonths(1).AddDays(-1);
                    break;

                case 2: // Theo Năm — cả năm của tuNgay
                    tuNgay = new DateTime(tuNgay.Year, 1, 1);
                    denNgay = new DateTime(tuNgay.Year, 12, 31);
                    break;
            }

            if (tuNgay > denNgay)
            {
                MessageBox.Show("Ngày bắt đầu phải nhỏ hơn ngày kết thúc.", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoadBaoCaoTheoNgay(tuNgay, denNgay);
        }
        //===============================================
        private void LoadBaoCaoTheoNgay(DateTime tuNgay, DateTime denNgay)
        {
            try
            {
                BaoCaoTheoNgay bc = _baoCaoService.GetBaoCaoTheoNgay(tuNgay, denNgay);

                lblTongDoanhThu.Text = bc.TongDoanhThu.ToString("N0") + " đ";
                lblSoHoaDon.Text = bc.SoHoaDon.ToString("N0");

                DrawDoanhThuChart(bc.DanhSachHoaDon);
                //BindHoaDon(bc.DanhSachHoaDon);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải báo cáo: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // ── Vẽ biểu đồ cột theo ngày (khi lọc khoảng ngày) ──────────────────
        private void DrawDoanhThuChart(List<HoaDon> danhSach)
        {
            chartDoanhThu.Series.Clear();

            var series = new Series("Doanh thu")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.FromArgb(0, 122, 204),
                IsValueShownAsLabel = false
            };

            var grouped = danhSach
                .GroupBy(h => h.NgayLap.Date)
                .OrderBy(g => g.Key);

            foreach (var g in grouped)
                series.Points.AddXY(g.Key.ToString("dd/MM"), g.Sum(h => h.ThanhToan));

            chartDoanhThu.Series.Add(series);
            chartDoanhThu.Legends.Clear();
        }

        private void chartCoCau_Click(object sender, EventArgs e)
        {

        }
        private void cboLoaiThongKe_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.IsHandleCreated) return;
        }
    }
}
