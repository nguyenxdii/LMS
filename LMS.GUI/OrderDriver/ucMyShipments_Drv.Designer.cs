namespace LMS.GUI.OrderDriver
{
    partial class ucMyShipments_Drv
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblHistory = new System.Windows.Forms.Label();
            this.lblAssigned = new System.Windows.Forms.Label();
            this.dgvActive = new Guna.UI2.WinForms.Guna2DataGridView();
            this.dgvAll = new Guna.UI2.WinForms.Guna2DataGridView();
            this.cmbStatus = new Guna.UI2.WinForms.Guna2ComboBox();
            this.dtTo = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.dtFrom = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.btnSearchHistory = new Guna.UI2.WinForms.Guna2Button();
            this.btnReceive = new Guna.UI2.WinForms.Guna2Button();
            this.btnReload = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvActive)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAll)).BeginInit();
            this.SuspendLayout();
            // 
            // lblHistory
            // 
            this.lblHistory.AutoSize = true;
            this.lblHistory.Location = new System.Drawing.Point(395, -36);
            this.lblHistory.Name = "lblHistory";
            this.lblHistory.Size = new System.Drawing.Size(73, 20);
            this.lblHistory.TabIndex = 18;
            this.lblHistory.Text = "lblHistory";
            // 
            // lblAssigned
            // 
            this.lblAssigned.AutoSize = true;
            this.lblAssigned.Location = new System.Drawing.Point(139, -49);
            this.lblAssigned.Name = "lblAssigned";
            this.lblAssigned.Size = new System.Drawing.Size(90, 20);
            this.lblAssigned.TabIndex = 19;
            this.lblAssigned.Text = "lblAssigned";
            // 
            // dgvActive
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvActive.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvActive.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvActive.ColumnHeadersHeight = 4;
            this.dgvActive.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvActive.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvActive.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvActive.Location = new System.Drawing.Point(12, 132);
            this.dgvActive.Name = "dgvActive";
            this.dgvActive.RowHeadersVisible = false;
            this.dgvActive.RowHeadersWidth = 62;
            this.dgvActive.RowTemplate.Height = 28;
            this.dgvActive.Size = new System.Drawing.Size(907, 254);
            this.dgvActive.TabIndex = 17;
            this.dgvActive.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvActive.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvActive.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvActive.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvActive.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvActive.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvActive.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvActive.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvActive.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvActive.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.dgvActive.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvActive.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvActive.ThemeStyle.HeaderStyle.Height = 4;
            this.dgvActive.ThemeStyle.ReadOnly = false;
            this.dgvActive.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvActive.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvActive.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.dgvActive.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvActive.ThemeStyle.RowsStyle.Height = 28;
            this.dgvActive.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvActive.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // dgvAll
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            this.dgvAll.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAll.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvAll.ColumnHeadersHeight = 4;
            this.dgvAll.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvAll.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvAll.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvAll.Location = new System.Drawing.Point(9, 420);
            this.dgvAll.Name = "dgvAll";
            this.dgvAll.RowHeadersVisible = false;
            this.dgvAll.RowHeadersWidth = 62;
            this.dgvAll.RowTemplate.Height = 28;
            this.dgvAll.Size = new System.Drawing.Size(910, 294);
            this.dgvAll.TabIndex = 16;
            this.dgvAll.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvAll.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvAll.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvAll.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvAll.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvAll.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvAll.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvAll.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvAll.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvAll.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.dgvAll.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvAll.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvAll.ThemeStyle.HeaderStyle.Height = 4;
            this.dgvAll.ThemeStyle.ReadOnly = false;
            this.dgvAll.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvAll.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvAll.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.dgvAll.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvAll.ThemeStyle.RowsStyle.Height = 28;
            this.dgvAll.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvAll.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // cmbStatus
            // 
            this.cmbStatus.BackColor = System.Drawing.Color.Transparent;
            this.cmbStatus.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbStatus.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbStatus.ItemHeight = 30;
            this.cmbStatus.Location = new System.Drawing.Point(514, 23);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(238, 36);
            this.cmbStatus.TabIndex = 15;
            // 
            // dtTo
            // 
            this.dtTo.Checked = true;
            this.dtTo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            this.dtTo.Location = new System.Drawing.Point(649, 90);
            this.dtTo.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtTo.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(200, 36);
            this.dtTo.TabIndex = 13;
            this.dtTo.Value = new System.DateTime(2025, 10, 23, 17, 28, 37, 210);
            // 
            // dtFrom
            // 
            this.dtFrom.Checked = true;
            this.dtFrom.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            this.dtFrom.Location = new System.Drawing.Point(443, 90);
            this.dtFrom.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtFrom.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(200, 36);
            this.dtFrom.TabIndex = 14;
            this.dtFrom.Value = new System.DateTime(2025, 10, 23, 17, 28, 37, 210);
            // 
            // btnSearchHistory
            // 
            this.btnSearchHistory.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSearchHistory.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnSearchHistory.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnSearchHistory.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnSearchHistory.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSearchHistory.ForeColor = System.Drawing.Color.White;
            this.btnSearchHistory.Location = new System.Drawing.Point(314, 23);
            this.btnSearchHistory.Name = "btnSearchHistory";
            this.btnSearchHistory.Size = new System.Drawing.Size(180, 45);
            this.btnSearchHistory.TabIndex = 10;
            this.btnSearchHistory.Text = "Tìm kiếm";
            // 
            // btnReceive
            // 
            this.btnReceive.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnReceive.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnReceive.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnReceive.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnReceive.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnReceive.ForeColor = System.Drawing.Color.White;
            this.btnReceive.Location = new System.Drawing.Point(128, 74);
            this.btnReceive.Name = "btnReceive";
            this.btnReceive.Size = new System.Drawing.Size(180, 45);
            this.btnReceive.TabIndex = 11;
            this.btnReceive.Text = "Nhận chuyến";
            // 
            // btnReload
            // 
            this.btnReload.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnReload.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnReload.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnReload.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnReload.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnReload.ForeColor = System.Drawing.Color.White;
            this.btnReload.Location = new System.Drawing.Point(128, 23);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(180, 45);
            this.btnReload.TabIndex = 12;
            this.btnReload.Text = "Tải lại";
            // 
            // ucMyShipments_Drv1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblHistory);
            this.Controls.Add(this.lblAssigned);
            this.Controls.Add(this.dgvActive);
            this.Controls.Add(this.dgvAll);
            this.Controls.Add(this.cmbStatus);
            this.Controls.Add(this.dtTo);
            this.Controls.Add(this.dtFrom);
            this.Controls.Add(this.btnSearchHistory);
            this.Controls.Add(this.btnReceive);
            this.Controls.Add(this.btnReload);
            this.Name = "ucMyShipments_Drv1";
            this.Size = new System.Drawing.Size(939, 747);
            ((System.ComponentModel.ISupportInitialize)(this.dgvActive)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAll)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHistory;
        private System.Windows.Forms.Label lblAssigned;
        private Guna.UI2.WinForms.Guna2DataGridView dgvActive;
        private Guna.UI2.WinForms.Guna2DataGridView dgvAll;
        private Guna.UI2.WinForms.Guna2ComboBox cmbStatus;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtTo;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtFrom;
        private Guna.UI2.WinForms.Guna2Button btnSearchHistory;
        private Guna.UI2.WinForms.Guna2Button btnReceive;
        private Guna.UI2.WinForms.Guna2Button btnReload;
    }
}
