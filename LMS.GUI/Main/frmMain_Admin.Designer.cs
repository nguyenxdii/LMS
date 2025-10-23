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
            this.pnlContent = new Guna.UI2.WinForms.Guna2Panel();
            this.btnHome = new Guna.UI2.WinForms.Guna2Button();
            this.btnVehicle = new Guna.UI2.WinForms.Guna2Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnAccount = new Guna.UI2.WinForms.Guna2Button();
            this.btnShipment = new Guna.UI2.WinForms.Guna2Button();
            this.btnReport = new Guna.UI2.WinForms.Guna2Button();
            this.btnWarehouse = new Guna.UI2.WinForms.Guna2Button();
            this.btnCustomer = new Guna.UI2.WinForms.Guna2Button();
            this.btnTracking = new Guna.UI2.WinForms.Guna2Button();
            this.btnOrder = new Guna.UI2.WinForms.Guna2Button();
            this.btnDriver = new Guna.UI2.WinForms.Guna2Button();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2PictureBox1 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.guna2PictureBox2 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.guna2Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlContent
            // 
            this.pnlContent.Location = new System.Drawing.Point(276, 120);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(1019, 640);
            this.pnlContent.TabIndex = 5;
            // 
            // btnHome
            // 
            this.btnHome.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnHome.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnHome.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnHome.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnHome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnHome.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(113)))), ((int)(((byte)(185)))));
            this.btnHome.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnHome.ForeColor = System.Drawing.Color.White;
            this.btnHome.Image = global::LMS.GUI.Properties.Resources.home;
            this.btnHome.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnHome.ImageSize = new System.Drawing.Size(40, 40);
            this.btnHome.Location = new System.Drawing.Point(3, 3);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(266, 65);
            this.btnHome.TabIndex = 0;
            this.btnHome.Text = "Trang Chủ";
            this.btnHome.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // btnVehicle
            // 
            this.btnVehicle.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnVehicle.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnVehicle.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnVehicle.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnVehicle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnVehicle.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(113)))), ((int)(((byte)(185)))));
            this.btnVehicle.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnVehicle.ForeColor = System.Drawing.Color.White;
            this.btnVehicle.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnVehicle.ImageSize = new System.Drawing.Size(40, 40);
            this.btnVehicle.Location = new System.Drawing.Point(3, 216);
            this.btnVehicle.Name = "btnVehicle";
            this.btnVehicle.Size = new System.Drawing.Size(266, 65);
            this.btnVehicle.TabIndex = 0;
            this.btnVehicle.Text = "Phương Tiện";
            this.btnVehicle.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.btnHome, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnVehicle, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnAccount, 0, 13);
            this.tableLayoutPanel1.Controls.Add(this.btnShipment, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnReport, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.btnWarehouse, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnCustomer, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnTracking, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.btnOrder, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.btnDriver, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(-2, 120);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 14;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(272, 719);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // btnAccount
            // 
            this.btnAccount.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnAccount.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnAccount.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnAccount.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnAccount.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(113)))), ((int)(((byte)(185)))));
            this.btnAccount.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnAccount.ForeColor = System.Drawing.Color.White;
            this.btnAccount.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnAccount.ImageSize = new System.Drawing.Size(40, 40);
            this.btnAccount.Location = new System.Drawing.Point(3, 643);
            this.btnAccount.Name = "btnAccount";
            this.btnAccount.Size = new System.Drawing.Size(266, 65);
            this.btnAccount.TabIndex = 0;
            this.btnAccount.Text = "Tài Khoản";
            this.btnAccount.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnAccount.Click += new System.EventHandler(this.btnAccount_Click);
            // 
            // btnShipment
            // 
            this.btnShipment.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnShipment.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnShipment.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnShipment.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnShipment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnShipment.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(113)))), ((int)(((byte)(185)))));
            this.btnShipment.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnShipment.ForeColor = System.Drawing.Color.White;
            this.btnShipment.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnShipment.ImageSize = new System.Drawing.Size(40, 40);
            this.btnShipment.Location = new System.Drawing.Point(3, 358);
            this.btnShipment.Name = "btnShipment";
            this.btnShipment.Size = new System.Drawing.Size(266, 66);
            this.btnShipment.TabIndex = 0;
            this.btnShipment.Text = "Chuyến Hàng";
            this.btnShipment.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // btnReport
            // 
            this.btnReport.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnReport.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnReport.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnReport.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnReport.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(113)))), ((int)(((byte)(185)))));
            this.btnReport.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnReport.ForeColor = System.Drawing.Color.White;
            this.btnReport.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnReport.ImageSize = new System.Drawing.Size(40, 40);
            this.btnReport.Location = new System.Drawing.Point(3, 572);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(266, 65);
            this.btnReport.TabIndex = 0;
            this.btnReport.Text = "Báo Cáo";
            this.btnReport.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // btnWarehouse
            // 
            this.btnWarehouse.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnWarehouse.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnWarehouse.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnWarehouse.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnWarehouse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnWarehouse.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(113)))), ((int)(((byte)(185)))));
            this.btnWarehouse.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnWarehouse.ForeColor = System.Drawing.Color.White;
            this.btnWarehouse.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnWarehouse.ImageSize = new System.Drawing.Size(40, 40);
            this.btnWarehouse.Location = new System.Drawing.Point(3, 287);
            this.btnWarehouse.Name = "btnWarehouse";
            this.btnWarehouse.Size = new System.Drawing.Size(266, 65);
            this.btnWarehouse.TabIndex = 0;
            this.btnWarehouse.Text = "Kho Hàng";
            this.btnWarehouse.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // btnCustomer
            // 
            this.btnCustomer.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCustomer.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnCustomer.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnCustomer.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnCustomer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCustomer.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(113)))), ((int)(((byte)(185)))));
            this.btnCustomer.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnCustomer.ForeColor = System.Drawing.Color.White;
            this.btnCustomer.Image = global::LMS.GUI.Properties.Resources.customer;
            this.btnCustomer.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnCustomer.ImageSize = new System.Drawing.Size(40, 40);
            this.btnCustomer.Location = new System.Drawing.Point(3, 74);
            this.btnCustomer.Name = "btnCustomer";
            this.btnCustomer.Size = new System.Drawing.Size(266, 65);
            this.btnCustomer.TabIndex = 0;
            this.btnCustomer.Text = "Khách Hàng";
            this.btnCustomer.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // btnTracking
            // 
            this.btnTracking.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnTracking.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnTracking.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnTracking.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnTracking.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTracking.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(113)))), ((int)(((byte)(185)))));
            this.btnTracking.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnTracking.ForeColor = System.Drawing.Color.White;
            this.btnTracking.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnTracking.ImageSize = new System.Drawing.Size(40, 40);
            this.btnTracking.Location = new System.Drawing.Point(3, 501);
            this.btnTracking.Name = "btnTracking";
            this.btnTracking.Size = new System.Drawing.Size(266, 65);
            this.btnTracking.TabIndex = 0;
            this.btnTracking.Text = "Theo Dõi Đơn Hàng";
            this.btnTracking.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // btnOrder
            // 
            this.btnOrder.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnOrder.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnOrder.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnOrder.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOrder.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(113)))), ((int)(((byte)(185)))));
            this.btnOrder.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnOrder.ForeColor = System.Drawing.Color.White;
            this.btnOrder.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnOrder.ImageSize = new System.Drawing.Size(40, 40);
            this.btnOrder.Location = new System.Drawing.Point(3, 430);
            this.btnOrder.Name = "btnOrder";
            this.btnOrder.Size = new System.Drawing.Size(266, 65);
            this.btnOrder.TabIndex = 0;
            this.btnOrder.Text = "Đơn Hàng";
            this.btnOrder.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // btnDriver
            // 
            this.btnDriver.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnDriver.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnDriver.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnDriver.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnDriver.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDriver.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(113)))), ((int)(((byte)(185)))));
            this.btnDriver.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnDriver.ForeColor = System.Drawing.Color.White;
            this.btnDriver.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnDriver.ImageSize = new System.Drawing.Size(40, 40);
            this.btnDriver.Location = new System.Drawing.Point(3, 145);
            this.btnDriver.Name = "btnDriver";
            this.btnDriver.Size = new System.Drawing.Size(266, 65);
            this.btnDriver.TabIndex = 0;
            this.btnDriver.Text = "Tài xế";
            this.btnDriver.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Location = new System.Drawing.Point(928, 40);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(51, 20);
            this.lblWelcome.TabIndex = 4;
            this.lblWelcome.Text = "label2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(69, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(295, 29);
            this.label1.TabIndex = 3;
            this.label1.Text = "LMS - Admin Dashboard";
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(113)))), ((int)(((byte)(185)))));
            this.guna2Panel1.Controls.Add(this.lblWelcome);
            this.guna2Panel1.Controls.Add(this.label1);
            this.guna2Panel1.Controls.Add(this.guna2PictureBox1);
            this.guna2Panel1.Controls.Add(this.guna2PictureBox2);
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2Panel1.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(1293, 100);
            this.guna2Panel1.TabIndex = 3;
            // 
            // guna2PictureBox1
            // 
            this.guna2PictureBox1.ImageRotate = 0F;
            this.guna2PictureBox1.Location = new System.Drawing.Point(12, 12);
            this.guna2PictureBox1.Name = "guna2PictureBox1";
            this.guna2PictureBox1.Size = new System.Drawing.Size(51, 69);
            this.guna2PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.guna2PictureBox1.TabIndex = 2;
            this.guna2PictureBox1.TabStop = false;
            this.guna2PictureBox1.Tag = "";
            // 
            // guna2PictureBox2
            // 
            this.guna2PictureBox2.ImageRotate = 0F;
            this.guna2PictureBox2.Location = new System.Drawing.Point(871, 12);
            this.guna2PictureBox2.Name = "guna2PictureBox2";
            this.guna2PictureBox2.Size = new System.Drawing.Size(51, 69);
            this.guna2PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.guna2PictureBox2.TabIndex = 2;
            this.guna2PictureBox2.TabStop = false;
            this.guna2PictureBox2.Tag = "";
            // 
            // frmMain_Admin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1293, 852);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.guna2Panel1);
            this.Name = "frmMain_Admin";
            this.Text = "frmMain_Admin";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.guna2Panel1.ResumeLayout(false);
            this.guna2Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel pnlContent;
        private Guna.UI2.WinForms.Guna2Button btnHome;
        private Guna.UI2.WinForms.Guna2Button btnVehicle;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Guna.UI2.WinForms.Guna2Button btnAccount;
        private Guna.UI2.WinForms.Guna2Button btnShipment;
        private Guna.UI2.WinForms.Guna2Button btnReport;
        private Guna.UI2.WinForms.Guna2Button btnWarehouse;
        private Guna.UI2.WinForms.Guna2Button btnCustomer;
        private Guna.UI2.WinForms.Guna2Button btnTracking;
        private Guna.UI2.WinForms.Guna2Button btnOrder;
        private Guna.UI2.WinForms.Guna2Button btnDriver;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox1;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox2;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
    }
}