namespace LMS.GUI.VehicleAdmin
{
    partial class ucVehicle_Admin
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
            this.dgvVehicles = new Guna.UI2.WinForms.Guna2DataGridView();
            this.btnSearch = new Guna.UI2.WinForms.Guna2Button();
            this.btnDelete = new Guna.UI2.WinForms.Guna2Button();
            this.btnEdit = new Guna.UI2.WinForms.Guna2Button();
            this.btnAdd = new Guna.UI2.WinForms.Guna2Button();
            this.btnReload = new Guna.UI2.WinForms.Guna2Button();
            this.btnAssignDriver = new Guna.UI2.WinForms.Guna2Button();
            this.btnUnassignDriver = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVehicles)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvVehicles
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvVehicles.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvVehicles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvVehicles.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvVehicles.ColumnHeadersHeight = 4;
            this.dgvVehicles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvVehicles.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvVehicles.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvVehicles.Location = new System.Drawing.Point(44, 82);
            this.dgvVehicles.Name = "dgvVehicles";
            this.dgvVehicles.RowHeadersVisible = false;
            this.dgvVehicles.RowHeadersWidth = 62;
            this.dgvVehicles.RowTemplate.Height = 28;
            this.dgvVehicles.Size = new System.Drawing.Size(917, 524);
            this.dgvVehicles.TabIndex = 34;
            this.dgvVehicles.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvVehicles.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvVehicles.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvVehicles.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvVehicles.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvVehicles.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvVehicles.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvVehicles.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvVehicles.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvVehicles.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.dgvVehicles.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvVehicles.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvVehicles.ThemeStyle.HeaderStyle.Height = 4;
            this.dgvVehicles.ThemeStyle.ReadOnly = false;
            this.dgvVehicles.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvVehicles.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvVehicles.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.dgvVehicles.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvVehicles.ThemeStyle.RowsStyle.Height = 28;
            this.dgvVehicles.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvVehicles.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // btnSearch
            // 
            this.btnSearch.BorderRadius = 15;
            this.btnSearch.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSearch.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnSearch.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnSearch.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnSearch.FillColor = System.Drawing.Color.Black;
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(993, 294);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(169, 45);
            this.btnSearch.TabIndex = 30;
            this.btnSearch.Text = "Tìm Kiếm";
            // 
            // btnDelete
            // 
            this.btnDelete.BorderRadius = 15;
            this.btnDelete.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnDelete.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnDelete.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnDelete.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnDelete.FillColor = System.Drawing.Color.Black;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(993, 214);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(169, 45);
            this.btnDelete.TabIndex = 32;
            this.btnDelete.Text = "Xóa";
            // 
            // btnEdit
            // 
            this.btnEdit.BorderRadius = 15;
            this.btnEdit.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnEdit.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnEdit.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnEdit.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnEdit.FillColor = System.Drawing.Color.Black;
            this.btnEdit.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.Location = new System.Drawing.Point(993, 138);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(169, 45);
            this.btnEdit.TabIndex = 31;
            this.btnEdit.Text = "Sửa";
            // 
            // btnAdd
            // 
            this.btnAdd.BorderRadius = 15;
            this.btnAdd.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnAdd.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnAdd.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnAdd.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnAdd.FillColor = System.Drawing.Color.Black;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(993, 58);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(169, 45);
            this.btnAdd.TabIndex = 33;
            this.btnAdd.Text = "Thêm";
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
            this.btnReload.Location = new System.Drawing.Point(993, 371);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(169, 45);
            this.btnReload.TabIndex = 31;
            this.btnReload.Text = "Tải Lại";
            // 
            // btnAssignDriver
            // 
            this.btnAssignDriver.BorderRadius = 15;
            this.btnAssignDriver.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnAssignDriver.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnAssignDriver.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnAssignDriver.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnAssignDriver.FillColor = System.Drawing.Color.Black;
            this.btnAssignDriver.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnAssignDriver.ForeColor = System.Drawing.Color.White;
            this.btnAssignDriver.Location = new System.Drawing.Point(993, 447);
            this.btnAssignDriver.Name = "btnAssignDriver";
            this.btnAssignDriver.Size = new System.Drawing.Size(169, 45);
            this.btnAssignDriver.TabIndex = 32;
            this.btnAssignDriver.Text = "Gán Tài xế";
            // 
            // btnUnassignDriver
            // 
            this.btnUnassignDriver.BorderRadius = 15;
            this.btnUnassignDriver.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnUnassignDriver.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnUnassignDriver.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnUnassignDriver.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnUnassignDriver.FillColor = System.Drawing.Color.Black;
            this.btnUnassignDriver.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnUnassignDriver.ForeColor = System.Drawing.Color.White;
            this.btnUnassignDriver.Location = new System.Drawing.Point(993, 527);
            this.btnUnassignDriver.Name = "btnUnassignDriver";
            this.btnUnassignDriver.Size = new System.Drawing.Size(169, 45);
            this.btnUnassignDriver.TabIndex = 30;
            this.btnUnassignDriver.Text = "Gỡ Tài xế";
            // 
            // ucVehicle_Admin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvVehicles);
            this.Controls.Add(this.btnUnassignDriver);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnAssignDriver);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnReload);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnAdd);
            this.Name = "ucVehicle_Admin";
            this.Size = new System.Drawing.Size(1199, 689);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVehicles)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2DataGridView dgvVehicles;
        private Guna.UI2.WinForms.Guna2Button btnSearch;
        private Guna.UI2.WinForms.Guna2Button btnDelete;
        private Guna.UI2.WinForms.Guna2Button btnEdit;
        private Guna.UI2.WinForms.Guna2Button btnAdd;
        private Guna.UI2.WinForms.Guna2Button btnReload;
        private Guna.UI2.WinForms.Guna2Button btnAssignDriver;
        private Guna.UI2.WinForms.Guna2Button btnUnassignDriver;
    }
}
