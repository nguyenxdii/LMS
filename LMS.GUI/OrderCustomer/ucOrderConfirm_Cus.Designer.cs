namespace LMS.GUI.OrderCustomer
{
    partial class ucOrderConfirm_Cus
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
            this.btnBack = new Guna.UI2.WinForms.Guna2Button();
            this.btnConfirmCreate = new Guna.UI2.WinForms.Guna2Button();
            this.btnCancel = new Guna.UI2.WinForms.Guna2Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lblRemainingAmount = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.lblDepositAmount = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.lblDepositPercent = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.lblTotalFee = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.lblPickupFee = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.lblRouteFee = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.lblPackageDesc = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lblDesiredTime = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblPickup = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblDest = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblOrigin = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBack
            // 
            this.btnBack.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnBack.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnBack.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnBack.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.Location = new System.Drawing.Point(24, 314);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(168, 45);
            this.btnBack.TabIndex = 2;
            this.btnBack.Text = "Quay lại tạo đơn";
            // 
            // btnConfirmCreate
            // 
            this.btnConfirmCreate.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnConfirmCreate.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnConfirmCreate.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnConfirmCreate.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnConfirmCreate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnConfirmCreate.ForeColor = System.Drawing.Color.White;
            this.btnConfirmCreate.Location = new System.Drawing.Point(221, 314);
            this.btnConfirmCreate.Name = "btnConfirmCreate";
            this.btnConfirmCreate.Size = new System.Drawing.Size(159, 45);
            this.btnConfirmCreate.TabIndex = 2;
            this.btnConfirmCreate.Text = "Xác nhận";
            // 
            // btnCancel
            // 
            this.btnCancel.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCancel.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnCancel.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnCancel.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(422, 314);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(159, 45);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Hủy";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.lblRemainingAmount, 1, 13);
            this.tableLayoutPanel2.Controls.Add(this.label25, 0, 13);
            this.tableLayoutPanel2.Controls.Add(this.lblDepositAmount, 1, 12);
            this.tableLayoutPanel2.Controls.Add(this.label23, 0, 12);
            this.tableLayoutPanel2.Controls.Add(this.lblDepositPercent, 1, 11);
            this.tableLayoutPanel2.Controls.Add(this.label21, 0, 11);
            this.tableLayoutPanel2.Controls.Add(this.lblTotalFee, 1, 10);
            this.tableLayoutPanel2.Controls.Add(this.label19, 0, 10);
            this.tableLayoutPanel2.Controls.Add(this.lblPickupFee, 1, 9);
            this.tableLayoutPanel2.Controls.Add(this.label17, 0, 9);
            this.tableLayoutPanel2.Controls.Add(this.lblRouteFee, 1, 8);
            this.tableLayoutPanel2.Controls.Add(this.label15, 0, 8);
            this.tableLayoutPanel2.Controls.Add(this.lblPackageDesc, 1, 7);
            this.tableLayoutPanel2.Controls.Add(this.label13, 0, 7);
            this.tableLayoutPanel2.Controls.Add(this.lblDesiredTime, 1, 6);
            this.tableLayoutPanel2.Controls.Add(this.label11, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.lblPickup, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.label9, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.lblDest, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.label7, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.lblOrigin, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(66, 64);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 14;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(421, 225);
            this.tableLayoutPanel2.TabIndex = 5;
            // 
            // lblRemainingAmount
            // 
            this.lblRemainingAmount.AutoSize = true;
            this.lblRemainingAmount.Location = new System.Drawing.Point(213, 200);
            this.lblRemainingAmount.Name = "lblRemainingAmount";
            this.lblRemainingAmount.Size = new System.Drawing.Size(60, 20);
            this.lblRemainingAmount.TabIndex = 29;
            this.lblRemainingAmount.Text = "label26";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label25.Location = new System.Drawing.Point(3, 200);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(204, 25);
            this.label25.TabIndex = 28;
            this.label25.Text = "Thanh toán khi nhận hàng";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDepositAmount
            // 
            this.lblDepositAmount.AutoSize = true;
            this.lblDepositAmount.Location = new System.Drawing.Point(213, 180);
            this.lblDepositAmount.Name = "lblDepositAmount";
            this.lblDepositAmount.Size = new System.Drawing.Size(60, 20);
            this.lblDepositAmount.TabIndex = 27;
            this.lblDepositAmount.Text = "label24";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label23.Location = new System.Drawing.Point(3, 180);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(204, 20);
            this.label23.TabIndex = 26;
            this.label23.Text = "Số tiền COD";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDepositPercent
            // 
            this.lblDepositPercent.AutoSize = true;
            this.lblDepositPercent.Location = new System.Drawing.Point(213, 160);
            this.lblDepositPercent.Name = "lblDepositPercent";
            this.lblDepositPercent.Size = new System.Drawing.Size(60, 20);
            this.lblDepositPercent.TabIndex = 25;
            this.lblDepositPercent.Text = "label22";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label21.Location = new System.Drawing.Point(3, 160);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(204, 20);
            this.label21.TabIndex = 24;
            this.label21.Text = "COD";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTotalFee
            // 
            this.lblTotalFee.AutoSize = true;
            this.lblTotalFee.Location = new System.Drawing.Point(213, 140);
            this.lblTotalFee.Name = "lblTotalFee";
            this.lblTotalFee.Size = new System.Drawing.Size(60, 20);
            this.lblTotalFee.TabIndex = 23;
            this.lblTotalFee.Text = "label20";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label19.Location = new System.Drawing.Point(3, 140);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(204, 20);
            this.label19.TabIndex = 22;
            this.label19.Text = "Tổng phí";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPickupFee
            // 
            this.lblPickupFee.AutoSize = true;
            this.lblPickupFee.Location = new System.Drawing.Point(213, 120);
            this.lblPickupFee.Name = "lblPickupFee";
            this.lblPickupFee.Size = new System.Drawing.Size(60, 20);
            this.lblPickupFee.TabIndex = 21;
            this.lblPickupFee.Text = "label18";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label17.Location = new System.Drawing.Point(3, 120);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(204, 20);
            this.label17.TabIndex = 20;
            this.label17.Text = "Phí giao hàng";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRouteFee
            // 
            this.lblRouteFee.AutoSize = true;
            this.lblRouteFee.Location = new System.Drawing.Point(213, 100);
            this.lblRouteFee.Name = "lblRouteFee";
            this.lblRouteFee.Size = new System.Drawing.Size(60, 20);
            this.lblRouteFee.TabIndex = 19;
            this.lblRouteFee.Text = "label16";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label15.Location = new System.Drawing.Point(3, 100);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(204, 20);
            this.label15.TabIndex = 18;
            this.label15.Text = "Phí tuyến đường";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPackageDesc
            // 
            this.lblPackageDesc.AutoSize = true;
            this.lblPackageDesc.Location = new System.Drawing.Point(213, 80);
            this.lblPackageDesc.Name = "lblPackageDesc";
            this.lblPackageDesc.Size = new System.Drawing.Size(60, 20);
            this.lblPackageDesc.TabIndex = 17;
            this.lblPackageDesc.Text = "label14";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Location = new System.Drawing.Point(3, 80);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(204, 20);
            this.label13.TabIndex = 16;
            this.label13.Text = "Ghi chú";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDesiredTime
            // 
            this.lblDesiredTime.AutoSize = true;
            this.lblDesiredTime.Location = new System.Drawing.Point(213, 60);
            this.lblDesiredTime.Name = "lblDesiredTime";
            this.lblDesiredTime.Size = new System.Drawing.Size(60, 20);
            this.lblDesiredTime.TabIndex = 15;
            this.lblDesiredTime.Text = "label12";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Location = new System.Drawing.Point(3, 60);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(204, 20);
            this.label11.TabIndex = 14;
            this.label11.Text = "Thời gian lấy hàng";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPickup
            // 
            this.lblPickup.AutoSize = true;
            this.lblPickup.Location = new System.Drawing.Point(213, 40);
            this.lblPickup.Name = "lblPickup";
            this.lblPickup.Size = new System.Drawing.Size(60, 20);
            this.lblPickup.TabIndex = 13;
            this.lblPickup.Text = "label10";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(3, 40);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(204, 20);
            this.label9.TabIndex = 12;
            this.label9.Text = "Địa chỉ đến lấy hàng";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDest
            // 
            this.lblDest.AutoSize = true;
            this.lblDest.Location = new System.Drawing.Point(213, 20);
            this.lblDest.Name = "lblDest";
            this.lblDest.Size = new System.Drawing.Size(51, 20);
            this.lblDest.TabIndex = 11;
            this.lblDest.Text = "label8";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(3, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(204, 20);
            this.label7.TabIndex = 10;
            this.label7.Text = "Kho nhận";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblOrigin
            // 
            this.lblOrigin.AutoSize = true;
            this.lblOrigin.Location = new System.Drawing.Point(213, 0);
            this.lblOrigin.Name = "lblOrigin";
            this.lblOrigin.Size = new System.Drawing.Size(51, 20);
            this.lblOrigin.TabIndex = 7;
            this.lblOrigin.Text = "label4";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(204, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Kho gửi";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ucOrderConfirm_Cus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirmCreate);
            this.Controls.Add(this.btnBack);
            this.Name = "ucOrderConfirm_Cus";
            this.Size = new System.Drawing.Size(1058, 825);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Guna.UI2.WinForms.Guna2Button btnBack;
        private Guna.UI2.WinForms.Guna2Button btnConfirmCreate;
        private Guna.UI2.WinForms.Guna2Button btnCancel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label lblRemainingAmount;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label lblDepositAmount;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label lblDepositPercent;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label lblTotalFee;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label lblPickupFee;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label lblRouteFee;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label lblPackageDesc;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lblDesiredTime;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblPickup;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblDest;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblOrigin;
        private System.Windows.Forms.Label label3;
    }
}
