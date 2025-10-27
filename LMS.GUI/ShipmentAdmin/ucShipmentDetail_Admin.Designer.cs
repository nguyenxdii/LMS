namespace LMS.GUI.ShipmentAdmin
{
    partial class ucShipmentDetail_Admin
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
            this.pnlTop = new CSharp.Winform.UI.Panel();
            this.guna2ControlBox3 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.dgvDriverHistory = new CSharp.Winform.UI.DataGridView();
            this.dgvRouteStops = new CSharp.Winform.UI.DataGridView();
            this.panel1 = new CSharp.Winform.UI.Panel();
            this.tableLayoutPanel1 = new CSharp.Winform.UI.TableLayoutPanel();
            this.lblDuration = new System.Windows.Forms.Label();
            this.lblVehicleNo = new System.Windows.Forms.Label();
            this.lblDeliveredAt = new System.Windows.Forms.Label();
            this.lblDriverName = new System.Windows.Forms.Label();
            this.lblStartedAt = new System.Windows.Forms.Label();
            this.lblCustomerName = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblOrderNo = new System.Windows.Forms.Label();
            this.lblRoute = new System.Windows.Forms.Label();
            this.lblShipmentNo = new System.Windows.Forms.Label();
            this.txtNotes = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnClose = new Guna.UI2.WinForms.Guna2Button();
            this.label6 = new System.Windows.Forms.Label();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDriverHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRouteStops)).BeginInit();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.Controls.Add(this.guna2ControlBox3);
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1426, 49);
            this.pnlTop.TabIndex = 9;
            // 
            // guna2ControlBox3
            // 
            this.guna2ControlBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2ControlBox3.BorderRadius = 5;
            this.guna2ControlBox3.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(24)))), ((int)(((byte)(29)))));
            this.guna2ControlBox3.IconColor = System.Drawing.Color.White;
            this.guna2ControlBox3.Location = new System.Drawing.Point(1374, 5);
            this.guna2ControlBox3.Name = "guna2ControlBox3";
            this.guna2ControlBox3.Size = new System.Drawing.Size(46, 35);
            this.guna2ControlBox3.TabIndex = 5;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblTitle.Location = new System.Drawing.Point(16, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(102, 25);
            this.lblTitle.TabIndex = 13;
            this.lblTitle.Text = "Tên Chuyến";
            // 
            // dgvDriverHistory
            // 
            this.dgvDriverHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDriverHistory.Location = new System.Drawing.Point(672, 296);
            this.dgvDriverHistory.Name = "dgvDriverHistory";
            this.dgvDriverHistory.RowHeadersWidth = 62;
            this.dgvDriverHistory.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.dgvDriverHistory.RowTemplate.Height = 28;
            this.dgvDriverHistory.Size = new System.Drawing.Size(646, 396);
            this.dgvDriverHistory.TabIndex = 12;
            // 
            // dgvRouteStops
            // 
            this.dgvRouteStops.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRouteStops.Location = new System.Drawing.Point(20, 296);
            this.dgvRouteStops.Name = "dgvRouteStops";
            this.dgvRouteStops.RowHeadersWidth = 62;
            this.dgvRouteStops.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.dgvRouteStops.RowTemplate.Height = 28;
            this.dgvRouteStops.Size = new System.Drawing.Size(646, 396);
            this.dgvRouteStops.TabIndex = 12;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Controls.Add(this.txtNotes);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.dgvDriverHistory);
            this.panel1.Controls.Add(this.dgvRouteStops);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.pnlTop);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1426, 782);
            this.panel1.TabIndex = 10;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.lblDuration, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblVehicleNo, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblDeliveredAt, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblDriverName, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblStartedAt, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblCustomerName, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblStatus, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblOrderNo, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblRoute, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblShipmentNo, 0, 0);
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(13, 55);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(917, 224);
            this.tableLayoutPanel1.TabIndex = 15;
            // 
            // lblDuration
            // 
            this.lblDuration.AutoSize = true;
            this.lblDuration.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblDuration.Location = new System.Drawing.Point(461, 176);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(102, 25);
            this.lblDuration.TabIndex = 22;
            this.lblDuration.Text = "Tên Chuyến";
            // 
            // lblVehicleNo
            // 
            this.lblVehicleNo.AutoSize = true;
            this.lblVehicleNo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblVehicleNo.Location = new System.Drawing.Point(3, 176);
            this.lblVehicleNo.Name = "lblVehicleNo";
            this.lblVehicleNo.Size = new System.Drawing.Size(102, 25);
            this.lblVehicleNo.TabIndex = 21;
            this.lblVehicleNo.Text = "Tên Chuyến";
            // 
            // lblDeliveredAt
            // 
            this.lblDeliveredAt.AutoSize = true;
            this.lblDeliveredAt.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblDeliveredAt.Location = new System.Drawing.Point(461, 132);
            this.lblDeliveredAt.Name = "lblDeliveredAt";
            this.lblDeliveredAt.Size = new System.Drawing.Size(102, 25);
            this.lblDeliveredAt.TabIndex = 20;
            this.lblDeliveredAt.Text = "Tên Chuyến";
            // 
            // lblDriverName
            // 
            this.lblDriverName.AutoSize = true;
            this.lblDriverName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblDriverName.Location = new System.Drawing.Point(3, 132);
            this.lblDriverName.Name = "lblDriverName";
            this.lblDriverName.Size = new System.Drawing.Size(102, 25);
            this.lblDriverName.TabIndex = 19;
            this.lblDriverName.Text = "Tên Chuyến";
            // 
            // lblStartedAt
            // 
            this.lblStartedAt.AutoSize = true;
            this.lblStartedAt.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblStartedAt.Location = new System.Drawing.Point(461, 88);
            this.lblStartedAt.Name = "lblStartedAt";
            this.lblStartedAt.Size = new System.Drawing.Size(102, 25);
            this.lblStartedAt.TabIndex = 18;
            this.lblStartedAt.Text = "Tên Chuyến";
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.AutoSize = true;
            this.lblCustomerName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblCustomerName.Location = new System.Drawing.Point(3, 88);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(102, 25);
            this.lblCustomerName.TabIndex = 17;
            this.lblCustomerName.Text = "Tên Chuyến";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblStatus.Location = new System.Drawing.Point(461, 44);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(102, 25);
            this.lblStatus.TabIndex = 16;
            this.lblStatus.Text = "Tên Chuyến";
            // 
            // lblOrderNo
            // 
            this.lblOrderNo.AutoSize = true;
            this.lblOrderNo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblOrderNo.Location = new System.Drawing.Point(3, 44);
            this.lblOrderNo.Name = "lblOrderNo";
            this.lblOrderNo.Size = new System.Drawing.Size(102, 25);
            this.lblOrderNo.TabIndex = 15;
            this.lblOrderNo.Text = "Tên Chuyến";
            // 
            // lblRoute
            // 
            this.lblRoute.AutoSize = true;
            this.lblRoute.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblRoute.Location = new System.Drawing.Point(461, 0);
            this.lblRoute.Name = "lblRoute";
            this.lblRoute.Size = new System.Drawing.Size(102, 25);
            this.lblRoute.TabIndex = 14;
            this.lblRoute.Text = "Tên Chuyến";
            // 
            // lblShipmentNo
            // 
            this.lblShipmentNo.AutoSize = true;
            this.lblShipmentNo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblShipmentNo.Location = new System.Drawing.Point(3, 0);
            this.lblShipmentNo.Name = "lblShipmentNo";
            this.lblShipmentNo.Size = new System.Drawing.Size(102, 25);
            this.lblShipmentNo.TabIndex = 13;
            this.lblShipmentNo.Text = "Tên Chuyến";
            // 
            // txtNotes
            // 
            this.txtNotes.BorderRadius = 15;
            this.txtNotes.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNotes.DefaultText = "";
            this.txtNotes.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtNotes.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtNotes.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtNotes.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtNotes.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtNotes.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtNotes.ForeColor = System.Drawing.Color.Black;
            this.txtNotes.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtNotes.Location = new System.Drawing.Point(1027, 108);
            this.txtNotes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.PlaceholderText = "";
            this.txtNotes.SelectedText = "";
            this.txtNotes.Size = new System.Drawing.Size(236, 36);
            this.txtNotes.TabIndex = 12;
            // 
            // btnClose
            // 
            this.btnClose.BorderRadius = 15;
            this.btnClose.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnClose.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnClose.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnClose.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnClose.FillColor = System.Drawing.Color.Black;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(948, 55);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(180, 45);
            this.btnClose.TabIndex = 14;
            this.btnClose.Text = "Đóng";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label6.Location = new System.Drawing.Point(935, 119);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 25);
            this.label6.TabIndex = 13;
            this.label6.Text = "Tìm Kiếm";
            // 
            // ucShipmentDetail_Admin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.panel1);
            this.Name = "ucShipmentDetail_Admin";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Size = new System.Drawing.Size(1430, 786);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDriverHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRouteStops)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private CSharp.Winform.UI.Panel pnlTop;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox3;
        private CSharp.Winform.UI.DataGridView dgvDriverHistory;
        private CSharp.Winform.UI.DataGridView dgvRouteStops;
        private CSharp.Winform.UI.Panel panel1;
        private Guna.UI2.WinForms.Guna2TextBox txtNotes;
        private System.Windows.Forms.Label label6;
        private CSharp.Winform.UI.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblDuration;
        private System.Windows.Forms.Label lblVehicleNo;
        private System.Windows.Forms.Label lblDeliveredAt;
        private System.Windows.Forms.Label lblDriverName;
        private System.Windows.Forms.Label lblStartedAt;
        private System.Windows.Forms.Label lblCustomerName;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblOrderNo;
        private System.Windows.Forms.Label lblRoute;
        private System.Windows.Forms.Label lblShipmentNo;
        private Guna.UI2.WinForms.Guna2Button btnClose;
        private System.Windows.Forms.Label lblTitle;
    }
}
