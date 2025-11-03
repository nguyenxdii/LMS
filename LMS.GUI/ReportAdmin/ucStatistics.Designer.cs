namespace LMS.GUI.ReportAdmin
{
    partial class ucStatistics
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle85 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle86 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle87 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle88 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle89 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle90 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pieOrderStatus = new LiveCharts.WinForms.PieChart();
            this.lblRevenueValue = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tabDrivers = new System.Windows.Forms.TabPage();
            this.chartTopDrivers = new LiveCharts.WinForms.CartesianChart();
            this.dgvDriverDetails = new Guna.UI2.WinForms.Guna2DataGridView();
            this.tabCustomers = new System.Windows.Forms.TabPage();
            this.chartTopCustomers = new LiveCharts.WinForms.CartesianChart();
            this.dgvCustomerDetails = new Guna.UI2.WinForms.Guna2DataGridView();
            this.tabOperations = new System.Windows.Forms.TabPage();
            this.chartTopRoutes = new LiveCharts.WinForms.CartesianChart();
            this.pieShipmentStatus = new LiveCharts.WinForms.PieChart();
            this.tabRevenue = new System.Windows.Forms.TabPage();
            this.chartRevenueOverTime = new LiveCharts.WinForms.CartesianChart();
            this.tabOverview = new System.Windows.Forms.TabPage();
            this.btnExportOverview = new Guna.UI2.WinForms.Guna2Button();
            this.tabControlMain = new Guna.UI2.WinForms.Guna2TabControl();
            this.guna2Panel4 = new Guna.UI2.WinForms.Guna2Panel();
            this.lblCompletedValue = new System.Windows.Forms.Label();
            this.guna2GroupBox1 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.btnCustom = new Guna.UI2.WinForms.Guna2Button();
            this.btnYear = new Guna.UI2.WinForms.Guna2Button();
            this.btnQuarter = new Guna.UI2.WinForms.Guna2Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnMonth = new Guna.UI2.WinForms.Guna2Button();
            this.dtpFrom = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.btnWeek = new Guna.UI2.WinForms.Guna2Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnToday = new Guna.UI2.WinForms.Guna2Button();
            this.dtpTo = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.guna2Panel3 = new Guna.UI2.WinForms.Guna2Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTotalOrdersValue = new System.Windows.Forms.Label();
            this.guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            this.lblInProgressValue = new System.Windows.Forms.Label();
            this.lblInProgressValue1 = new System.Windows.Forms.Label();
            this.tabDrivers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDriverDetails)).BeginInit();
            this.tabCustomers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomerDetails)).BeginInit();
            this.tabOperations.SuspendLayout();
            this.tabRevenue.SuspendLayout();
            this.tabOverview.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.guna2Panel4.SuspendLayout();
            this.guna2GroupBox1.SuspendLayout();
            this.guna2Panel3.SuspendLayout();
            this.guna2Panel1.SuspendLayout();
            this.guna2Panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pieOrderStatus
            // 
            this.pieOrderStatus.Location = new System.Drawing.Point(6, 16);
            this.pieOrderStatus.Name = "pieOrderStatus";
            this.pieOrderStatus.Size = new System.Drawing.Size(934, 400);
            this.pieOrderStatus.TabIndex = 0;
            this.pieOrderStatus.Text = "pieChart1";
            // 
            // lblRevenueValue
            // 
            this.lblRevenueValue.AutoSize = true;
            this.lblRevenueValue.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.lblRevenueValue.Location = new System.Drawing.Point(9, 34);
            this.lblRevenueValue.Name = "lblRevenueValue";
            this.lblRevenueValue.Size = new System.Drawing.Size(111, 54);
            this.lblRevenueValue.TabIndex = 2;
            this.lblRevenueValue.Text = "1000";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label9.Location = new System.Drawing.Point(78, 4);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(106, 25);
            this.label9.TabIndex = 2;
            this.label9.Text = "Doanh Thu";
            // 
            // tabDrivers
            // 
            this.tabDrivers.Controls.Add(this.chartTopDrivers);
            this.tabDrivers.Controls.Add(this.dgvDriverDetails);
            this.tabDrivers.Location = new System.Drawing.Point(184, 4);
            this.tabDrivers.Name = "tabDrivers";
            this.tabDrivers.Padding = new System.Windows.Forms.Padding(3);
            this.tabDrivers.Size = new System.Drawing.Size(946, 854);
            this.tabDrivers.TabIndex = 5;
            this.tabDrivers.Text = "Tài Xế";
            this.tabDrivers.UseVisualStyleBackColor = true;
            // 
            // chartTopDrivers
            // 
            this.chartTopDrivers.Location = new System.Drawing.Point(6, 6);
            this.chartTopDrivers.Name = "chartTopDrivers";
            this.chartTopDrivers.Size = new System.Drawing.Size(934, 400);
            this.chartTopDrivers.TabIndex = 5;
            this.chartTopDrivers.Text = "cartesianChart4";
            // 
            // dgvDriverDetails
            // 
            dataGridViewCellStyle85.BackColor = System.Drawing.Color.White;
            this.dgvDriverDetails.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle85;
            this.dgvDriverDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            dataGridViewCellStyle86.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle86.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle86.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            dataGridViewCellStyle86.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle86.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle86.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle86.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDriverDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle86;
            this.dgvDriverDetails.ColumnHeadersHeight = 4;
            this.dgvDriverDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle87.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle87.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle87.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            dataGridViewCellStyle87.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle87.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle87.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle87.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDriverDetails.DefaultCellStyle = dataGridViewCellStyle87;
            this.dgvDriverDetails.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvDriverDetails.Location = new System.Drawing.Point(23, 447);
            this.dgvDriverDetails.Name = "dgvDriverDetails";
            this.dgvDriverDetails.RowHeadersVisible = false;
            this.dgvDriverDetails.RowHeadersWidth = 62;
            this.dgvDriverDetails.RowTemplate.Height = 28;
            this.dgvDriverDetails.Size = new System.Drawing.Size(896, 282);
            this.dgvDriverDetails.TabIndex = 4;
            this.dgvDriverDetails.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvDriverDetails.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvDriverDetails.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvDriverDetails.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvDriverDetails.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvDriverDetails.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvDriverDetails.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvDriverDetails.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvDriverDetails.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvDriverDetails.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.dgvDriverDetails.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvDriverDetails.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvDriverDetails.ThemeStyle.HeaderStyle.Height = 4;
            this.dgvDriverDetails.ThemeStyle.ReadOnly = false;
            this.dgvDriverDetails.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvDriverDetails.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvDriverDetails.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.dgvDriverDetails.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvDriverDetails.ThemeStyle.RowsStyle.Height = 28;
            this.dgvDriverDetails.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvDriverDetails.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // tabCustomers
            // 
            this.tabCustomers.Controls.Add(this.chartTopCustomers);
            this.tabCustomers.Controls.Add(this.dgvCustomerDetails);
            this.tabCustomers.Location = new System.Drawing.Point(184, 4);
            this.tabCustomers.Name = "tabCustomers";
            this.tabCustomers.Padding = new System.Windows.Forms.Padding(3);
            this.tabCustomers.Size = new System.Drawing.Size(946, 854);
            this.tabCustomers.TabIndex = 4;
            this.tabCustomers.Text = "Khách Hàng";
            this.tabCustomers.UseVisualStyleBackColor = true;
            // 
            // chartTopCustomers
            // 
            this.chartTopCustomers.Location = new System.Drawing.Point(3, 6);
            this.chartTopCustomers.Name = "chartTopCustomers";
            this.chartTopCustomers.Size = new System.Drawing.Size(937, 423);
            this.chartTopCustomers.TabIndex = 4;
            this.chartTopCustomers.Text = "cartesianChart3";
            // 
            // dgvCustomerDetails
            // 
            dataGridViewCellStyle88.BackColor = System.Drawing.Color.White;
            this.dgvCustomerDetails.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle88;
            this.dgvCustomerDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            dataGridViewCellStyle89.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle89.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle89.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            dataGridViewCellStyle89.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle89.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle89.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle89.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCustomerDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle89;
            this.dgvCustomerDetails.ColumnHeadersHeight = 4;
            this.dgvCustomerDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle90.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle90.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle90.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            dataGridViewCellStyle90.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle90.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle90.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle90.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCustomerDetails.DefaultCellStyle = dataGridViewCellStyle90;
            this.dgvCustomerDetails.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvCustomerDetails.Location = new System.Drawing.Point(23, 485);
            this.dgvCustomerDetails.Name = "dgvCustomerDetails";
            this.dgvCustomerDetails.RowHeadersVisible = false;
            this.dgvCustomerDetails.RowHeadersWidth = 62;
            this.dgvCustomerDetails.RowTemplate.Height = 28;
            this.dgvCustomerDetails.Size = new System.Drawing.Size(917, 282);
            this.dgvCustomerDetails.TabIndex = 3;
            this.dgvCustomerDetails.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvCustomerDetails.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvCustomerDetails.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvCustomerDetails.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvCustomerDetails.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvCustomerDetails.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvCustomerDetails.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvCustomerDetails.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvCustomerDetails.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvCustomerDetails.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.dgvCustomerDetails.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvCustomerDetails.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvCustomerDetails.ThemeStyle.HeaderStyle.Height = 4;
            this.dgvCustomerDetails.ThemeStyle.ReadOnly = false;
            this.dgvCustomerDetails.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvCustomerDetails.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvCustomerDetails.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.dgvCustomerDetails.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvCustomerDetails.ThemeStyle.RowsStyle.Height = 28;
            this.dgvCustomerDetails.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvCustomerDetails.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // tabOperations
            // 
            this.tabOperations.Controls.Add(this.chartTopRoutes);
            this.tabOperations.Controls.Add(this.pieShipmentStatus);
            this.tabOperations.Location = new System.Drawing.Point(184, 4);
            this.tabOperations.Name = "tabOperations";
            this.tabOperations.Padding = new System.Windows.Forms.Padding(3);
            this.tabOperations.Size = new System.Drawing.Size(946, 854);
            this.tabOperations.TabIndex = 2;
            this.tabOperations.Text = "Vận Hành";
            this.tabOperations.UseVisualStyleBackColor = true;
            // 
            // chartTopRoutes
            // 
            this.chartTopRoutes.Location = new System.Drawing.Point(6, 407);
            this.chartTopRoutes.Name = "chartTopRoutes";
            this.chartTopRoutes.Size = new System.Drawing.Size(934, 400);
            this.chartTopRoutes.TabIndex = 1;
            this.chartTopRoutes.Text = "cartesianChart2";
            // 
            // pieShipmentStatus
            // 
            this.pieShipmentStatus.Location = new System.Drawing.Point(6, 6);
            this.pieShipmentStatus.Name = "pieShipmentStatus";
            this.pieShipmentStatus.Size = new System.Drawing.Size(934, 400);
            this.pieShipmentStatus.TabIndex = 0;
            this.pieShipmentStatus.Text = "pieChart2";
            // 
            // tabRevenue
            // 
            this.tabRevenue.Controls.Add(this.chartRevenueOverTime);
            this.tabRevenue.Location = new System.Drawing.Point(184, 4);
            this.tabRevenue.Name = "tabRevenue";
            this.tabRevenue.Padding = new System.Windows.Forms.Padding(3);
            this.tabRevenue.Size = new System.Drawing.Size(946, 854);
            this.tabRevenue.TabIndex = 1;
            this.tabRevenue.Text = "Doanh Thu";
            this.tabRevenue.UseVisualStyleBackColor = true;
            // 
            // chartRevenueOverTime
            // 
            this.chartRevenueOverTime.Location = new System.Drawing.Point(6, 6);
            this.chartRevenueOverTime.Name = "chartRevenueOverTime";
            this.chartRevenueOverTime.Size = new System.Drawing.Size(921, 400);
            this.chartRevenueOverTime.TabIndex = 0;
            this.chartRevenueOverTime.Text = "cartesianChart1";
            // 
            // tabOverview
            // 
            this.tabOverview.Controls.Add(this.pieOrderStatus);
            this.tabOverview.Location = new System.Drawing.Point(184, 4);
            this.tabOverview.Name = "tabOverview";
            this.tabOverview.Padding = new System.Windows.Forms.Padding(3);
            this.tabOverview.Size = new System.Drawing.Size(946, 854);
            this.tabOverview.TabIndex = 0;
            this.tabOverview.Text = "Tổng Quan";
            this.tabOverview.UseVisualStyleBackColor = true;
            // 
            // btnExportOverview
            // 
            this.btnExportOverview.BorderRadius = 15;
            this.btnExportOverview.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnExportOverview.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnExportOverview.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnExportOverview.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnExportOverview.FillColor = System.Drawing.Color.Black;
            this.btnExportOverview.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnExportOverview.ForeColor = System.Drawing.Color.White;
            this.btnExportOverview.Location = new System.Drawing.Point(896, 43);
            this.btnExportOverview.Name = "btnExportOverview";
            this.btnExportOverview.Size = new System.Drawing.Size(122, 45);
            this.btnExportOverview.TabIndex = 2;
            this.btnExportOverview.Text = "Chi Tiết";
            // 
            // tabControlMain
            // 
            this.tabControlMain.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tabControlMain.Controls.Add(this.tabOverview);
            this.tabControlMain.Controls.Add(this.tabRevenue);
            this.tabControlMain.Controls.Add(this.tabOperations);
            this.tabControlMain.Controls.Add(this.tabCustomers);
            this.tabControlMain.Controls.Add(this.tabDrivers);
            this.tabControlMain.ItemSize = new System.Drawing.Size(180, 80);
            this.tabControlMain.Location = new System.Drawing.Point(30, 317);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(1134, 862);
            this.tabControlMain.TabButtonHoverState.BorderColor = System.Drawing.Color.Empty;
            this.tabControlMain.TabButtonHoverState.FillColor = System.Drawing.Color.LightGray;
            this.tabControlMain.TabButtonHoverState.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.tabControlMain.TabButtonHoverState.ForeColor = System.Drawing.Color.Black;
            this.tabControlMain.TabButtonHoverState.InnerColor = System.Drawing.Color.White;
            this.tabControlMain.TabButtonIdleState.BorderColor = System.Drawing.Color.Empty;
            this.tabControlMain.TabButtonIdleState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(33)))), ((int)(((byte)(36)))));
            this.tabControlMain.TabButtonIdleState.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.tabControlMain.TabButtonIdleState.ForeColor = System.Drawing.Color.White;
            this.tabControlMain.TabButtonIdleState.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(57)))));
            this.tabControlMain.TabButtonSelectedState.BorderColor = System.Drawing.Color.Transparent;
            this.tabControlMain.TabButtonSelectedState.FillColor = System.Drawing.Color.Black;
            this.tabControlMain.TabButtonSelectedState.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.tabControlMain.TabButtonSelectedState.ForeColor = System.Drawing.Color.White;
            this.tabControlMain.TabButtonSelectedState.InnerColor = System.Drawing.Color.White;
            this.tabControlMain.TabButtonSize = new System.Drawing.Size(180, 80);
            this.tabControlMain.TabButtonTextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.tabControlMain.TabIndex = 10;
            this.tabControlMain.TabMenuBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(33)))), ((int)(((byte)(36)))));
            // 
            // guna2Panel4
            // 
            this.guna2Panel4.BackColor = System.Drawing.Color.White;
            this.guna2Panel4.BorderColor = System.Drawing.Color.Black;
            this.guna2Panel4.BorderThickness = 1;
            this.guna2Panel4.Controls.Add(this.lblRevenueValue);
            this.guna2Panel4.Controls.Add(this.label9);
            this.guna2Panel4.Location = new System.Drawing.Point(870, 3);
            this.guna2Panel4.Name = "guna2Panel4";
            this.guna2Panel4.Size = new System.Drawing.Size(263, 106);
            this.guna2Panel4.TabIndex = 6;
            // 
            // lblCompletedValue
            // 
            this.lblCompletedValue.AutoSize = true;
            this.lblCompletedValue.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.lblCompletedValue.Location = new System.Drawing.Point(3, 34);
            this.lblCompletedValue.Name = "lblCompletedValue";
            this.lblCompletedValue.Size = new System.Drawing.Size(111, 54);
            this.lblCompletedValue.TabIndex = 2;
            this.lblCompletedValue.Text = "1000";
            // 
            // guna2GroupBox1
            // 
            this.guna2GroupBox1.BorderColor = System.Drawing.Color.Black;
            this.guna2GroupBox1.BorderRadius = 15;
            this.guna2GroupBox1.BorderThickness = 2;
            this.guna2GroupBox1.Controls.Add(this.btnExportOverview);
            this.guna2GroupBox1.Controls.Add(this.btnCustom);
            this.guna2GroupBox1.Controls.Add(this.btnYear);
            this.guna2GroupBox1.Controls.Add(this.btnQuarter);
            this.guna2GroupBox1.Controls.Add(this.label3);
            this.guna2GroupBox1.Controls.Add(this.btnMonth);
            this.guna2GroupBox1.Controls.Add(this.btnWeek);
            this.guna2GroupBox1.Controls.Add(this.btnToday);
            this.guna2GroupBox1.Controls.Add(this.dtpFrom);
            this.guna2GroupBox1.Controls.Add(this.dtpTo);
            this.guna2GroupBox1.Controls.Add(this.label2);
            this.guna2GroupBox1.CustomBorderColor = System.Drawing.Color.Black;
            this.guna2GroupBox1.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.guna2GroupBox1.ForeColor = System.Drawing.Color.White;
            this.guna2GroupBox1.Location = new System.Drawing.Point(27, 129);
            this.guna2GroupBox1.Name = "guna2GroupBox1";
            this.guna2GroupBox1.Size = new System.Drawing.Size(1140, 155);
            this.guna2GroupBox1.TabIndex = 5;
            this.guna2GroupBox1.Text = "Bộ Lọc";
            this.guna2GroupBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnCustom
            // 
            this.btnCustom.BorderRadius = 15;
            this.btnCustom.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCustom.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnCustom.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnCustom.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnCustom.FillColor = System.Drawing.Color.Black;
            this.btnCustom.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnCustom.ForeColor = System.Drawing.Color.White;
            this.btnCustom.Location = new System.Drawing.Point(767, 43);
            this.btnCustom.Name = "btnCustom";
            this.btnCustom.Size = new System.Drawing.Size(123, 45);
            this.btnCustom.TabIndex = 3;
            this.btnCustom.Text = "Tùy Chỉnh";
            // 
            // btnYear
            // 
            this.btnYear.BorderRadius = 15;
            this.btnYear.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnYear.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnYear.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnYear.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnYear.FillColor = System.Drawing.Color.Black;
            this.btnYear.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnYear.ForeColor = System.Drawing.Color.White;
            this.btnYear.Location = new System.Drawing.Point(638, 43);
            this.btnYear.Name = "btnYear";
            this.btnYear.Size = new System.Drawing.Size(123, 45);
            this.btnYear.TabIndex = 3;
            this.btnYear.Text = "Năm";
            // 
            // btnQuarter
            // 
            this.btnQuarter.BorderRadius = 15;
            this.btnQuarter.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnQuarter.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnQuarter.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnQuarter.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnQuarter.FillColor = System.Drawing.Color.Black;
            this.btnQuarter.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnQuarter.ForeColor = System.Drawing.Color.White;
            this.btnQuarter.Location = new System.Drawing.Point(509, 43);
            this.btnQuarter.Name = "btnQuarter";
            this.btnQuarter.Size = new System.Drawing.Size(123, 45);
            this.btnQuarter.TabIndex = 3;
            this.btnQuarter.Text = "Quý";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(571, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 25);
            this.label3.TabIndex = 2;
            this.label3.Text = "Đến Ngày: ";
            // 
            // btnMonth
            // 
            this.btnMonth.BorderRadius = 15;
            this.btnMonth.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnMonth.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnMonth.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnMonth.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnMonth.FillColor = System.Drawing.Color.Black;
            this.btnMonth.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnMonth.ForeColor = System.Drawing.Color.White;
            this.btnMonth.Location = new System.Drawing.Point(380, 43);
            this.btnMonth.Name = "btnMonth";
            this.btnMonth.Size = new System.Drawing.Size(123, 45);
            this.btnMonth.TabIndex = 3;
            this.btnMonth.Text = "Tháng";
            // 
            // dtpFrom
            // 
            this.dtpFrom.BorderRadius = 15;
            this.dtpFrom.Checked = true;
            this.dtpFrom.FillColor = System.Drawing.Color.Black;
            this.dtpFrom.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpFrom.ForeColor = System.Drawing.Color.White;
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(356, 94);
            this.dtpFrom.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpFrom.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(200, 36);
            this.dtpFrom.TabIndex = 1;
            this.dtpFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.dtpFrom.Value = new System.DateTime(2025, 10, 31, 16, 31, 29, 894);
            // 
            // btnWeek
            // 
            this.btnWeek.BorderRadius = 15;
            this.btnWeek.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnWeek.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnWeek.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnWeek.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnWeek.FillColor = System.Drawing.Color.Black;
            this.btnWeek.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnWeek.ForeColor = System.Drawing.Color.White;
            this.btnWeek.Location = new System.Drawing.Point(251, 43);
            this.btnWeek.Name = "btnWeek";
            this.btnWeek.Size = new System.Drawing.Size(123, 45);
            this.btnWeek.TabIndex = 3;
            this.btnWeek.Text = "Tuần";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(262, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "Từ Ngày: ";
            // 
            // btnToday
            // 
            this.btnToday.BorderRadius = 15;
            this.btnToday.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnToday.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnToday.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnToday.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnToday.FillColor = System.Drawing.Color.Black;
            this.btnToday.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnToday.ForeColor = System.Drawing.Color.White;
            this.btnToday.Location = new System.Drawing.Point(122, 43);
            this.btnToday.Name = "btnToday";
            this.btnToday.Size = new System.Drawing.Size(123, 45);
            this.btnToday.TabIndex = 3;
            this.btnToday.Text = "Hôm Nay";
            // 
            // dtpTo
            // 
            this.dtpTo.BorderRadius = 15;
            this.dtpTo.Checked = true;
            this.dtpTo.FillColor = System.Drawing.Color.Black;
            this.dtpTo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpTo.ForeColor = System.Drawing.Color.White;
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(679, 94);
            this.dtpTo.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpTo.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(200, 36);
            this.dtpTo.TabIndex = 1;
            this.dtpTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.dtpTo.Value = new System.DateTime(2025, 10, 31, 16, 30, 24, 261);
            // 
            // guna2Panel3
            // 
            this.guna2Panel3.BackColor = System.Drawing.Color.White;
            this.guna2Panel3.BorderColor = System.Drawing.Color.Black;
            this.guna2Panel3.BorderThickness = 1;
            this.guna2Panel3.Controls.Add(this.lblCompletedValue);
            this.guna2Panel3.Controls.Add(this.label7);
            this.guna2Panel3.Location = new System.Drawing.Point(601, 3);
            this.guna2Panel3.Name = "guna2Panel3";
            this.guna2Panel3.Size = new System.Drawing.Size(263, 106);
            this.guna2Panel3.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label7.Location = new System.Drawing.Point(26, 4);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(210, 25);
            this.label7.TabIndex = 2;
            this.label7.Text = "Giao Hàng Thành Công";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label1.Location = new System.Drawing.Point(58, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Tổng Đơn Hàng";
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.White;
            this.guna2Panel1.BorderColor = System.Drawing.Color.Black;
            this.guna2Panel1.BorderThickness = 1;
            this.guna2Panel1.Controls.Add(this.lblTotalOrdersValue);
            this.guna2Panel1.Controls.Add(this.label1);
            this.guna2Panel1.Location = new System.Drawing.Point(61, 3);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(263, 106);
            this.guna2Panel1.TabIndex = 8;
            // 
            // lblTotalOrdersValue
            // 
            this.lblTotalOrdersValue.AutoSize = true;
            this.lblTotalOrdersValue.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.lblTotalOrdersValue.Location = new System.Drawing.Point(3, 34);
            this.lblTotalOrdersValue.Name = "lblTotalOrdersValue";
            this.lblTotalOrdersValue.Size = new System.Drawing.Size(111, 54);
            this.lblTotalOrdersValue.TabIndex = 2;
            this.lblTotalOrdersValue.Text = "1000";
            // 
            // guna2Panel2
            // 
            this.guna2Panel2.BackColor = System.Drawing.Color.White;
            this.guna2Panel2.BorderColor = System.Drawing.Color.Black;
            this.guna2Panel2.BorderThickness = 1;
            this.guna2Panel2.Controls.Add(this.lblInProgressValue);
            this.guna2Panel2.Controls.Add(this.lblInProgressValue1);
            this.guna2Panel2.Location = new System.Drawing.Point(332, 3);
            this.guna2Panel2.Name = "guna2Panel2";
            this.guna2Panel2.Size = new System.Drawing.Size(263, 106);
            this.guna2Panel2.TabIndex = 9;
            // 
            // lblInProgressValue
            // 
            this.lblInProgressValue.AutoSize = true;
            this.lblInProgressValue.Font = new System.Drawing.Font("Segoe UI", 20F);
            this.lblInProgressValue.Location = new System.Drawing.Point(4, 34);
            this.lblInProgressValue.Name = "lblInProgressValue";
            this.lblInProgressValue.Size = new System.Drawing.Size(111, 54);
            this.lblInProgressValue.TabIndex = 2;
            this.lblInProgressValue.Text = "1000";
            // 
            // lblInProgressValue1
            // 
            this.lblInProgressValue1.AutoSize = true;
            this.lblInProgressValue1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblInProgressValue1.Location = new System.Drawing.Point(49, 4);
            this.lblInProgressValue1.Name = "lblInProgressValue1";
            this.lblInProgressValue1.Size = new System.Drawing.Size(164, 25);
            this.lblInProgressValue1.TabIndex = 2;
            this.lblInProgressValue1.Text = "Đang Vận Chuyển";
            // 
            // ucStatistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.guna2Panel4);
            this.Controls.Add(this.guna2GroupBox1);
            this.Controls.Add(this.guna2Panel3);
            this.Controls.Add(this.guna2Panel1);
            this.Controls.Add(this.guna2Panel2);
            this.Name = "ucStatistics";
            this.Size = new System.Drawing.Size(1195, 1261);
            this.tabDrivers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDriverDetails)).EndInit();
            this.tabCustomers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomerDetails)).EndInit();
            this.tabOperations.ResumeLayout(false);
            this.tabRevenue.ResumeLayout(false);
            this.tabOverview.ResumeLayout(false);
            this.tabControlMain.ResumeLayout(false);
            this.guna2Panel4.ResumeLayout(false);
            this.guna2Panel4.PerformLayout();
            this.guna2GroupBox1.ResumeLayout(false);
            this.guna2GroupBox1.PerformLayout();
            this.guna2Panel3.ResumeLayout(false);
            this.guna2Panel3.PerformLayout();
            this.guna2Panel1.ResumeLayout(false);
            this.guna2Panel1.PerformLayout();
            this.guna2Panel2.ResumeLayout(false);
            this.guna2Panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private LiveCharts.WinForms.PieChart pieOrderStatus;
        private System.Windows.Forms.Label lblRevenueValue;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TabPage tabDrivers;
        private LiveCharts.WinForms.CartesianChart chartTopDrivers;
        private Guna.UI2.WinForms.Guna2DataGridView dgvDriverDetails;
        private System.Windows.Forms.TabPage tabCustomers;
        private LiveCharts.WinForms.CartesianChart chartTopCustomers;
        private Guna.UI2.WinForms.Guna2DataGridView dgvCustomerDetails;
        private System.Windows.Forms.TabPage tabOperations;
        private LiveCharts.WinForms.CartesianChart chartTopRoutes;
        private LiveCharts.WinForms.PieChart pieShipmentStatus;
        private System.Windows.Forms.TabPage tabRevenue;
        private LiveCharts.WinForms.CartesianChart chartRevenueOverTime;
        private System.Windows.Forms.TabPage tabOverview;
        private Guna.UI2.WinForms.Guna2Button btnExportOverview;
        private Guna.UI2.WinForms.Guna2TabControl tabControlMain;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel4;
        private System.Windows.Forms.Label lblCompletedValue;
        private Guna.UI2.WinForms.Guna2GroupBox guna2GroupBox1;
        private Guna.UI2.WinForms.Guna2Button btnCustom;
        private Guna.UI2.WinForms.Guna2Button btnYear;
        private Guna.UI2.WinForms.Guna2Button btnQuarter;
        private System.Windows.Forms.Label label3;
        private Guna.UI2.WinForms.Guna2Button btnMonth;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpFrom;
        private Guna.UI2.WinForms.Guna2Button btnWeek;
        private System.Windows.Forms.Label label2;
        private Guna.UI2.WinForms.Guna2Button btnToday;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpTo;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private System.Windows.Forms.Label lblTotalOrdersValue;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
        private System.Windows.Forms.Label lblInProgressValue;
        private System.Windows.Forms.Label lblInProgressValue1;
    }
}
