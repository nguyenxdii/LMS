namespace LMS.GUI.OrderCustomer
{
    partial class ucOrderCreate_Cus
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
            this.label9 = new CSharp.Winform.UI.Label();
            this.label7 = new CSharp.Winform.UI.Label();
            this.btnClear = new Guna.UI2.WinForms.Guna2Button();
            this.btnNext = new Guna.UI2.WinForms.Guna2Button();
            this.dtpDesiredTime = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.txtPackageDescription = new Guna.UI2.WinForms.Guna2TextBox();
            this.label8 = new CSharp.Winform.UI.Label();
            this.lblEstimatedFee = new System.Windows.Forms.Label();
            this.txtPickupAddress = new Guna.UI2.WinForms.Guna2TextBox();
            this.chkPickupAtSender = new System.Windows.Forms.CheckBox();
            this.cmbDestWarehouse = new Guna.UI2.WinForms.Guna2ComboBox();
            this.cmbDestZone = new Guna.UI2.WinForms.Guna2ComboBox();
            this.cmbOriginWarehouse = new Guna.UI2.WinForms.Guna2ComboBox();
            this.cmbOriginZone = new Guna.UI2.WinForms.Guna2ComboBox();
            this.label3 = new CSharp.Winform.UI.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new CSharp.Winform.UI.Label();
            this.label5 = new CSharp.Winform.UI.Label();
            this.label4 = new CSharp.Winform.UI.Label();
            this.label2 = new CSharp.Winform.UI.Label();
            this.label1 = new CSharp.Winform.UI.Label();
            this.btnCalcFee = new Guna.UI2.WinForms.Guna2Button();
            this.panel1 = new CSharp.Winform.UI.Panel();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label9.Location = new System.Drawing.Point(96, 406);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 25);
            this.label9.TabIndex = 8;
            this.label9.Text = "Tổng Phí";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label7.Location = new System.Drawing.Point(105, 307);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 25);
            this.label7.TabIndex = 6;
            this.label7.Text = "Ghi Chú";
            // 
            // btnClear
            // 
            this.btnClear.BorderRadius = 15;
            this.btnClear.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnClear.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnClear.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnClear.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnClear.FillColor = System.Drawing.Color.Black;
            this.btnClear.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(750, 162);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(219, 45);
            this.btnClear.TabIndex = 0;
            this.btnClear.Text = "Tải lại";
            // 
            // btnNext
            // 
            this.btnNext.BorderRadius = 15;
            this.btnNext.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnNext.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnNext.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnNext.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnNext.FillColor = System.Drawing.Color.Black;
            this.btnNext.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnNext.ForeColor = System.Drawing.Color.White;
            this.btnNext.Location = new System.Drawing.Point(750, 93);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(219, 45);
            this.btnNext.TabIndex = 3;
            this.btnNext.Text = "Xác nhận - Thanh toán";
            // 
            // dtpDesiredTime
            // 
            this.dtpDesiredTime.BorderRadius = 15;
            this.dtpDesiredTime.Checked = true;
            this.dtpDesiredTime.CustomFormat = "";
            this.dtpDesiredTime.FillColor = System.Drawing.Color.Black;
            this.dtpDesiredTime.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpDesiredTime.ForeColor = System.Drawing.Color.White;
            this.dtpDesiredTime.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDesiredTime.Location = new System.Drawing.Point(188, 350);
            this.dtpDesiredTime.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpDesiredTime.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpDesiredTime.Name = "dtpDesiredTime";
            this.dtpDesiredTime.Size = new System.Drawing.Size(290, 36);
            this.dtpDesiredTime.TabIndex = 17;
            this.dtpDesiredTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.dtpDesiredTime.Value = new System.DateTime(2025, 10, 22, 11, 50, 17, 627);
            // 
            // txtPackageDescription
            // 
            this.txtPackageDescription.BorderRadius = 15;
            this.txtPackageDescription.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPackageDescription.DefaultText = "";
            this.txtPackageDescription.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtPackageDescription.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtPackageDescription.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtPackageDescription.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtPackageDescription.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtPackageDescription.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtPackageDescription.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtPackageDescription.Location = new System.Drawing.Point(188, 303);
            this.txtPackageDescription.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPackageDescription.Name = "txtPackageDescription";
            this.txtPackageDescription.PlaceholderText = "";
            this.txtPackageDescription.SelectedText = "";
            this.txtPackageDescription.Size = new System.Drawing.Size(288, 36);
            this.txtPackageDescription.TabIndex = 16;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label8.Location = new System.Drawing.Point(45, 354);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(134, 25);
            this.label8.TabIndex = 7;
            this.label8.Text = "Ngày Gửi Hàng";
            // 
            // lblEstimatedFee
            // 
            this.lblEstimatedFee.AutoSize = true;
            this.lblEstimatedFee.Location = new System.Drawing.Point(182, 406);
            this.lblEstimatedFee.Name = "lblEstimatedFee";
            this.lblEstimatedFee.Size = new System.Drawing.Size(63, 25);
            this.lblEstimatedFee.TabIndex = 9;
            this.lblEstimatedFee.Text = "label1";
            // 
            // txtPickupAddress
            // 
            this.txtPickupAddress.BorderRadius = 15;
            this.txtPickupAddress.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPickupAddress.DefaultText = "";
            this.txtPickupAddress.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtPickupAddress.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtPickupAddress.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtPickupAddress.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtPickupAddress.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtPickupAddress.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtPickupAddress.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtPickupAddress.Location = new System.Drawing.Point(187, 255);
            this.txtPickupAddress.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPickupAddress.Name = "txtPickupAddress";
            this.txtPickupAddress.PlaceholderText = "";
            this.txtPickupAddress.SelectedText = "";
            this.txtPickupAddress.Size = new System.Drawing.Size(288, 36);
            this.txtPickupAddress.TabIndex = 15;
            // 
            // chkPickupAtSender
            // 
            this.chkPickupAtSender.AutoSize = true;
            this.chkPickupAtSender.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.chkPickupAtSender.Location = new System.Drawing.Point(187, 217);
            this.chkPickupAtSender.Name = "chkPickupAtSender";
            this.chkPickupAtSender.Size = new System.Drawing.Size(168, 29);
            this.chkPickupAtSender.TabIndex = 14;
            this.chkPickupAtSender.Text = "Phí 100.000VND";
            this.chkPickupAtSender.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkPickupAtSender.UseVisualStyleBackColor = true;
            // 
            // cmbDestWarehouse
            // 
            this.cmbDestWarehouse.BackColor = System.Drawing.Color.Transparent;
            this.cmbDestWarehouse.BorderRadius = 15;
            this.cmbDestWarehouse.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbDestWarehouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDestWarehouse.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbDestWarehouse.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbDestWarehouse.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbDestWarehouse.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbDestWarehouse.ItemHeight = 30;
            this.cmbDestWarehouse.Location = new System.Drawing.Point(187, 166);
            this.cmbDestWarehouse.Name = "cmbDestWarehouse";
            this.cmbDestWarehouse.Size = new System.Drawing.Size(290, 36);
            this.cmbDestWarehouse.TabIndex = 13;
            // 
            // cmbDestZone
            // 
            this.cmbDestZone.BackColor = System.Drawing.Color.Transparent;
            this.cmbDestZone.BorderRadius = 15;
            this.cmbDestZone.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbDestZone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDestZone.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbDestZone.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbDestZone.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbDestZone.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbDestZone.ItemHeight = 30;
            this.cmbDestZone.Location = new System.Drawing.Point(187, 121);
            this.cmbDestZone.Name = "cmbDestZone";
            this.cmbDestZone.Size = new System.Drawing.Size(290, 36);
            this.cmbDestZone.TabIndex = 12;
            // 
            // cmbOriginWarehouse
            // 
            this.cmbOriginWarehouse.BackColor = System.Drawing.Color.Transparent;
            this.cmbOriginWarehouse.BorderRadius = 15;
            this.cmbOriginWarehouse.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbOriginWarehouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOriginWarehouse.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbOriginWarehouse.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbOriginWarehouse.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbOriginWarehouse.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbOriginWarehouse.ItemHeight = 30;
            this.cmbOriginWarehouse.Location = new System.Drawing.Point(187, 75);
            this.cmbOriginWarehouse.Name = "cmbOriginWarehouse";
            this.cmbOriginWarehouse.Size = new System.Drawing.Size(290, 36);
            this.cmbOriginWarehouse.TabIndex = 11;
            // 
            // cmbOriginZone
            // 
            this.cmbOriginZone.BackColor = System.Drawing.Color.Transparent;
            this.cmbOriginZone.BorderRadius = 15;
            this.cmbOriginZone.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbOriginZone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOriginZone.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbOriginZone.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbOriginZone.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbOriginZone.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbOriginZone.ItemHeight = 30;
            this.cmbOriginZone.Location = new System.Drawing.Point(187, 29);
            this.cmbOriginZone.Name = "cmbOriginZone";
            this.cmbOriginZone.Size = new System.Drawing.Size(290, 36);
            this.cmbOriginZone.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label3.Location = new System.Drawing.Point(23, 125);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(156, 25);
            this.label3.TabIndex = 2;
            this.label3.Text = "K. Vực Nhận Hàng";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.dtpDesiredTime);
            this.groupBox1.Controls.Add(this.txtPackageDescription);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.lblEstimatedFee);
            this.groupBox1.Controls.Add(this.txtPickupAddress);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.chkPickupAtSender);
            this.groupBox1.Controls.Add(this.cmbDestWarehouse);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cmbDestZone);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cmbOriginWarehouse);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cmbOriginZone);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.groupBox1.Location = new System.Drawing.Point(229, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(502, 460);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Thông Tin Đơn Hàng";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label6.Location = new System.Drawing.Point(32, 259);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(147, 25);
            this.label6.TabIndex = 5;
            this.label6.Text = "Địa Chỉ Lấy Hàng";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label5.Location = new System.Drawing.Point(27, 217);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(152, 25);
            this.label5.TabIndex = 4;
            this.label5.Text = "Lấy Hàng Tận Nơi";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label4.Location = new System.Drawing.Point(41, 170);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(138, 25);
            this.label4.TabIndex = 3;
            this.label4.Text = "Kho Nhận Hàng";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label2.Location = new System.Drawing.Point(56, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "Kho Gửi Hàng";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label1.Location = new System.Drawing.Point(38, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "K. Vực Gửi Hàng";
            // 
            // btnCalcFee
            // 
            this.btnCalcFee.BorderRadius = 15;
            this.btnCalcFee.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCalcFee.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnCalcFee.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnCalcFee.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnCalcFee.FillColor = System.Drawing.Color.Black;
            this.btnCalcFee.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnCalcFee.ForeColor = System.Drawing.Color.White;
            this.btnCalcFee.Location = new System.Drawing.Point(750, 22);
            this.btnCalcFee.Name = "btnCalcFee";
            this.btnCalcFee.Size = new System.Drawing.Size(219, 45);
            this.btnCalcFee.TabIndex = 2;
            this.btnCalcFee.Text = "Tính tiền";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Controls.Add(this.btnCalcFee);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1199, 689);
            this.panel1.TabIndex = 0;
            // 
            // ucOrderCreate_Cus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "ucOrderCreate_Cus";
            this.Size = new System.Drawing.Size(1199, 689);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CSharp.Winform.UI.Label label9;
        private CSharp.Winform.UI.Label label7;
        private Guna.UI2.WinForms.Guna2Button btnClear;
        private Guna.UI2.WinForms.Guna2Button btnNext;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpDesiredTime;
        private Guna.UI2.WinForms.Guna2TextBox txtPackageDescription;
        private CSharp.Winform.UI.Label label8;
        private System.Windows.Forms.Label lblEstimatedFee;
        private Guna.UI2.WinForms.Guna2TextBox txtPickupAddress;
        private System.Windows.Forms.CheckBox chkPickupAtSender;
        private Guna.UI2.WinForms.Guna2ComboBox cmbDestWarehouse;
        private Guna.UI2.WinForms.Guna2ComboBox cmbDestZone;
        private Guna.UI2.WinForms.Guna2ComboBox cmbOriginWarehouse;
        private Guna.UI2.WinForms.Guna2ComboBox cmbOriginZone;
        private CSharp.Winform.UI.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private CSharp.Winform.UI.Label label6;
        private CSharp.Winform.UI.Label label5;
        private CSharp.Winform.UI.Label label4;
        private CSharp.Winform.UI.Label label2;
        private CSharp.Winform.UI.Label label1;
        private Guna.UI2.WinForms.Guna2Button btnCalcFee;
        private CSharp.Winform.UI.Panel panel1;
    }
}
