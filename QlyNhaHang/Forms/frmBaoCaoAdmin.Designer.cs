namespace QlyNhaHang.Forms
{
    partial class frmBaoCaoAdmin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.mySqlDataAdapter1 = new MySql.Data.MySqlClient.MySqlDataAdapter();
            this.nudNam = new System.Windows.Forms.NumericUpDown();
            this.cboThang = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.lblTongHoaDonNam = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTangTruong = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTongDoanhThuNam = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dgvBaoCao = new System.Windows.Forms.DataGridView();
            this.chartBaoCao = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.nudNam)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBaoCao)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartBaoCao)).BeginInit();
            this.SuspendLayout();
            // 
            // mySqlDataAdapter1
            // 
            this.mySqlDataAdapter1.DeleteCommand = null;
            this.mySqlDataAdapter1.InsertCommand = null;
            this.mySqlDataAdapter1.SelectCommand = null;
            this.mySqlDataAdapter1.UpdateCommand = null;
            // 
            // nudNam
            // 
            this.nudNam.Location = new System.Drawing.Point(186, 150);
            this.nudNam.Name = "nudNam";
            this.nudNam.Size = new System.Drawing.Size(120, 20);
            this.nudNam.TabIndex = 7;
            this.nudNam.ValueChanged += new System.EventHandler(this.nudNam_ValueChanged);
            // 
            // cboThang
            // 
            this.cboThang.FormattingEnabled = true;
            this.cboThang.Location = new System.Drawing.Point(41, 149);
            this.cboThang.Name = "cboThang";
            this.cboThang.Size = new System.Drawing.Size(121, 21);
            this.cboThang.TabIndex = 8;
            this.cboThang.SelectedIndexChanged += new System.EventHandler(this.cboThang_SelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Chocolate;
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.lblTongHoaDonNam);
            this.panel2.Location = new System.Drawing.Point(197, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(190, 106);
            this.panel2.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label3.Location = new System.Drawing.Point(21, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "Tổng hoá đơn ";
            // 
            // lblTongHoaDonNam
            // 
            this.lblTongHoaDonNam.AutoSize = true;
            this.lblTongHoaDonNam.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblTongHoaDonNam.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblTongHoaDonNam.Location = new System.Drawing.Point(21, 50);
            this.lblTongHoaDonNam.Name = "lblTongHoaDonNam";
            this.lblTongHoaDonNam.Size = new System.Drawing.Size(14, 15);
            this.lblTongHoaDonNam.TabIndex = 5;
            this.lblTongHoaDonNam.Text = "0";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.CornflowerBlue;
            this.panel1.BackgroundImage = global::QlyNhaHang.Properties.Resources.OAK3ZH0;
            this.panel1.Controls.Add(this.lblTangTruong);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lblTongDoanhThuNam);
            this.panel1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(188, 106);
            this.panel1.TabIndex = 9;
            // 
            // lblTangTruong
            // 
            this.lblTangTruong.AutoSize = true;
            this.lblTangTruong.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTangTruong.Location = new System.Drawing.Point(13, 74);
            this.lblTangTruong.Name = "lblTangTruong";
            this.lblTangTruong.Size = new System.Drawing.Size(26, 15);
            this.lblTangTruong.TabIndex = 4;
            this.lblTangTruong.Text = "lbel";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(13, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Tổng doanh thu";
            // 
            // lblTongDoanhThuNam
            // 
            this.lblTongDoanhThuNam.AutoSize = true;
            this.lblTongDoanhThuNam.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblTongDoanhThuNam.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblTongDoanhThuNam.Location = new System.Drawing.Point(13, 50);
            this.lblTongDoanhThuNam.Name = "lblTongDoanhThuNam";
            this.lblTongDoanhThuNam.Size = new System.Drawing.Size(14, 15);
            this.lblTongDoanhThuNam.TabIndex = 1;
            this.lblTongDoanhThuNam.Text = "0";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Controls.Add(this.panel2);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(38, 19);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(413, 109);
            this.flowLayoutPanel1.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(38, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Tháng";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(183, 131);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 15);
            this.label4.TabIndex = 13;
            this.label4.Text = "Năm";
            // 
            // dgvBaoCao
            // 
            this.dgvBaoCao.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBaoCao.Location = new System.Drawing.Point(41, 187);
            this.dgvBaoCao.Name = "dgvBaoCao";
            this.dgvBaoCao.ReadOnly = true;
            this.dgvBaoCao.Size = new System.Drawing.Size(384, 362);
            this.dgvBaoCao.TabIndex = 0;
            // 
            // chartBaoCao
            // 
            chartArea2.Name = "ChartArea1";
            this.chartBaoCao.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartBaoCao.Legends.Add(legend2);
            this.chartBaoCao.Location = new System.Drawing.Point(449, 19);
            this.chartBaoCao.Name = "chartBaoCao";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartBaoCao.Series.Add(series2);
            this.chartBaoCao.Size = new System.Drawing.Size(323, 530);
            this.chartBaoCao.TabIndex = 14;
            this.chartBaoCao.Text = "chart1";
            // 
            // frmBaoCaoAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.chartBaoCao);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.cboThang);
            this.Controls.Add(this.nudNam);
            this.Controls.Add(this.dgvBaoCao);
            this.Name = "frmBaoCaoAdmin";
            this.Text = "frmBaoCaoAdmin";
            this.Load += new System.EventHandler(this.frmBaoCaoAdmin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudNam)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBaoCao)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartBaoCao)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MySql.Data.MySqlClient.MySqlDataAdapter mySqlDataAdapter1;
        private System.Windows.Forms.NumericUpDown nudNam;
        private System.Windows.Forms.ComboBox cboThang;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblTongHoaDonNam;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTongDoanhThuNam;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dgvBaoCao;
        private System.Windows.Forms.Label lblTangTruong;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartBaoCao;
    }
}