namespace LMS.GUI.Main
{
    partial class frmMain_Admin
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
            this.components = new System.ComponentModel.Container();
            this.btnHam = new Guna.UI2.WinForms.Guna2PictureBox();
            this.btnMenu = new CSharp.Winform.UI.Button();
            this.btnCustomer = new CSharp.Winform.UI.Button();
            this.btnDriver = new CSharp.Winform.UI.Button();
            this.btnVehicle = new CSharp.Winform.UI.Button();
            this.btnWarehouse = new CSharp.Winform.UI.Button();
            this.btnShipment = new CSharp.Winform.UI.Button();
            this.btnOrder = new CSharp.Winform.UI.Button();
            this.btnReport = new CSharp.Winform.UI.Button();
            this.btnRouteTemplate = new CSharp.Winform.UI.Button();
            this.btnHome = new CSharp.Winform.UI.Button();
            this.menuContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.btnLogOut = new CSharp.Winform.UI.Button();
            this.sidebar = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAccount = new CSharp.Winform.UI.Button();
            this.guna2ControlBox3 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.guna2ControlBox2 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.guna2ControlBox1 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.pnlTop = new CSharp.Winform.UI.Panel();
            this.tsslblUserInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.stsMain = new System.Windows.Forms.StatusStrip();
            this.task = new CSharp.Winform.UI.Panel();
            this.lblPageTitle = new CSharp.Winform.UI.Label();
            this.pnlContent = new Guna.UI2.WinForms.Guna2Panel();
            this.menuTransition = new System.Windows.Forms.Timer(this.components);
            this.sidebarTransition = new System.Windows.Forms.Timer(this.components);
            this.timerClock = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.btnHam)).BeginInit();
            this.menuContainer.SuspendLayout();
            this.sidebar.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.stsMain.SuspendLayout();
            this.task.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnHam
            // 
            this.btnHam.Image = global::LMS.GUI.Properties.Resources.menu_;
            this.btnHam.ImageRotate = 0F;
            this.btnHam.InitialImage = global::LMS.GUI.Properties.Resources.menu_;
            this.btnHam.Location = new System.Drawing.Point(15, 8);
            this.btnHam.Name = "btnHam";
            this.btnHam.Size = new System.Drawing.Size(48, 51);
            this.btnHam.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnHam.TabIndex = 2;
            this.btnHam.TabStop = false;
            this.btnHam.Click += new System.EventHandler(this.btnHam_Click);
            // 
            // btnMenu
            // 
            this.btnMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(24)))), ((int)(((byte)(29)))));
            this.btnMenu.FlatAppearance.BorderSize = 0;
            this.btnMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMenu.ForeColor = System.Drawing.Color.White;
            this.btnMenu.Image = global::LMS.GUI.Properties.Resources.toolmini;
            this.btnMenu.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMenu.Location = new System.Drawing.Point(0, 0);
            this.btnMenu.Margin = new System.Windows.Forms.Padding(0);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnMenu.Size = new System.Drawing.Size(301, 58);
            this.btnMenu.TabIndex = 2;
            this.btnMenu.Text = "       Chức Năng";
            this.btnMenu.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnMenu.UseVisualStyleBackColor = false;
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // btnCustomer
            // 
            this.btnCustomer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(33)))), ((int)(((byte)(36)))));
            this.btnCustomer.FlatAppearance.BorderSize = 0;
            this.btnCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCustomer.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustomer.ForeColor = System.Drawing.Color.White;
            this.btnCustomer.Image = global::LMS.GUI.Properties.Resources.dot_mini;
            this.btnCustomer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCustomer.Location = new System.Drawing.Point(0, 58);
            this.btnCustomer.Margin = new System.Windows.Forms.Padding(0);
            this.btnCustomer.Name = "btnCustomer";
            this.btnCustomer.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnCustomer.Size = new System.Drawing.Size(301, 58);
            this.btnCustomer.TabIndex = 10;
            this.btnCustomer.Text = "            Khách Hàng";
            this.btnCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCustomer.UseVisualStyleBackColor = false;
            // 
            // btnDriver
            // 
            this.btnDriver.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(33)))), ((int)(((byte)(36)))));
            this.btnDriver.FlatAppearance.BorderSize = 0;
            this.btnDriver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDriver.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDriver.ForeColor = System.Drawing.Color.White;
            this.btnDriver.Image = global::LMS.GUI.Properties.Resources.dot_mini;
            this.btnDriver.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDriver.Location = new System.Drawing.Point(0, 116);
            this.btnDriver.Margin = new System.Windows.Forms.Padding(0);
            this.btnDriver.Name = "btnDriver";
            this.btnDriver.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnDriver.Size = new System.Drawing.Size(301, 58);
            this.btnDriver.TabIndex = 11;
            this.btnDriver.Text = "            Tài Xế";
            this.btnDriver.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDriver.UseVisualStyleBackColor = false;
            // 
            // btnVehicle
            // 
            this.btnVehicle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(33)))), ((int)(((byte)(36)))));
            this.btnVehicle.FlatAppearance.BorderSize = 0;
            this.btnVehicle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVehicle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVehicle.ForeColor = System.Drawing.Color.White;
            this.btnVehicle.Image = global::LMS.GUI.Properties.Resources.dot_mini;
            this.btnVehicle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnVehicle.Location = new System.Drawing.Point(0, 174);
            this.btnVehicle.Margin = new System.Windows.Forms.Padding(0);
            this.btnVehicle.Name = "btnVehicle";
            this.btnVehicle.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnVehicle.Size = new System.Drawing.Size(301, 58);
            this.btnVehicle.TabIndex = 12;
            this.btnVehicle.Text = "            Quản Lý Phương Tiện";
            this.btnVehicle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnVehicle.UseVisualStyleBackColor = false;
            // 
            // btnWarehouse
            // 
            this.btnWarehouse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(33)))), ((int)(((byte)(36)))));
            this.btnWarehouse.FlatAppearance.BorderSize = 0;
            this.btnWarehouse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWarehouse.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWarehouse.ForeColor = System.Drawing.Color.White;
            this.btnWarehouse.Image = global::LMS.GUI.Properties.Resources.dot_mini;
            this.btnWarehouse.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnWarehouse.Location = new System.Drawing.Point(0, 232);
            this.btnWarehouse.Margin = new System.Windows.Forms.Padding(0);
            this.btnWarehouse.Name = "btnWarehouse";
            this.btnWarehouse.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnWarehouse.Size = new System.Drawing.Size(301, 58);
            this.btnWarehouse.TabIndex = 13;
            this.btnWarehouse.Text = "            Quản Lý Kho";
            this.btnWarehouse.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnWarehouse.UseVisualStyleBackColor = false;
            // 
            // btnShipment
            // 
            this.btnShipment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(33)))), ((int)(((byte)(36)))));
            this.btnShipment.FlatAppearance.BorderSize = 0;
            this.btnShipment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShipment.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShipment.ForeColor = System.Drawing.Color.White;
            this.btnShipment.Image = global::LMS.GUI.Properties.Resources.dot_mini;
            this.btnShipment.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnShipment.Location = new System.Drawing.Point(0, 290);
            this.btnShipment.Margin = new System.Windows.Forms.Padding(0);
            this.btnShipment.Name = "btnShipment";
            this.btnShipment.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnShipment.Size = new System.Drawing.Size(301, 58);
            this.btnShipment.TabIndex = 14;
            this.btnShipment.Text = "            Quản Lý Chuyến Hàng";
            this.btnShipment.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnShipment.UseVisualStyleBackColor = false;
            // 
            // btnOrder
            // 
            this.btnOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(33)))), ((int)(((byte)(36)))));
            this.btnOrder.FlatAppearance.BorderSize = 0;
            this.btnOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOrder.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOrder.ForeColor = System.Drawing.Color.White;
            this.btnOrder.Image = global::LMS.GUI.Properties.Resources.dot_mini;
            this.btnOrder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOrder.Location = new System.Drawing.Point(0, 348);
            this.btnOrder.Margin = new System.Windows.Forms.Padding(0);
            this.btnOrder.Name = "btnOrder";
            this.btnOrder.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnOrder.Size = new System.Drawing.Size(301, 58);
            this.btnOrder.TabIndex = 15;
            this.btnOrder.Text = "            Đơn Hàng";
            this.btnOrder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOrder.UseVisualStyleBackColor = false;
            // 
            // btnReport
            // 
            this.btnReport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(33)))), ((int)(((byte)(36)))));
            this.btnReport.FlatAppearance.BorderSize = 0;
            this.btnReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReport.ForeColor = System.Drawing.Color.White;
            this.btnReport.Image = global::LMS.GUI.Properties.Resources.dot_mini;
            this.btnReport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReport.Location = new System.Drawing.Point(0, 406);
            this.btnReport.Margin = new System.Windows.Forms.Padding(0);
            this.btnReport.Name = "btnReport";
            this.btnReport.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnReport.Size = new System.Drawing.Size(301, 58);
            this.btnReport.TabIndex = 17;
            this.btnReport.Text = "            Thống Kê";
            this.btnReport.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReport.UseVisualStyleBackColor = false;
            // 
            // btnRouteTemplate
            // 
            this.btnRouteTemplate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(33)))), ((int)(((byte)(36)))));
            this.btnRouteTemplate.FlatAppearance.BorderSize = 0;
            this.btnRouteTemplate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRouteTemplate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRouteTemplate.ForeColor = System.Drawing.Color.White;
            this.btnRouteTemplate.Image = global::LMS.GUI.Properties.Resources.dot_mini;
            this.btnRouteTemplate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRouteTemplate.Location = new System.Drawing.Point(0, 464);
            this.btnRouteTemplate.Margin = new System.Windows.Forms.Padding(0);
            this.btnRouteTemplate.Name = "btnRouteTemplate";
            this.btnRouteTemplate.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnRouteTemplate.Size = new System.Drawing.Size(301, 58);
            this.btnRouteTemplate.TabIndex = 18;
            this.btnRouteTemplate.Text = "            Quản Lý Tuyến Đường";
            this.btnRouteTemplate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRouteTemplate.UseVisualStyleBackColor = false;
            // 
            // btnHome
            // 
            this.btnHome.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(24)))), ((int)(((byte)(29)))));
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHome.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHome.ForeColor = System.Drawing.Color.White;
            this.btnHome.Image = global::LMS.GUI.Properties.Resources.dashboard_1;
            this.btnHome.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHome.Location = new System.Drawing.Point(0, 0);
            this.btnHome.Margin = new System.Windows.Forms.Padding(0);
            this.btnHome.Name = "btnHome";
            this.btnHome.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnHome.Size = new System.Drawing.Size(301, 58);
            this.btnHome.TabIndex = 6;
            this.btnHome.Text = "       Trang Chủ";
            this.btnHome.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnHome.UseVisualStyleBackColor = false;
            // 
            // menuContainer
            // 
            this.menuContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(33)))), ((int)(((byte)(36)))));
            this.menuContainer.Controls.Add(this.btnMenu);
            this.menuContainer.Controls.Add(this.btnCustomer);
            this.menuContainer.Controls.Add(this.btnDriver);
            this.menuContainer.Controls.Add(this.btnVehicle);
            this.menuContainer.Controls.Add(this.btnWarehouse);
            this.menuContainer.Controls.Add(this.btnShipment);
            this.menuContainer.Controls.Add(this.btnOrder);
            this.menuContainer.Controls.Add(this.btnReport);
            this.menuContainer.Controls.Add(this.btnRouteTemplate);
            this.menuContainer.Location = new System.Drawing.Point(3, 61);
            this.menuContainer.Name = "menuContainer";
            this.menuContainer.Size = new System.Drawing.Size(301, 58);
            this.menuContainer.TabIndex = 9;
            // 
            // btnLogOut
            // 
            this.btnLogOut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(24)))), ((int)(((byte)(29)))));
            this.btnLogOut.FlatAppearance.BorderSize = 0;
            this.btnLogOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogOut.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogOut.ForeColor = System.Drawing.Color.White;
            this.btnLogOut.Image = global::LMS.GUI.Properties.Resources.logout_;
            this.btnLogOut.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogOut.Location = new System.Drawing.Point(0, 180);
            this.btnLogOut.Margin = new System.Windows.Forms.Padding(0);
            this.btnLogOut.Name = "btnLogOut";
            this.btnLogOut.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnLogOut.Size = new System.Drawing.Size(301, 58);
            this.btnLogOut.TabIndex = 8;
            this.btnLogOut.Text = "       Đăng Xuất";
            this.btnLogOut.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLogOut.UseVisualStyleBackColor = false;
            // 
            // sidebar
            // 
            this.sidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(24)))), ((int)(((byte)(29)))));
            this.sidebar.Controls.Add(this.btnHome);
            this.sidebar.Controls.Add(this.menuContainer);
            this.sidebar.Controls.Add(this.btnAccount);
            this.sidebar.Controls.Add(this.btnLogOut);
            this.sidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidebar.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.sidebar.Location = new System.Drawing.Point(2, 69);
            this.sidebar.Name = "sidebar";
            this.sidebar.Size = new System.Drawing.Size(301, 747);
            this.sidebar.TabIndex = 16;
            // 
            // btnAccount
            // 
            this.btnAccount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(24)))), ((int)(((byte)(29)))));
            this.btnAccount.FlatAppearance.BorderSize = 0;
            this.btnAccount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAccount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAccount.ForeColor = System.Drawing.Color.White;
            this.btnAccount.Image = global::LMS.GUI.Properties.Resources.account_;
            this.btnAccount.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAccount.Location = new System.Drawing.Point(0, 122);
            this.btnAccount.Margin = new System.Windows.Forms.Padding(0);
            this.btnAccount.Name = "btnAccount";
            this.btnAccount.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnAccount.Size = new System.Drawing.Size(301, 58);
            this.btnAccount.TabIndex = 7;
            this.btnAccount.Text = "       Tài Khoản";
            this.btnAccount.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAccount.UseVisualStyleBackColor = false;
            // 
            // guna2ControlBox3
            // 
            this.guna2ControlBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2ControlBox3.BorderRadius = 5;
            this.guna2ControlBox3.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(24)))), ((int)(((byte)(29)))));
            this.guna2ControlBox3.IconColor = System.Drawing.Color.White;
            this.guna2ControlBox3.Location = new System.Drawing.Point(1423, 12);
            this.guna2ControlBox3.Name = "guna2ControlBox3";
            this.guna2ControlBox3.Size = new System.Drawing.Size(61, 39);
            this.guna2ControlBox3.TabIndex = 4;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(69, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(235, 36);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "Admin Dashboard";
            this.lblTitle.Click += new System.EventHandler(this.lblTitle_Click);
            // 
            // guna2ControlBox2
            // 
            this.guna2ControlBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2ControlBox2.BorderRadius = 5;
            this.guna2ControlBox2.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MaximizeBox;
            this.guna2ControlBox2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(24)))), ((int)(((byte)(29)))));
            this.guna2ControlBox2.IconColor = System.Drawing.Color.White;
            this.guna2ControlBox2.Location = new System.Drawing.Point(1356, 11);
            this.guna2ControlBox2.Name = "guna2ControlBox2";
            this.guna2ControlBox2.Size = new System.Drawing.Size(61, 39);
            this.guna2ControlBox2.TabIndex = 3;
            // 
            // guna2ControlBox1
            // 
            this.guna2ControlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2ControlBox1.BorderRadius = 5;
            this.guna2ControlBox1.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MinimizeBox;
            this.guna2ControlBox1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(24)))), ((int)(((byte)(29)))));
            this.guna2ControlBox1.IconColor = System.Drawing.Color.White;
            this.guna2ControlBox1.Location = new System.Drawing.Point(1289, 11);
            this.guna2ControlBox1.Name = "guna2ControlBox1";
            this.guna2ControlBox1.Size = new System.Drawing.Size(61, 39);
            this.guna2ControlBox1.TabIndex = 2;
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.Controls.Add(this.btnHam);
            this.pnlTop.Controls.Add(this.guna2ControlBox3);
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Controls.Add(this.guna2ControlBox2);
            this.pnlTop.Controls.Add(this.guna2ControlBox1);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.pnlTop.Location = new System.Drawing.Point(2, 2);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1496, 67);
            this.pnlTop.TabIndex = 15;
            // 
            // tsslblUserInfo
            // 
            this.tsslblUserInfo.Name = "tsslblUserInfo";
            this.tsslblUserInfo.Size = new System.Drawing.Size(120, 25);
            this.tsslblUserInfo.Text = "tsslblUserInfo";
            // 
            // stsMain
            // 
            this.stsMain.BackColor = System.Drawing.SystemColors.Control;
            this.stsMain.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.stsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslblUserInfo});
            this.stsMain.Location = new System.Drawing.Point(2, 816);
            this.stsMain.Name = "stsMain";
            this.stsMain.Size = new System.Drawing.Size(1496, 32);
            this.stsMain.TabIndex = 14;
            this.stsMain.Text = "statusStrip1";
            // 
            // task
            // 
            this.task.BackColor = System.Drawing.Color.LightGray;
            this.task.Controls.Add(this.lblPageTitle);
            this.task.Dock = System.Windows.Forms.DockStyle.Top;
            this.task.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.task.Location = new System.Drawing.Point(303, 69);
            this.task.Name = "task";
            this.task.Size = new System.Drawing.Size(1195, 44);
            this.task.TabIndex = 17;
            // 
            // lblPageTitle
            // 
            this.lblPageTitle.AutoSize = true;
            this.lblPageTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblPageTitle.Location = new System.Drawing.Point(7, 11);
            this.lblPageTitle.Name = "lblPageTitle";
            this.lblPageTitle.Size = new System.Drawing.Size(59, 25);
            this.lblPageTitle.TabIndex = 1;
            this.lblPageTitle.Text = "label1";
            // 
            // pnlContent
            // 
            this.pnlContent.BackColor = System.Drawing.SystemColors.Control;
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(303, 113);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(1195, 703);
            this.pnlContent.TabIndex = 18;
            // 
            // menuTransition
            // 
            this.menuTransition.Interval = 5;
            this.menuTransition.Tick += new System.EventHandler(this.menuTransition_Tick);
            // 
            // sidebarTransition
            // 
            this.sidebarTransition.Interval = 5;
            this.sidebarTransition.Tick += new System.EventHandler(this.sidebarTransition_Tick);
            // 
            // timerClock
            // 
            this.timerClock.Enabled = true;
            this.timerClock.Interval = 1000;
            this.timerClock.Tick += new System.EventHandler(this.timerClock_Tick);
            // 
            // frmMain_Admin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1500, 850);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.task);
            this.Controls.Add(this.sidebar);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.stsMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmMain_Admin";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Text = "frmMain_Admin1";
            ((System.ComponentModel.ISupportInitialize)(this.btnHam)).EndInit();
            this.menuContainer.ResumeLayout(false);
            this.sidebar.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.stsMain.ResumeLayout(false);
            this.stsMain.PerformLayout();
            this.task.ResumeLayout(false);
            this.task.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2PictureBox btnHam;
        private CSharp.Winform.UI.Button btnMenu;
        private CSharp.Winform.UI.Button btnCustomer;
        private CSharp.Winform.UI.Button btnDriver;
        private CSharp.Winform.UI.Button btnVehicle;
        private CSharp.Winform.UI.Button btnWarehouse;
        private CSharp.Winform.UI.Button btnShipment;
        private CSharp.Winform.UI.Button btnOrder;
        private CSharp.Winform.UI.Button btnReport;
        private CSharp.Winform.UI.Button btnRouteTemplate;
        private CSharp.Winform.UI.Button btnHome;
        private System.Windows.Forms.FlowLayoutPanel menuContainer;
        private CSharp.Winform.UI.Button btnLogOut;
        private System.Windows.Forms.FlowLayoutPanel sidebar;
        private CSharp.Winform.UI.Button btnAccount;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox3;
        private System.Windows.Forms.Label lblTitle;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox2;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox1;
        private CSharp.Winform.UI.Panel pnlTop;
        private System.Windows.Forms.ToolStripStatusLabel tsslblUserInfo;
        private System.Windows.Forms.StatusStrip stsMain;
        private CSharp.Winform.UI.Panel task;
        private CSharp.Winform.UI.Label lblPageTitle;
        private Guna.UI2.WinForms.Guna2Panel pnlContent;
        private System.Windows.Forms.Timer menuTransition;
        private System.Windows.Forms.Timer sidebarTransition;
        private System.Windows.Forms.Timer timerClock;
    }
}