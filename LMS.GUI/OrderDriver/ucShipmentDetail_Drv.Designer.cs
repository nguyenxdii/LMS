namespace LMS.GUI.OrderDriver
{
    partial class ucShipmentDetail_Drv
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvShipments = new Guna.UI2.WinForms.Guna2DataGridView();
            this.cmbStatus = new Guna.UI2.WinForms.Guna2ComboBox();
            this.dtTo = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.dtFrom = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.btnReload = new Guna.UI2.WinForms.Guna2Button();
            this.btnManageShipment = new Guna.UI2.WinForms.Guna2Button();
            this.tlpContent = new CSharp.Winform.UI.TableLayoutPanel();
            this.lblStartedAt = new System.Windows.Forms.Label();
            this.lblShipmentNo = new System.Windows.Forms.Label();
            this.lblTotalStops = new System.Windows.Forms.Label();
            this.lblOrderNo = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblCustomerName = new System.Windows.Forms.Label();
            this.lblRoute = new System.Windows.Forms.Label();
            this.lblOriginWarehouse = new System.Windows.Forms.Label();
            this.lblDestinationWarehouse = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvShipments)).BeginInit();
            this.tlpContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvShipments
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvShipments.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvShipments.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvShipments.ColumnHeadersHeight = 4;
            this.dgvShipments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvShipments.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvShipments.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvShipments.Location = new System.Drawing.Point(35, 303);
            this.dgvShipments.Name = "dgvShipments";
            this.dgvShipments.RowHeadersVisible = false;
            this.dgvShipments.RowHeadersWidth = 62;
            this.dgvShipments.RowTemplate.Height = 28;
            this.dgvShipments.Size = new System.Drawing.Size(1128, 354);
            this.dgvShipments.TabIndex = 22;
            this.dgvShipments.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvShipments.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvShipments.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvShipments.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvShipments.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvShipments.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvShipments.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvShipments.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvShipments.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvShipments.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.dgvShipments.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvShipments.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvShipments.ThemeStyle.HeaderStyle.Height = 4;
            this.dgvShipments.ThemeStyle.ReadOnly = false;
            this.dgvShipments.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvShipments.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvShipments.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.dgvShipments.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvShipments.ThemeStyle.RowsStyle.Height = 28;
            this.dgvShipments.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvShipments.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // cmbStatus
            // 
            this.cmbStatus.BackColor = System.Drawing.Color.Transparent;
            this.cmbStatus.BorderRadius = 15;
            this.cmbStatus.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbStatus.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbStatus.ItemHeight = 30;
            this.cmbStatus.Location = new System.Drawing.Point(705, 247);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(200, 36);
            this.cmbStatus.TabIndex = 21;
            // 
            // dtTo
            // 
            this.dtTo.BorderRadius = 15;
            this.dtTo.Checked = true;
            this.dtTo.FillColor = System.Drawing.Color.Black;
            this.dtTo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtTo.ForeColor = System.Drawing.Color.White;
            this.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtTo.Location = new System.Drawing.Point(499, 247);
            this.dtTo.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtTo.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(200, 36);
            this.dtTo.TabIndex = 19;
            this.dtTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.dtTo.Value = new System.DateTime(2025, 10, 23, 17, 28, 37, 210);
            // 
            // dtFrom
            // 
            this.dtFrom.BorderRadius = 15;
            this.dtFrom.Checked = true;
            this.dtFrom.FillColor = System.Drawing.Color.Black;
            this.dtFrom.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtFrom.ForeColor = System.Drawing.Color.White;
            this.dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtFrom.Location = new System.Drawing.Point(293, 247);
            this.dtFrom.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtFrom.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(200, 36);
            this.dtFrom.TabIndex = 20;
            this.dtFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.dtFrom.Value = new System.DateTime(2025, 10, 23, 17, 28, 37, 210);
            // 
            // btnReload
            // 
            this.btnReload.BorderRadius = 15;
            this.btnReload.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnReload.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnReload.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnReload.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnReload.FillColor = System.Drawing.Color.Black;
            this.btnReload.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnReload.ForeColor = System.Drawing.Color.White;
            this.btnReload.Location = new System.Drawing.Point(499, 190);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(190, 45);
            this.btnReload.TabIndex = 18;
            this.btnReload.Text = "Tải lại";
            // 
            // btnManageShipment
            // 
            this.btnManageShipment.BorderRadius = 15;
            this.btnManageShipment.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnManageShipment.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnManageShipment.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnManageShipment.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnManageShipment.FillColor = System.Drawing.Color.Black;
            this.btnManageShipment.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnManageShipment.ForeColor = System.Drawing.Color.White;
            this.btnManageShipment.Location = new System.Drawing.Point(293, 190);
            this.btnManageShipment.Name = "btnManageShipment";
            this.btnManageShipment.Size = new System.Drawing.Size(200, 45);
            this.btnManageShipment.TabIndex = 18;
            this.btnManageShipment.Text = "Quản lý chuyến";
            // 
            // tlpContent
            // 
            this.tlpContent.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tlpContent.ColumnCount = 3;
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpContent.Controls.Add(this.lblStartedAt, 2, 2);
            this.tlpContent.Controls.Add(this.lblShipmentNo, 0, 0);
            this.tlpContent.Controls.Add(this.lblTotalStops, 2, 1);
            this.tlpContent.Controls.Add(this.lblOrderNo, 0, 1);
            this.tlpContent.Controls.Add(this.lblStatus, 2, 0);
            this.tlpContent.Controls.Add(this.lblCustomerName, 0, 2);
            this.tlpContent.Controls.Add(this.lblRoute, 1, 2);
            this.tlpContent.Controls.Add(this.lblOriginWarehouse, 1, 0);
            this.tlpContent.Controls.Add(this.lblDestinationWarehouse, 1, 1);
            this.tlpContent.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.tlpContent.Location = new System.Drawing.Point(121, 22);
            this.tlpContent.Name = "tlpContent";
            this.tlpContent.RowCount = 3;
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tlpContent.Size = new System.Drawing.Size(957, 147);
            this.tlpContent.TabIndex = 26;
            // 
            // lblStartedAt
            // 
            this.lblStartedAt.AutoSize = true;
            this.lblStartedAt.ForeColor = System.Drawing.Color.Black;
            this.lblStartedAt.Location = new System.Drawing.Point(207, 91);
            this.lblStartedAt.Name = "lblStartedAt";
            this.lblStartedAt.Size = new System.Drawing.Size(79, 28);
            this.lblStartedAt.TabIndex = 43;
            this.lblStartedAt.Text = "bắt đầu";
            this.lblStartedAt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblShipmentNo
            // 
            this.lblShipmentNo.AutoSize = true;
            this.lblShipmentNo.ForeColor = System.Drawing.Color.Black;
            this.lblShipmentNo.Location = new System.Drawing.Point(4, 1);
            this.lblShipmentNo.Name = "lblShipmentNo";
            this.lblShipmentNo.Size = new System.Drawing.Size(71, 28);
            this.lblShipmentNo.TabIndex = 35;
            this.lblShipmentNo.Text = "Mã CH";
            this.lblShipmentNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTotalStops
            // 
            this.lblTotalStops.AutoSize = true;
            this.lblTotalStops.ForeColor = System.Drawing.Color.Black;
            this.lblTotalStops.Location = new System.Drawing.Point(207, 46);
            this.lblTotalStops.Name = "lblTotalStops";
            this.lblTotalStops.Size = new System.Drawing.Size(90, 28);
            this.lblTotalStops.TabIndex = 42;
            this.lblTotalStops.Text = "số chặng";
            this.lblTotalStops.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOrderNo
            // 
            this.lblOrderNo.AutoSize = true;
            this.lblOrderNo.ForeColor = System.Drawing.Color.Black;
            this.lblOrderNo.Location = new System.Drawing.Point(4, 46);
            this.lblOrderNo.Name = "lblOrderNo";
            this.lblOrderNo.Size = new System.Drawing.Size(80, 28);
            this.lblOrderNo.TabIndex = 36;
            this.lblOrderNo.Text = "Mã đơn";
            this.lblOrderNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.ForeColor = System.Drawing.Color.Black;
            this.lblStatus.Location = new System.Drawing.Point(207, 1);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(97, 28);
            this.lblStatus.TabIndex = 41;
            this.lblStatus.Text = "trạng thái";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.AutoSize = true;
            this.lblCustomerName.ForeColor = System.Drawing.Color.Black;
            this.lblCustomerName.Location = new System.Drawing.Point(4, 91);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(96, 28);
            this.lblCustomerName.TabIndex = 37;
            this.lblCustomerName.Text = "tên khách";
            this.lblCustomerName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRoute
            // 
            this.lblRoute.AutoSize = true;
            this.lblRoute.ForeColor = System.Drawing.Color.Black;
            this.lblRoute.Location = new System.Drawing.Point(107, 91);
            this.lblRoute.Name = "lblRoute";
            this.lblRoute.Size = new System.Drawing.Size(61, 28);
            this.lblRoute.TabIndex = 40;
            this.lblRoute.Text = "tuyến";
            this.lblRoute.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOriginWarehouse
            // 
            this.lblOriginWarehouse.AutoSize = true;
            this.lblOriginWarehouse.ForeColor = System.Drawing.Color.Black;
            this.lblOriginWarehouse.Location = new System.Drawing.Point(107, 1);
            this.lblOriginWarehouse.Name = "lblOriginWarehouse";
            this.lblOriginWarehouse.Size = new System.Drawing.Size(79, 28);
            this.lblOriginWarehouse.TabIndex = 38;
            this.lblOriginWarehouse.Text = "kho gửi";
            this.lblOriginWarehouse.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDestinationWarehouse
            // 
            this.lblDestinationWarehouse.AutoSize = true;
            this.lblDestinationWarehouse.ForeColor = System.Drawing.Color.Black;
            this.lblDestinationWarehouse.Location = new System.Drawing.Point(107, 46);
            this.lblDestinationWarehouse.Name = "lblDestinationWarehouse";
            this.lblDestinationWarehouse.Size = new System.Drawing.Size(93, 28);
            this.lblDestinationWarehouse.TabIndex = 39;
            this.lblDestinationWarehouse.Text = "kho nhận";
            this.lblDestinationWarehouse.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ucShipmentDetail_Drv
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpContent);
            this.Controls.Add(this.dgvShipments);
            this.Controls.Add(this.cmbStatus);
            this.Controls.Add(this.dtTo);
            this.Controls.Add(this.dtFrom);
            this.Controls.Add(this.btnManageShipment);
            this.Controls.Add(this.btnReload);
            this.Name = "ucShipmentDetail_Drv";
            this.Size = new System.Drawing.Size(1199, 689);
            ((System.ComponentModel.ISupportInitialize)(this.dgvShipments)).EndInit();
            this.tlpContent.ResumeLayout(false);
            this.tlpContent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2DataGridView dgvShipments;
        private Guna.UI2.WinForms.Guna2ComboBox cmbStatus;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtTo;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtFrom;
        private Guna.UI2.WinForms.Guna2Button btnReload;
        private Guna.UI2.WinForms.Guna2Button btnManageShipment;
        private CSharp.Winform.UI.TableLayoutPanel tlpContent;
        private System.Windows.Forms.Label lblStartedAt;
        private System.Windows.Forms.Label lblShipmentNo;
        private System.Windows.Forms.Label lblTotalStops;
        private System.Windows.Forms.Label lblOrderNo;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblCustomerName;
        private System.Windows.Forms.Label lblRoute;
        private System.Windows.Forms.Label lblOriginWarehouse;
        private System.Windows.Forms.Label lblDestinationWarehouse;
    }
}
