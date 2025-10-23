namespace LMS.GUI.OrderDriver
{
    partial class ucShipmentRun_Drv
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
            this.dgvRoute = new Guna.UI2.WinForms.Guna2DataGridView();
            this.lblPhase = new CSharp.Winform.UI.Label();
            this.lblFromTo = new CSharp.Winform.UI.Label();
            this.btnReload = new Guna.UI2.WinForms.Guna2Button();
            this.btnComplete = new Guna.UI2.WinForms.Guna2Button();
            this.btnArrive = new Guna.UI2.WinForms.Guna2Button();
            this.btnDepart = new Guna.UI2.WinForms.Guna2Button();
            this.lblHint = new CSharp.Winform.UI.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoute)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvRoute
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            this.dgvRoute.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRoute.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvRoute.ColumnHeadersHeight = 4;
            this.dgvRoute.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRoute.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvRoute.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvRoute.Location = new System.Drawing.Point(37, 224);
            this.dgvRoute.Name = "dgvRoute";
            this.dgvRoute.RowHeadersVisible = false;
            this.dgvRoute.RowHeadersWidth = 62;
            this.dgvRoute.RowTemplate.Height = 28;
            this.dgvRoute.Size = new System.Drawing.Size(690, 420);
            this.dgvRoute.TabIndex = 10;
            this.dgvRoute.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvRoute.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvRoute.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvRoute.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvRoute.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvRoute.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvRoute.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvRoute.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvRoute.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvRoute.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.dgvRoute.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvRoute.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvRoute.ThemeStyle.HeaderStyle.Height = 4;
            this.dgvRoute.ThemeStyle.ReadOnly = false;
            this.dgvRoute.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvRoute.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvRoute.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.dgvRoute.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvRoute.ThemeStyle.RowsStyle.Height = 28;
            this.dgvRoute.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvRoute.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // lblPhase
            // 
            this.lblPhase.AutoSize = true;
            this.lblPhase.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.lblPhase.Location = new System.Drawing.Point(33, 160);
            this.lblPhase.Name = "lblPhase";
            this.lblPhase.Size = new System.Drawing.Size(71, 21);
            this.lblPhase.TabIndex = 8;
            this.lblPhase.Text = "lblPhase";
            // 
            // lblFromTo
            // 
            this.lblFromTo.AutoSize = true;
            this.lblFromTo.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.lblFromTo.Location = new System.Drawing.Point(33, 126);
            this.lblFromTo.Name = "lblFromTo";
            this.lblFromTo.Size = new System.Drawing.Size(84, 21);
            this.lblFromTo.TabIndex = 9;
            this.lblFromTo.Text = "lblFromTo";
            // 
            // btnReload
            // 
            this.btnReload.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnReload.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnReload.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnReload.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnReload.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnReload.ForeColor = System.Drawing.Color.White;
            this.btnReload.Location = new System.Drawing.Point(223, 62);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(124, 45);
            this.btnReload.TabIndex = 3;
            this.btnReload.Text = "Quay Lai";
            // 
            // btnComplete
            // 
            this.btnComplete.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnComplete.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnComplete.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnComplete.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnComplete.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnComplete.ForeColor = System.Drawing.Color.White;
            this.btnComplete.Location = new System.Drawing.Point(37, 62);
            this.btnComplete.Name = "btnComplete";
            this.btnComplete.Size = new System.Drawing.Size(141, 45);
            this.btnComplete.TabIndex = 4;
            this.btnComplete.Text = "Hoàn thành";
            // 
            // btnArrive
            // 
            this.btnArrive.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnArrive.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnArrive.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnArrive.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnArrive.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnArrive.ForeColor = System.Drawing.Color.White;
            this.btnArrive.Location = new System.Drawing.Point(179, 11);
            this.btnArrive.Name = "btnArrive";
            this.btnArrive.Size = new System.Drawing.Size(149, 45);
            this.btnArrive.TabIndex = 5;
            this.btnArrive.Text = "btnArrive";
            // 
            // btnDepart
            // 
            this.btnDepart.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnDepart.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnDepart.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnDepart.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnDepart.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnDepart.ForeColor = System.Drawing.Color.White;
            this.btnDepart.Location = new System.Drawing.Point(37, 11);
            this.btnDepart.Name = "btnDepart";
            this.btnDepart.Size = new System.Drawing.Size(124, 45);
            this.btnDepart.TabIndex = 6;
            this.btnDepart.Text = "btnDepart";
            // 
            // lblHint
            // 
            this.lblHint.AutoSize = true;
            this.lblHint.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.lblHint.Location = new System.Drawing.Point(33, 190);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(57, 21);
            this.lblHint.TabIndex = 9;
            this.lblHint.Text = "lblHint";
            // 
            // ucShipmentRun_Drv1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvRoute);
            this.Controls.Add(this.lblPhase);
            this.Controls.Add(this.lblHint);
            this.Controls.Add(this.lblFromTo);
            this.Controls.Add(this.btnReload);
            this.Controls.Add(this.btnComplete);
            this.Controls.Add(this.btnArrive);
            this.Controls.Add(this.btnDepart);
            this.Name = "ucShipmentRun_Drv1";
            this.Size = new System.Drawing.Size(1047, 789);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoute)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2DataGridView dgvRoute;
        private CSharp.Winform.UI.Label lblPhase;
        private CSharp.Winform.UI.Label lblFromTo;
        private Guna.UI2.WinForms.Guna2Button btnReload;
        private Guna.UI2.WinForms.Guna2Button btnComplete;
        private Guna.UI2.WinForms.Guna2Button btnArrive;
        private Guna.UI2.WinForms.Guna2Button btnDepart;
        private CSharp.Winform.UI.Label lblHint;
    }
}
