namespace LMS.GUI.Main
{
    partial class frmMain_Driver
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
            this.lblPageTitle = new CSharp.Winform.UI.Label();
            this.sidebarTransition = new System.Windows.Forms.Timer(this.components);
            this.menuTransition = new System.Windows.Forms.Timer(this.components);
            this.btnMenu = new CSharp.Winform.UI.Button();
            this.btnMyShipments = new CSharp.Winform.UI.Button();
            this.btnShipmentDetail = new CSharp.Winform.UI.Button();
            this.btnShipmentRun = new CSharp.Winform.UI.Button();
            this.task = new CSharp.Winform.UI.Panel();
            this.sidebar = new System.Windows.Forms.FlowLayoutPanel();
            this.btnHome = new CSharp.Winform.UI.Button();
            this.menuContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAccount = new CSharp.Winform.UI.Button();
            this.btnLogOut = new CSharp.Winform.UI.Button();
            this.tsllblWelcome = new System.Windows.Forms.ToolStripStatusLabel();
            this.guna2ControlBox3 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.guna2ControlBox2 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.guna2ControlBox1 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.btnHam = new Guna.UI2.WinForms.Guna2PictureBox();
            this.pnlTop = new CSharp.Winform.UI.Panel();
            this.stsMain = new System.Windows.Forms.StatusStrip();
            this.pnlContent = new Guna.UI2.WinForms.Guna2Panel();
            this.task.SuspendLayout();
            this.sidebar.SuspendLayout();
            this.menuContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnHam)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblPageTitle
            // 
            this.lblPageTitle.AutoSize = true;
            this.lblPageTitle.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.lblPageTitle.Location = new System.Drawing.Point(7, 11);
            this.lblPageTitle.Name = "lblPageTitle";
            this.lblPageTitle.Size = new System.Drawing.Size(54, 21);
            this.lblPageTitle.TabIndex = 1;
            this.lblPageTitle.Text = "label1";
            // 
            // sidebarTransition
            // 
            this.sidebarTransition.Interval = 10;
            this.sidebarTransition.Tick += new System.EventHandler(this.sidebarTransition_Tick);
            // 
            // menuTransition
            // 
            this.menuTransition.Interval = 10;
            this.menuTransition.Tick += new System.EventHandler(this.menuTransition_Tick);
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
            this.btnMenu.Text = "       Công Cụ";
            this.btnMenu.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnMenu.UseVisualStyleBackColor = false;
            // 
            // btnMyShipments
            // 
            this.btnMyShipments.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(33)))), ((int)(((byte)(36)))));
            this.btnMyShipments.FlatAppearance.BorderSize = 0;
            this.btnMyShipments.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMyShipments.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMyShipments.ForeColor = System.Drawing.Color.White;
            this.btnMyShipments.Image = global::LMS.GUI.Properties.Resources.dot_mini;
            this.btnMyShipments.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMyShipments.Location = new System.Drawing.Point(0, 58);
            this.btnMyShipments.Margin = new System.Windows.Forms.Padding(0);
            this.btnMyShipments.Name = "btnMyShipments";
            this.btnMyShipments.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnMyShipments.Size = new System.Drawing.Size(301, 58);
            this.btnMyShipments.TabIndex = 10;
            this.btnMyShipments.Text = "            Đơn Hàng Của Tôi";
            this.btnMyShipments.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMyShipments.UseVisualStyleBackColor = false;
            // 
            // btnShipmentDetail
            // 
            this.btnShipmentDetail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(33)))), ((int)(((byte)(36)))));
            this.btnShipmentDetail.FlatAppearance.BorderSize = 0;
            this.btnShipmentDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShipmentDetail.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShipmentDetail.ForeColor = System.Drawing.Color.White;
            this.btnShipmentDetail.Image = global::LMS.GUI.Properties.Resources.dot_mini;
            this.btnShipmentDetail.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnShipmentDetail.Location = new System.Drawing.Point(0, 116);
            this.btnShipmentDetail.Margin = new System.Windows.Forms.Padding(0);
            this.btnShipmentDetail.Name = "btnShipmentDetail";
            this.btnShipmentDetail.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnShipmentDetail.Size = new System.Drawing.Size(301, 58);
            this.btnShipmentDetail.TabIndex = 11;
            this.btnShipmentDetail.Text = "            Chi Tiết Chuyến Hàng";
            this.btnShipmentDetail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnShipmentDetail.UseVisualStyleBackColor = false;
            // 
            // btnShipmentRun
            // 
            this.btnShipmentRun.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(33)))), ((int)(((byte)(36)))));
            this.btnShipmentRun.FlatAppearance.BorderSize = 0;
            this.btnShipmentRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShipmentRun.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShipmentRun.ForeColor = System.Drawing.Color.White;
            this.btnShipmentRun.Image = global::LMS.GUI.Properties.Resources.dot_mini;
            this.btnShipmentRun.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnShipmentRun.Location = new System.Drawing.Point(0, 174);
            this.btnShipmentRun.Margin = new System.Windows.Forms.Padding(0);
            this.btnShipmentRun.Name = "btnShipmentRun";
            this.btnShipmentRun.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnShipmentRun.Size = new System.Drawing.Size(301, 58);
            this.btnShipmentRun.TabIndex = 12;
            this.btnShipmentRun.Text = "            Chuyến Hàng Hiện Tại";
            this.btnShipmentRun.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnShipmentRun.UseVisualStyleBackColor = false;
            // 
            // task
            // 
            this.task.BackColor = System.Drawing.Color.LightGray;
            this.task.Controls.Add(this.lblPageTitle);
            this.task.Dock = System.Windows.Forms.DockStyle.Top;
            this.task.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.task.Location = new System.Drawing.Point(301, 67);
            this.task.Name = "task";
            this.task.Size = new System.Drawing.Size(1199, 44);
            this.task.TabIndex = 12;
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
            this.sidebar.Location = new System.Drawing.Point(0, 67);
            this.sidebar.Name = "sidebar";
            this.sidebar.Size = new System.Drawing.Size(301, 733);
            this.sidebar.TabIndex = 14;
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
            this.menuContainer.Controls.Add(this.btnMyShipments);
            this.menuContainer.Controls.Add(this.btnShipmentDetail);
            this.menuContainer.Controls.Add(this.btnShipmentRun);
            this.menuContainer.Location = new System.Drawing.Point(3, 61);
            this.menuContainer.Name = "menuContainer";
            this.menuContainer.Size = new System.Drawing.Size(301, 58);
            this.menuContainer.TabIndex = 5;
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
            this.btnAccount.Click += new System.EventHandler(this.btnAccount_Click);
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
            //this.btnLogOut.Click += new System.EventHandler(this.btnLogOut_Click_1);
            // 
            // tsllblWelcome
            // 
            this.tsllblWelcome.Margin = new System.Windows.Forms.Padding(0, 3, 0, 2);
            this.tsllblWelcome.Name = "tsllblWelcome";
            this.tsllblWelcome.Size = new System.Drawing.Size(179, 25);
            this.tsllblWelcome.Text = "toolStripStatusLabel1";
            // 
            // guna2ControlBox3
            // 
            this.guna2ControlBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2ControlBox3.BorderRadius = 5;
            this.guna2ControlBox3.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(24)))), ((int)(((byte)(29)))));
            this.guna2ControlBox3.IconColor = System.Drawing.Color.White;
            this.guna2ControlBox3.Location = new System.Drawing.Point(1427, 12);
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
            this.lblTitle.Size = new System.Drawing.Size(228, 36);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "Driver Dashboard";
            // 
            // guna2ControlBox2
            // 
            this.guna2ControlBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2ControlBox2.BorderRadius = 5;
            this.guna2ControlBox2.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MaximizeBox;
            this.guna2ControlBox2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(24)))), ((int)(((byte)(29)))));
            this.guna2ControlBox2.IconColor = System.Drawing.Color.White;
            this.guna2ControlBox2.Location = new System.Drawing.Point(1360, 11);
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
            this.guna2ControlBox1.Location = new System.Drawing.Point(1293, 11);
            this.guna2ControlBox1.Name = "guna2ControlBox1";
            this.guna2ControlBox1.Size = new System.Drawing.Size(61, 39);
            this.guna2ControlBox1.TabIndex = 2;
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
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1500, 67);
            this.pnlTop.TabIndex = 13;
            // 
            // stsMain
            // 
            this.stsMain.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.stsMain.Location = new System.Drawing.Point(0, 667);
            this.stsMain.Name = "stsMain";
            this.stsMain.Size = new System.Drawing.Size(1199, 22);
            this.stsMain.TabIndex = 0;
            this.stsMain.Text = "statusStrip1";
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.stsMain);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(301, 111);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(1199, 689);
            this.pnlContent.TabIndex = 15;
            // 
            // frmMain_Driver
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1500, 800);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.task);
            this.Controls.Add(this.sidebar);
            this.Controls.Add(this.pnlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmMain_Driver";
            this.Text = "frmMain_Driver";
            this.task.ResumeLayout(false);
            this.task.PerformLayout();
            this.sidebar.ResumeLayout(false);
            this.menuContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnHam)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private CSharp.Winform.UI.Label lblPageTitle;
        private System.Windows.Forms.Timer sidebarTransition;
        private System.Windows.Forms.Timer menuTransition;
        private CSharp.Winform.UI.Button btnMenu;
        private CSharp.Winform.UI.Button btnMyShipments;
        private CSharp.Winform.UI.Button btnShipmentDetail;
        private CSharp.Winform.UI.Button btnShipmentRun;
        private CSharp.Winform.UI.Panel task;
        private System.Windows.Forms.FlowLayoutPanel sidebar;
        private CSharp.Winform.UI.Button btnHome;
        private System.Windows.Forms.FlowLayoutPanel menuContainer;
        private CSharp.Winform.UI.Button btnAccount;
        private CSharp.Winform.UI.Button btnLogOut;
        private System.Windows.Forms.ToolStripStatusLabel tsllblWelcome;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox3;
        private System.Windows.Forms.Label lblTitle;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox2;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox1;
        private Guna.UI2.WinForms.Guna2PictureBox btnHam;
        private CSharp.Winform.UI.Panel pnlTop;
        private System.Windows.Forms.StatusStrip stsMain;
        private Guna.UI2.WinForms.Guna2Panel pnlContent;
    }
}