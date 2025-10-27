namespace LMS.GUI.Main
{
    partial class frmMain_Customer
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
            this.pnlTopBar = new CSharp.Winform.UI.Panel();
            this.btnHam = new Guna.UI2.WinForms.Guna2PictureBox();
            this.guna2ControlBox3 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.guna2ControlBox2 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.guna2ControlBox1 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.tsllblWelcome = new System.Windows.Forms.ToolStripStatusLabel();
            this.stsMain = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.sidebar = new System.Windows.Forms.FlowLayoutPanel();
            this.btnHome = new CSharp.Winform.UI.Button();
            this.menuContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.btnMenu = new CSharp.Winform.UI.Button();
            this.btnNewOrder = new CSharp.Winform.UI.Button();
            this.btnOrderList = new CSharp.Winform.UI.Button();
            this.btnViewTracking = new CSharp.Winform.UI.Button();
            this.btnAccount = new CSharp.Winform.UI.Button();
            this.btnLogOut = new CSharp.Winform.UI.Button();
            this.menuTransition = new System.Windows.Forms.Timer(this.components);
            this.sidebarTransition = new System.Windows.Forms.Timer(this.components);
            this.pnlContent = new Guna.UI2.WinForms.Guna2Panel();
            this.task = new CSharp.Winform.UI.Panel();
            this.lblPageTitle = new CSharp.Winform.UI.Label();
            this.pnlTopBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnHam)).BeginInit();
            this.stsMain.SuspendLayout();
            this.sidebar.SuspendLayout();
            this.menuContainer.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.task.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTopBar
            // 
            this.pnlTopBar.BackColor = System.Drawing.Color.White;
            this.pnlTopBar.Controls.Add(this.btnHam);
            this.pnlTopBar.Controls.Add(this.guna2ControlBox3);
            this.pnlTopBar.Controls.Add(this.lblTitle);
            this.pnlTopBar.Controls.Add(this.guna2ControlBox2);
            this.pnlTopBar.Controls.Add(this.guna2ControlBox1);
            this.pnlTopBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopBar.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.pnlTopBar.Location = new System.Drawing.Point(0, 0);
            this.pnlTopBar.Name = "pnlTopBar";
            this.pnlTopBar.Size = new System.Drawing.Size(1500, 67);
            this.pnlTopBar.TabIndex = 9;
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
            this.lblTitle.Size = new System.Drawing.Size(271, 36);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "Customer Dashboard";
            this.lblTitle.Click += new System.EventHandler(this.lblTitle_Click);
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
            // tsllblWelcome
            // 
            this.tsllblWelcome.Name = "tsllblWelcome";
            this.tsllblWelcome.Size = new System.Drawing.Size(179, 25);
            this.tsllblWelcome.Text = "toolStripStatusLabel1";
            // 
            // stsMain
            // 
            this.stsMain.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.stsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsllblWelcome,
            this.toolStripStatusLabel1});
            this.stsMain.Location = new System.Drawing.Point(0, 657);
            this.stsMain.Name = "stsMain";
            this.stsMain.Size = new System.Drawing.Size(1199, 32);
            this.stsMain.TabIndex = 0;
            this.stsMain.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(179, 25);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
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
            this.sidebar.TabIndex = 10;
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
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // menuContainer
            // 
            this.menuContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(33)))), ((int)(((byte)(36)))));
            this.menuContainer.Controls.Add(this.btnMenu);
            this.menuContainer.Controls.Add(this.btnNewOrder);
            this.menuContainer.Controls.Add(this.btnOrderList);
            this.menuContainer.Controls.Add(this.btnViewTracking);
            this.menuContainer.Location = new System.Drawing.Point(3, 61);
            this.menuContainer.Name = "menuContainer";
            this.menuContainer.Size = new System.Drawing.Size(301, 58);
            this.menuContainer.TabIndex = 5;
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
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // btnNewOrder
            // 
            this.btnNewOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(33)))), ((int)(((byte)(36)))));
            this.btnNewOrder.FlatAppearance.BorderSize = 0;
            this.btnNewOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewOrder.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNewOrder.ForeColor = System.Drawing.Color.White;
            this.btnNewOrder.Image = global::LMS.GUI.Properties.Resources.dot_mini;
            this.btnNewOrder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNewOrder.Location = new System.Drawing.Point(0, 58);
            this.btnNewOrder.Margin = new System.Windows.Forms.Padding(0);
            this.btnNewOrder.Name = "btnNewOrder";
            this.btnNewOrder.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnNewOrder.Size = new System.Drawing.Size(301, 58);
            this.btnNewOrder.TabIndex = 10;
            this.btnNewOrder.Text = "            Tạo Đơn Hàng";
            this.btnNewOrder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNewOrder.UseVisualStyleBackColor = false;
            // 
            // btnOrderList
            // 
            this.btnOrderList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(33)))), ((int)(((byte)(36)))));
            this.btnOrderList.FlatAppearance.BorderSize = 0;
            this.btnOrderList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOrderList.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOrderList.ForeColor = System.Drawing.Color.White;
            this.btnOrderList.Image = global::LMS.GUI.Properties.Resources.dot_mini;
            this.btnOrderList.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOrderList.Location = new System.Drawing.Point(0, 116);
            this.btnOrderList.Margin = new System.Windows.Forms.Padding(0);
            this.btnOrderList.Name = "btnOrderList";
            this.btnOrderList.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnOrderList.Size = new System.Drawing.Size(301, 58);
            this.btnOrderList.TabIndex = 11;
            this.btnOrderList.Text = "            Danh Sách Đơn Hàng";
            this.btnOrderList.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOrderList.UseVisualStyleBackColor = false;
            // 
            // btnViewTracking
            // 
            this.btnViewTracking.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(33)))), ((int)(((byte)(36)))));
            this.btnViewTracking.FlatAppearance.BorderSize = 0;
            this.btnViewTracking.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewTracking.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewTracking.ForeColor = System.Drawing.Color.White;
            this.btnViewTracking.Image = global::LMS.GUI.Properties.Resources.dot_mini;
            this.btnViewTracking.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnViewTracking.Location = new System.Drawing.Point(0, 174);
            this.btnViewTracking.Margin = new System.Windows.Forms.Padding(0);
            this.btnViewTracking.Name = "btnViewTracking";
            this.btnViewTracking.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnViewTracking.Size = new System.Drawing.Size(301, 58);
            this.btnViewTracking.TabIndex = 12;
            this.btnViewTracking.Text = "            Tra Cứu";
            this.btnViewTracking.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnViewTracking.UseVisualStyleBackColor = false;
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
            this.btnLogOut.Click += new System.EventHandler(this.btnLogOut_Click);
            // 
            // menuTransition
            // 
            this.menuTransition.Interval = 10;
            this.menuTransition.Tick += new System.EventHandler(this.menuTransition_Tick);
            // 
            // sidebarTransition
            // 
            this.sidebarTransition.Interval = 10;
            this.sidebarTransition.Tick += new System.EventHandler(this.sidebarTransition_Tick);
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.stsMain);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(301, 111);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(1199, 689);
            this.pnlContent.TabIndex = 11;
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
            this.task.TabIndex = 1;
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
            // frmMain_Customer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1500, 800);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.task);
            this.Controls.Add(this.sidebar);
            this.Controls.Add(this.pnlTopBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmMain_Customer";
            this.Text = "frmMain_Customer";
            this.pnlTopBar.ResumeLayout(false);
            this.pnlTopBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnHam)).EndInit();
            this.stsMain.ResumeLayout(false);
            this.stsMain.PerformLayout();
            this.sidebar.ResumeLayout(false);
            this.menuContainer.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            this.task.ResumeLayout(false);
            this.task.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private CSharp.Winform.UI.Panel pnlTopBar;
        private Guna.UI2.WinForms.Guna2PictureBox btnHam;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox3;
        private System.Windows.Forms.Label lblTitle;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox2;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox1;
        private System.Windows.Forms.ToolStripStatusLabel tsllblWelcome;
        private System.Windows.Forms.StatusStrip stsMain;
        private System.Windows.Forms.FlowLayoutPanel sidebar;
        private CSharp.Winform.UI.Button btnHome;
        private System.Windows.Forms.FlowLayoutPanel menuContainer;
        private CSharp.Winform.UI.Button btnNewOrder;
        private CSharp.Winform.UI.Button btnOrderList;
        private CSharp.Winform.UI.Button btnViewTracking;
        private CSharp.Winform.UI.Button btnAccount;
        private CSharp.Winform.UI.Button btnLogOut;
        private System.Windows.Forms.Timer menuTransition;
        private System.Windows.Forms.Timer sidebarTransition;
        private Guna.UI2.WinForms.Guna2Panel pnlContent;
        private CSharp.Winform.UI.Button btnMenu;
        private CSharp.Winform.UI.Panel task;
        private CSharp.Winform.UI.Label lblPageTitle;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}