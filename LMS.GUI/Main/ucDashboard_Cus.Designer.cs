namespace LMS.GUI.Main
{
    partial class ucDashboard_Cus
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
            this.lblWelcome = new System.Windows.Forms.Label();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.lblInTransitCount = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCreateOrder = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            this.lblPendingCount = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.guna2Panel3 = new Guna.UI2.WinForms.Guna2Panel();
            this.lblCompletedCount = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.dgvRecentOrders = new Guna.UI2.WinForms.Guna2DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.guna2Panel1.SuspendLayout();
            this.guna2Panel2.SuspendLayout();
            this.guna2Panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecentOrders)).BeginInit();
            this.SuspendLayout();
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblWelcome.Location = new System.Drawing.Point(551, 18);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(97, 41);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "label1";
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.White;
            this.guna2Panel1.BorderColor = System.Drawing.Color.Black;
            this.guna2Panel1.BorderThickness = 1;
            this.guna2Panel1.Controls.Add(this.lblInTransitCount);
            this.guna2Panel1.Controls.Add(this.label2);
            this.guna2Panel1.Location = new System.Drawing.Point(451, 73);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(304, 127);
            this.guna2Panel1.TabIndex = 1;
            // 
            // lblInTransitCount
            // 
            this.lblInTransitCount.AutoSize = true;
            this.lblInTransitCount.Font = new System.Drawing.Font("Segoe UI", 17F);
            this.lblInTransitCount.Location = new System.Drawing.Point(3, 40);
            this.lblInTransitCount.Name = "lblInTransitCount";
            this.lblInTransitCount.Size = new System.Drawing.Size(109, 46);
            this.lblInTransitCount.TabIndex = 0;
            this.lblInTransitCount.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label2.Location = new System.Drawing.Point(64, 1);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(176, 28);
            this.label2.TabIndex = 0;
            this.label2.Text = "Đang vận chuyển";
            // 
            // btnCreateOrder
            // 
            this.btnCreateOrder.BorderColor = System.Drawing.Color.White;
            this.btnCreateOrder.BorderRadius = 15;
            this.btnCreateOrder.BorderThickness = 1;
            this.btnCreateOrder.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCreateOrder.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnCreateOrder.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnCreateOrder.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnCreateOrder.FillColor = System.Drawing.Color.Black;
            this.btnCreateOrder.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.btnCreateOrder.ForeColor = System.Drawing.Color.White;
            this.btnCreateOrder.Location = new System.Drawing.Point(107, 220);
            this.btnCreateOrder.Name = "btnCreateOrder";
            this.btnCreateOrder.Size = new System.Drawing.Size(989, 74);
            this.btnCreateOrder.TabIndex = 2;
            this.btnCreateOrder.Text = "ĐẶT ĐƠN NGAY";
            this.btnCreateOrder.Click += new System.EventHandler(this.btnCreateOrder_Click);
            // 
            // guna2Panel2
            // 
            this.guna2Panel2.BackColor = System.Drawing.Color.White;
            this.guna2Panel2.BorderColor = System.Drawing.Color.Black;
            this.guna2Panel2.BorderThickness = 1;
            this.guna2Panel2.Controls.Add(this.lblPendingCount);
            this.guna2Panel2.Controls.Add(this.label5);
            this.guna2Panel2.Location = new System.Drawing.Point(107, 73);
            this.guna2Panel2.Name = "guna2Panel2";
            this.guna2Panel2.Size = new System.Drawing.Size(304, 127);
            this.guna2Panel2.TabIndex = 1;
            // 
            // lblPendingCount
            // 
            this.lblPendingCount.AutoSize = true;
            this.lblPendingCount.Font = new System.Drawing.Font("Segoe UI", 17F);
            this.lblPendingCount.Location = new System.Drawing.Point(3, 40);
            this.lblPendingCount.Name = "lblPendingCount";
            this.lblPendingCount.Size = new System.Drawing.Size(109, 46);
            this.lblPendingCount.TabIndex = 0;
            this.lblPendingCount.Text = "label1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label5.Location = new System.Drawing.Point(74, 1);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(156, 28);
            this.label5.TabIndex = 0;
            this.label5.Text = "Đang chờ xử lý";
            // 
            // guna2Panel3
            // 
            this.guna2Panel3.BackColor = System.Drawing.Color.White;
            this.guna2Panel3.BorderColor = System.Drawing.Color.Black;
            this.guna2Panel3.BorderThickness = 1;
            this.guna2Panel3.Controls.Add(this.lblCompletedCount);
            this.guna2Panel3.Controls.Add(this.label7);
            this.guna2Panel3.Location = new System.Drawing.Point(792, 73);
            this.guna2Panel3.Name = "guna2Panel3";
            this.guna2Panel3.Size = new System.Drawing.Size(304, 127);
            this.guna2Panel3.TabIndex = 1;
            // 
            // lblCompletedCount
            // 
            this.lblCompletedCount.AutoSize = true;
            this.lblCompletedCount.Font = new System.Drawing.Font("Segoe UI", 17F);
            this.lblCompletedCount.Location = new System.Drawing.Point(3, 40);
            this.lblCompletedCount.Name = "lblCompletedCount";
            this.lblCompletedCount.Size = new System.Drawing.Size(109, 46);
            this.lblCompletedCount.TabIndex = 0;
            this.lblCompletedCount.Text = "label1";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label7.Location = new System.Drawing.Point(76, 1);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(152, 28);
            this.label7.TabIndex = 0;
            this.label7.Text = "Đã hoàn thành";
            // 
            // dgvRecentOrders
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvRecentOrders.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRecentOrders.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRecentOrders.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvRecentOrders.ColumnHeadersHeight = 4;
            this.dgvRecentOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRecentOrders.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvRecentOrders.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvRecentOrders.Location = new System.Drawing.Point(107, 344);
            this.dgvRecentOrders.Name = "dgvRecentOrders";
            this.dgvRecentOrders.RowHeadersVisible = false;
            this.dgvRecentOrders.RowHeadersWidth = 62;
            this.dgvRecentOrders.RowTemplate.Height = 28;
            this.dgvRecentOrders.Size = new System.Drawing.Size(989, 260);
            this.dgvRecentOrders.TabIndex = 3;
            this.dgvRecentOrders.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvRecentOrders.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvRecentOrders.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvRecentOrders.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvRecentOrders.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvRecentOrders.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvRecentOrders.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvRecentOrders.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvRecentOrders.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvRecentOrders.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.dgvRecentOrders.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvRecentOrders.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvRecentOrders.ThemeStyle.HeaderStyle.Height = 4;
            this.dgvRecentOrders.ThemeStyle.ReadOnly = false;
            this.dgvRecentOrders.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvRecentOrders.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvRecentOrders.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.dgvRecentOrders.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvRecentOrders.ThemeStyle.RowsStyle.Height = 28;
            this.dgvRecentOrders.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvRecentOrders.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label3.Location = new System.Drawing.Point(102, 302);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(212, 28);
            this.label3.TabIndex = 0;
            this.label3.Text = "5 Đơn Hàng Gần Đây";
            // 
            // ucDashboard_Cus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvRecentOrders);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCreateOrder);
            this.Controls.Add(this.guna2Panel3);
            this.Controls.Add(this.guna2Panel2);
            this.Controls.Add(this.guna2Panel1);
            this.Controls.Add(this.lblWelcome);
            this.Name = "ucDashboard_Cus";
            this.Size = new System.Drawing.Size(1199, 657);
            this.guna2Panel1.ResumeLayout(false);
            this.guna2Panel1.PerformLayout();
            this.guna2Panel2.ResumeLayout(false);
            this.guna2Panel2.PerformLayout();
            this.guna2Panel3.ResumeLayout(false);
            this.guna2Panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecentOrders)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblWelcome;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2Button btnCreateOrder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblInTransitCount;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
        private System.Windows.Forms.Label lblPendingCount;
        private System.Windows.Forms.Label label5;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel3;
        private System.Windows.Forms.Label lblCompletedCount;
        private System.Windows.Forms.Label label7;
        private Guna.UI2.WinForms.Guna2DataGridView dgvRecentOrders;
        private System.Windows.Forms.Label label3;
    }
}
