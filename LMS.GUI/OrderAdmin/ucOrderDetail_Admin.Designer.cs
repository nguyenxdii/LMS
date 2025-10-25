namespace LMS.GUI.OrderAdmin
{
    partial class ucOrderDetail_Admin
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
            this.panel1 = new CSharp.Winform.UI.Panel();
            this.btnBack = new Guna.UI2.WinForms.Guna2Button();
            this.grpDetail = new Guna.UI2.WinForms.Guna2GroupBox();
            this.panel2 = new CSharp.Winform.UI.Panel();
            this.guna2ControlBox1 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.guna2ControlBox3 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.lblCreatedAt = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblDepositAmount = new System.Windows.Forms.Label();
            this.lblTotalFee = new System.Windows.Forms.Label();
            this.lblPackage = new System.Windows.Forms.Label();
            this.lblPickupAddress = new System.Windows.Forms.Label();
            this.lblDest = new System.Windows.Forms.Label();
            this.lblOrigin = new System.Windows.Forms.Label();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.lblOrderId = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.grpDetail.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.grpDetail);
            this.panel1.Controls.Add(this.btnBack);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(496, 496);
            this.panel1.TabIndex = 0;
            // 
            // btnBack
            // 
            this.btnBack.BorderRadius = 15;
            this.btnBack.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnBack.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnBack.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnBack.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnBack.FillColor = System.Drawing.Color.Black;
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.Location = new System.Drawing.Point(158, 435);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(180, 45);
            this.btnBack.TabIndex = 49;
            this.btnBack.Text = "Quay lại";
            // 
            // grpDetail
            // 
            this.grpDetail.BorderRadius = 15;
            this.grpDetail.Controls.Add(this.lblCreatedAt);
            this.grpDetail.Controls.Add(this.lblStatus);
            this.grpDetail.Controls.Add(this.lblDepositAmount);
            this.grpDetail.Controls.Add(this.lblTotalFee);
            this.grpDetail.Controls.Add(this.lblPackage);
            this.grpDetail.Controls.Add(this.lblPickupAddress);
            this.grpDetail.Controls.Add(this.lblDest);
            this.grpDetail.Controls.Add(this.lblOrigin);
            this.grpDetail.Controls.Add(this.lblCustomer);
            this.grpDetail.Controls.Add(this.lblOrderId);
            this.grpDetail.CustomBorderColor = System.Drawing.Color.Black;
            this.grpDetail.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.grpDetail.ForeColor = System.Drawing.Color.White;
            this.grpDetail.Location = new System.Drawing.Point(24, 47);
            this.grpDetail.Name = "grpDetail";
            this.grpDetail.Size = new System.Drawing.Size(448, 382);
            this.grpDetail.TabIndex = 50;
            this.grpDetail.Text = "guna2GroupBox1";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.guna2ControlBox1);
            this.panel2.Controls.Add(this.guna2ControlBox3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(496, 41);
            this.panel2.TabIndex = 51;
            // 
            // guna2ControlBox1
            // 
            this.guna2ControlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2ControlBox1.BorderRadius = 5;
            this.guna2ControlBox1.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MinimizeBox;
            this.guna2ControlBox1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(24)))), ((int)(((byte)(29)))));
            this.guna2ControlBox1.IconColor = System.Drawing.Color.White;
            this.guna2ControlBox1.Location = new System.Drawing.Point(377, 3);
            this.guna2ControlBox1.Name = "guna2ControlBox1";
            this.guna2ControlBox1.Size = new System.Drawing.Size(55, 33);
            this.guna2ControlBox1.TabIndex = 4;
            // 
            // guna2ControlBox3
            // 
            this.guna2ControlBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2ControlBox3.BorderRadius = 5;
            this.guna2ControlBox3.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(24)))), ((int)(((byte)(29)))));
            this.guna2ControlBox3.IconColor = System.Drawing.Color.White;
            this.guna2ControlBox3.Location = new System.Drawing.Point(438, 3);
            this.guna2ControlBox3.Name = "guna2ControlBox3";
            this.guna2ControlBox3.Size = new System.Drawing.Size(55, 33);
            this.guna2ControlBox3.TabIndex = 4;
            // 
            // lblCreatedAt
            // 
            this.lblCreatedAt.AutoSize = true;
            this.lblCreatedAt.BackColor = System.Drawing.Color.Transparent;
            this.lblCreatedAt.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblCreatedAt.ForeColor = System.Drawing.Color.Black;
            this.lblCreatedAt.Location = new System.Drawing.Point(31, 343);
            this.lblCreatedAt.Name = "lblCreatedAt";
            this.lblCreatedAt.Size = new System.Drawing.Size(85, 25);
            this.lblCreatedAt.TabIndex = 64;
            this.lblCreatedAt.Text = "Ngày tạo";
            this.lblCreatedAt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblStatus.ForeColor = System.Drawing.Color.Black;
            this.lblStatus.Location = new System.Drawing.Point(30, 307);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(79, 25);
            this.lblStatus.TabIndex = 63;
            this.lblStatus.Text = "lblStatus";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDepositAmount
            // 
            this.lblDepositAmount.AutoSize = true;
            this.lblDepositAmount.BackColor = System.Drawing.Color.Transparent;
            this.lblDepositAmount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblDepositAmount.ForeColor = System.Drawing.Color.Black;
            this.lblDepositAmount.Location = new System.Drawing.Point(30, 275);
            this.lblDepositAmount.Name = "lblDepositAmount";
            this.lblDepositAmount.Size = new System.Drawing.Size(158, 25);
            this.lblDepositAmount.TabIndex = 62;
            this.lblDepositAmount.Text = "lblDepositAmount";
            this.lblDepositAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTotalFee
            // 
            this.lblTotalFee.AutoSize = true;
            this.lblTotalFee.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalFee.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblTotalFee.ForeColor = System.Drawing.Color.Black;
            this.lblTotalFee.Location = new System.Drawing.Point(30, 243);
            this.lblTotalFee.Name = "lblTotalFee";
            this.lblTotalFee.Size = new System.Drawing.Size(95, 25);
            this.lblTotalFee.TabIndex = 61;
            this.lblTotalFee.Text = "lblTotalFee";
            this.lblTotalFee.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPackage
            // 
            this.lblPackage.AutoSize = true;
            this.lblPackage.BackColor = System.Drawing.Color.Transparent;
            this.lblPackage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblPackage.ForeColor = System.Drawing.Color.Black;
            this.lblPackage.Location = new System.Drawing.Point(30, 211);
            this.lblPackage.Name = "lblPackage";
            this.lblPackage.Size = new System.Drawing.Size(95, 25);
            this.lblPackage.TabIndex = 60;
            this.lblPackage.Text = "lblPackage";
            this.lblPackage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPickupAddress
            // 
            this.lblPickupAddress.AutoSize = true;
            this.lblPickupAddress.BackColor = System.Drawing.Color.Transparent;
            this.lblPickupAddress.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblPickupAddress.ForeColor = System.Drawing.Color.Black;
            this.lblPickupAddress.Location = new System.Drawing.Point(30, 179);
            this.lblPickupAddress.Name = "lblPickupAddress";
            this.lblPickupAddress.Size = new System.Drawing.Size(148, 25);
            this.lblPickupAddress.TabIndex = 59;
            this.lblPickupAddress.Text = "lblPickupAddress";
            this.lblPickupAddress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDest
            // 
            this.lblDest.AutoSize = true;
            this.lblDest.BackColor = System.Drawing.Color.Transparent;
            this.lblDest.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblDest.ForeColor = System.Drawing.Color.Black;
            this.lblDest.Location = new System.Drawing.Point(30, 147);
            this.lblDest.Name = "lblDest";
            this.lblDest.Size = new System.Drawing.Size(67, 25);
            this.lblDest.TabIndex = 58;
            this.lblDest.Text = "lblDest";
            this.lblDest.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOrigin
            // 
            this.lblOrigin.AutoSize = true;
            this.lblOrigin.BackColor = System.Drawing.Color.Transparent;
            this.lblOrigin.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblOrigin.ForeColor = System.Drawing.Color.Black;
            this.lblOrigin.Location = new System.Drawing.Point(30, 115);
            this.lblOrigin.Name = "lblOrigin";
            this.lblOrigin.Size = new System.Drawing.Size(80, 25);
            this.lblOrigin.TabIndex = 57;
            this.lblOrigin.Text = "lblOrigin";
            this.lblOrigin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCustomer
            // 
            this.lblCustomer.AutoSize = true;
            this.lblCustomer.BackColor = System.Drawing.Color.Transparent;
            this.lblCustomer.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblCustomer.ForeColor = System.Drawing.Color.Black;
            this.lblCustomer.Location = new System.Drawing.Point(30, 83);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(108, 25);
            this.lblCustomer.TabIndex = 56;
            this.lblCustomer.Text = "lblCustomer";
            this.lblCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOrderId
            // 
            this.lblOrderId.AutoSize = true;
            this.lblOrderId.BackColor = System.Drawing.Color.Transparent;
            this.lblOrderId.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblOrderId.ForeColor = System.Drawing.Color.Black;
            this.lblOrderId.Location = new System.Drawing.Point(30, 51);
            this.lblOrderId.Name = "lblOrderId";
            this.lblOrderId.Size = new System.Drawing.Size(93, 25);
            this.lblOrderId.TabIndex = 55;
            this.lblOrderId.Text = "lblOrderId";
            this.lblOrderId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ucOrderDetail_Admin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.panel1);
            this.Name = "ucOrderDetail_Admin";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Size = new System.Drawing.Size(500, 500);
            this.panel1.ResumeLayout(false);
            this.grpDetail.ResumeLayout(false);
            this.grpDetail.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CSharp.Winform.UI.Panel panel1;
        private Guna.UI2.WinForms.Guna2Button btnBack;
        private Guna.UI2.WinForms.Guna2GroupBox grpDetail;
        private CSharp.Winform.UI.Panel panel2;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox1;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox3;
        private System.Windows.Forms.Label lblCreatedAt;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblDepositAmount;
        private System.Windows.Forms.Label lblTotalFee;
        private System.Windows.Forms.Label lblPackage;
        private System.Windows.Forms.Label lblPickupAddress;
        private System.Windows.Forms.Label lblDest;
        private System.Windows.Forms.Label lblOrigin;
        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.Label lblOrderId;
    }
}
