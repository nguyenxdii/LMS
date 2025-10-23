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
            this.dgvStops = new Guna.UI2.WinForms.Guna2DataGridView();
            this.btnComplete = new Guna.UI2.WinForms.Guna2Button();
            this.btnReload = new Guna.UI2.WinForms.Guna2Button();
            this.btnArrive = new Guna.UI2.WinForms.Guna2Button();
            this.btnDepart = new Guna.UI2.WinForms.Guna2Button();
            this.btnReceive = new Guna.UI2.WinForms.Guna2Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblCurrentSeq = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblRoute = new System.Windows.Forms.Label();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.lblOrderNo = new System.Windows.Forms.Label();
            this.lblShipmentNo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStops)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvStops
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvStops.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvStops.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvStops.ColumnHeadersHeight = 4;
            this.dgvStops.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvStops.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvStops.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvStops.Location = new System.Drawing.Point(24, 248);
            this.dgvStops.Name = "dgvStops";
            this.dgvStops.RowHeadersVisible = false;
            this.dgvStops.RowHeadersWidth = 62;
            this.dgvStops.RowTemplate.Height = 28;
            this.dgvStops.Size = new System.Drawing.Size(806, 381);
            this.dgvStops.TabIndex = 15;
            this.dgvStops.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvStops.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvStops.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvStops.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvStops.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvStops.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvStops.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvStops.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvStops.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvStops.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.dgvStops.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvStops.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvStops.ThemeStyle.HeaderStyle.Height = 4;
            this.dgvStops.ThemeStyle.ReadOnly = false;
            this.dgvStops.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvStops.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvStops.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.dgvStops.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvStops.ThemeStyle.RowsStyle.Height = 28;
            this.dgvStops.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvStops.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // btnComplete
            // 
            this.btnComplete.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnComplete.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnComplete.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnComplete.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnComplete.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnComplete.ForeColor = System.Drawing.Color.Black;
            this.btnComplete.Location = new System.Drawing.Point(788, 58);
            this.btnComplete.Name = "btnComplete";
            this.btnComplete.Size = new System.Drawing.Size(180, 45);
            this.btnComplete.TabIndex = 10;
            this.btnComplete.Text = "btnComplete";
            // 
            // btnReload
            // 
            this.btnReload.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnReload.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnReload.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnReload.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnReload.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnReload.ForeColor = System.Drawing.Color.Black;
            this.btnReload.Location = new System.Drawing.Point(602, 109);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(180, 45);
            this.btnReload.TabIndex = 11;
            this.btnReload.Text = "btnReload";
            // 
            // btnArrive
            // 
            this.btnArrive.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnArrive.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnArrive.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnArrive.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnArrive.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnArrive.ForeColor = System.Drawing.Color.Black;
            this.btnArrive.Location = new System.Drawing.Point(602, 58);
            this.btnArrive.Name = "btnArrive";
            this.btnArrive.Size = new System.Drawing.Size(180, 45);
            this.btnArrive.TabIndex = 12;
            this.btnArrive.Text = "btnArrive";
            // 
            // btnDepart
            // 
            this.btnDepart.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnDepart.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnDepart.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnDepart.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnDepart.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnDepart.ForeColor = System.Drawing.Color.Black;
            this.btnDepart.Location = new System.Drawing.Point(788, 7);
            this.btnDepart.Name = "btnDepart";
            this.btnDepart.Size = new System.Drawing.Size(180, 45);
            this.btnDepart.TabIndex = 13;
            this.btnDepart.Text = "btnDepart";
            // 
            // btnReceive
            // 
            this.btnReceive.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnReceive.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnReceive.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnReceive.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnReceive.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnReceive.ForeColor = System.Drawing.Color.Black;
            this.btnReceive.Location = new System.Drawing.Point(602, 9);
            this.btnReceive.Name = "btnReceive";
            this.btnReceive.Size = new System.Drawing.Size(180, 45);
            this.btnReceive.TabIndex = 14;
            this.btnReceive.Text = "btnReceive";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.4403F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.5597F));
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblCurrentSeq, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblStatus, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblRoute, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblCustomer, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblOrderNo, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblShipmentNo, 1, 0);
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.tableLayoutPanel1.ForeColor = System.Drawing.Color.Black;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(14, 20);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(538, 196);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(5, 162);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(124, 32);
            this.label6.TabIndex = 40;
            this.label6.Text = "Mô tả";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(5, 130);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(124, 30);
            this.label5.TabIndex = 39;
            this.label5.Text = "Địa chỉ lấy";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(5, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 30);
            this.label4.TabIndex = 38;
            this.label4.Text = "Kho nhận";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(5, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 30);
            this.label3.TabIndex = 37;
            this.label3.Text = "Kho gửi";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(5, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 30);
            this.label2.TabIndex = 36;
            this.label2.Text = "Tên khách";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(5, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 30);
            this.label1.TabIndex = 35;
            this.label1.Text = "Mã đơn";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCurrentSeq
            // 
            this.lblCurrentSeq.AutoSize = true;
            this.lblCurrentSeq.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCurrentSeq.ForeColor = System.Drawing.Color.Black;
            this.lblCurrentSeq.Location = new System.Drawing.Point(137, 162);
            this.lblCurrentSeq.Name = "lblCurrentSeq";
            this.lblCurrentSeq.Size = new System.Drawing.Size(396, 32);
            this.lblCurrentSeq.TabIndex = 30;
            this.lblCurrentSeq.Text = "Id";
            this.lblCurrentSeq.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.ForeColor = System.Drawing.Color.Black;
            this.lblStatus.Location = new System.Drawing.Point(137, 130);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(396, 30);
            this.lblStatus.TabIndex = 29;
            this.lblStatus.Text = "Id";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRoute
            // 
            this.lblRoute.AutoSize = true;
            this.lblRoute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRoute.ForeColor = System.Drawing.Color.Black;
            this.lblRoute.Location = new System.Drawing.Point(137, 98);
            this.lblRoute.Name = "lblRoute";
            this.lblRoute.Size = new System.Drawing.Size(396, 30);
            this.lblRoute.TabIndex = 28;
            this.lblRoute.Text = "Id";
            this.lblRoute.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCustomer
            // 
            this.lblCustomer.AutoSize = true;
            this.lblCustomer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCustomer.ForeColor = System.Drawing.Color.Black;
            this.lblCustomer.Location = new System.Drawing.Point(137, 66);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(396, 30);
            this.lblCustomer.TabIndex = 27;
            this.lblCustomer.Text = "Id";
            this.lblCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOrderNo
            // 
            this.lblOrderNo.AutoSize = true;
            this.lblOrderNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOrderNo.ForeColor = System.Drawing.Color.Black;
            this.lblOrderNo.Location = new System.Drawing.Point(137, 34);
            this.lblOrderNo.Name = "lblOrderNo";
            this.lblOrderNo.Size = new System.Drawing.Size(396, 30);
            this.lblOrderNo.TabIndex = 26;
            this.lblOrderNo.Text = "Id";
            this.lblOrderNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblShipmentNo
            // 
            this.lblShipmentNo.AutoSize = true;
            this.lblShipmentNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblShipmentNo.ForeColor = System.Drawing.Color.Black;
            this.lblShipmentNo.Location = new System.Drawing.Point(137, 2);
            this.lblShipmentNo.Name = "lblShipmentNo";
            this.lblShipmentNo.Size = new System.Drawing.Size(396, 30);
            this.lblShipmentNo.TabIndex = 25;
            this.lblShipmentNo.Text = "Id";
            this.lblShipmentNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ucShipmentDetail_Drv1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvStops);
            this.Controls.Add(this.btnComplete);
            this.Controls.Add(this.btnReload);
            this.Controls.Add(this.btnArrive);
            this.Controls.Add(this.btnDepart);
            this.Controls.Add(this.btnReceive);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ucShipmentDetail_Drv1";
            this.Size = new System.Drawing.Size(1008, 783);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStops)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2DataGridView dgvStops;
        private Guna.UI2.WinForms.Guna2Button btnComplete;
        private Guna.UI2.WinForms.Guna2Button btnReload;
        private Guna.UI2.WinForms.Guna2Button btnArrive;
        private Guna.UI2.WinForms.Guna2Button btnDepart;
        private Guna.UI2.WinForms.Guna2Button btnReceive;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblCurrentSeq;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblRoute;
        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.Label lblOrderNo;
        private System.Windows.Forms.Label lblShipmentNo;
    }
}
