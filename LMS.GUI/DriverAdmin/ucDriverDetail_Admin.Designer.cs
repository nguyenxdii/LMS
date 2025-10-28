namespace LMS.GUI.DriverAdmin
{
    partial class ucDriverDetail_Admin
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new CSharp.Winform.UI.Panel();
            this.panel2 = new CSharp.Winform.UI.Panel();
            this.lblTitle = new CSharp.Winform.UI.Label();
            this.guna2ControlBox3 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.dgvDriverShipments = new Guna.UI2.WinForms.Guna2DataGridView();
            this.groupBox1 = new CSharp.Winform.UI.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lblUsername = new CSharp.Winform.UI.Label();
            this.lblPassword = new CSharp.Winform.UI.Label();
            this.lblAccountStatus = new CSharp.Winform.UI.Label();
            this.grpInfo = new CSharp.Winform.UI.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblFullName = new CSharp.Winform.UI.Label();
            this.lblLicenseType = new CSharp.Winform.UI.Label();
            this.lblCitizenId = new CSharp.Winform.UI.Label();
            this.lblPhone = new CSharp.Winform.UI.Label();
            this.lblVehiclePlate = new CSharp.Winform.UI.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDriverShipments)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.grpInfo.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.dgvDriverShipments);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.grpInfo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1130, 685);
            this.panel1.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.lblTitle);
            this.panel2.Controls.Add(this.guna2ControlBox3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1130, 58);
            this.panel2.TabIndex = 13;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.lblTitle.Location = new System.Drawing.Point(13, 18);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(54, 21);
            this.lblTitle.TabIndex = 85;
            this.lblTitle.Text = "label4";
            // 
            // guna2ControlBox3
            // 
            this.guna2ControlBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2ControlBox3.BorderRadius = 5;
            this.guna2ControlBox3.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(24)))), ((int)(((byte)(29)))));
            this.guna2ControlBox3.IconColor = System.Drawing.Color.White;
            this.guna2ControlBox3.Location = new System.Drawing.Point(1061, 8);
            this.guna2ControlBox3.Name = "guna2ControlBox3";
            this.guna2ControlBox3.Size = new System.Drawing.Size(61, 39);
            this.guna2ControlBox3.TabIndex = 4;
            // 
            // dgvDriverShipments
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            this.dgvDriverShipments.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvDriverShipments.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Tahoma", 8.25F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDriverShipments.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvDriverShipments.ColumnHeadersHeight = 4;
            this.dgvDriverShipments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Tahoma", 8.25F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDriverShipments.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvDriverShipments.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvDriverShipments.Location = new System.Drawing.Point(27, 311);
            this.dgvDriverShipments.Name = "dgvDriverShipments";
            this.dgvDriverShipments.RowHeadersVisible = false;
            this.dgvDriverShipments.RowHeadersWidth = 62;
            this.dgvDriverShipments.RowTemplate.Height = 28;
            this.dgvDriverShipments.Size = new System.Drawing.Size(1140, 343);
            this.dgvDriverShipments.TabIndex = 12;
            this.dgvDriverShipments.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvDriverShipments.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvDriverShipments.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvDriverShipments.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvDriverShipments.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvDriverShipments.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvDriverShipments.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvDriverShipments.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvDriverShipments.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvDriverShipments.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.dgvDriverShipments.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvDriverShipments.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvDriverShipments.ThemeStyle.HeaderStyle.Height = 4;
            this.dgvDriverShipments.ThemeStyle.ReadOnly = false;
            this.dgvDriverShipments.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvDriverShipments.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvDriverShipments.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.dgvDriverShipments.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvDriverShipments.ThemeStyle.RowsStyle.Height = 28;
            this.dgvDriverShipments.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvDriverShipments.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel2);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.groupBox1.Location = new System.Drawing.Point(595, 83);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(583, 199);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Thông tin Tài khoản";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.lblVehiclePlate, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.lblUsername, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblPassword, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblAccountStatus, 0, 2);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(33, 51);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(523, 133);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblUsername.Location = new System.Drawing.Point(3, 0);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(111, 30);
            this.lblUsername.TabIndex = 2;
            this.lblUsername.Text = "Username";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblPassword.Location = new System.Drawing.Point(3, 30);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(126, 30);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "lblPassword";
            // 
            // lblAccountStatus
            // 
            this.lblAccountStatus.AutoSize = true;
            this.lblAccountStatus.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblAccountStatus.Location = new System.Drawing.Point(3, 60);
            this.lblAccountStatus.Name = "lblAccountStatus";
            this.lblAccountStatus.Size = new System.Drawing.Size(148, 30);
            this.lblAccountStatus.TabIndex = 2;
            this.lblAccountStatus.Text = "AccountStatus";
            // 
            // grpInfo
            // 
            this.grpInfo.Controls.Add(this.tableLayoutPanel1);
            this.grpInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.grpInfo.Location = new System.Drawing.Point(17, 83);
            this.grpInfo.Name = "grpInfo";
            this.grpInfo.Size = new System.Drawing.Size(572, 199);
            this.grpInfo.TabIndex = 10;
            this.grpInfo.TabStop = false;
            this.grpInfo.Text = "Thông tin Chung";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.lblFullName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblLicenseType, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblCitizenId, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblPhone, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(33, 51);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(523, 133);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // lblFullName
            // 
            this.lblFullName.AutoSize = true;
            this.lblFullName.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblFullName.Location = new System.Drawing.Point(3, 0);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(82, 30);
            this.lblFullName.TabIndex = 2;
            this.lblFullName.Text = "Họ Tên";
            // 
            // lblLicenseType
            // 
            this.lblLicenseType.AutoSize = true;
            this.lblLicenseType.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblLicenseType.Location = new System.Drawing.Point(3, 90);
            this.lblLicenseType.Name = "lblLicenseType";
            this.lblLicenseType.Size = new System.Drawing.Size(94, 30);
            this.lblLicenseType.TabIndex = 2;
            this.lblLicenseType.Text = "Bằng Lái";
            // 
            // lblCitizenId
            // 
            this.lblCitizenId.AutoSize = true;
            this.lblCitizenId.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblCitizenId.Location = new System.Drawing.Point(3, 30);
            this.lblCitizenId.Name = "lblCitizenId";
            this.lblCitizenId.Size = new System.Drawing.Size(68, 30);
            this.lblCitizenId.TabIndex = 2;
            this.lblCitizenId.Text = "CCCD";
            // 
            // lblPhone
            // 
            this.lblPhone.AutoSize = true;
            this.lblPhone.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblPhone.Location = new System.Drawing.Point(3, 60);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(52, 30);
            this.lblPhone.TabIndex = 2;
            this.lblPhone.Text = "SĐT";
            // 
            // lblVehiclePlate
            // 
            this.lblVehiclePlate.AutoSize = true;
            this.lblVehiclePlate.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblVehiclePlate.Location = new System.Drawing.Point(3, 90);
            this.lblVehiclePlate.Name = "lblVehiclePlate";
            this.lblVehiclePlate.Size = new System.Drawing.Size(152, 30);
            this.lblVehiclePlate.TabIndex = 3;
            this.lblVehiclePlate.Text = "lblVehiclePlate";
            // 
            // ucDriverDetail_Admin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.panel1);
            this.Name = "ucDriverDetail_Admin";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Size = new System.Drawing.Size(1134, 689);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDriverShipments)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.grpInfo.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private CSharp.Winform.UI.Panel panel1;
        private Guna.UI2.WinForms.Guna2DataGridView dgvDriverShipments;
        private CSharp.Winform.UI.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private CSharp.Winform.UI.Label lblUsername;
        private CSharp.Winform.UI.Label lblPassword;
        private CSharp.Winform.UI.Label lblAccountStatus;
        private CSharp.Winform.UI.GroupBox grpInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private CSharp.Winform.UI.Label lblFullName;
        private CSharp.Winform.UI.Label lblLicenseType;
        private CSharp.Winform.UI.Label lblCitizenId;
        private CSharp.Winform.UI.Label lblPhone;
        private CSharp.Winform.UI.Panel panel2;
        private CSharp.Winform.UI.Label lblTitle;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox3;
        private CSharp.Winform.UI.Label lblVehiclePlate;
    }
}
