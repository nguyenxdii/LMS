namespace LMS.GUI.VehicleAdmin
{
    partial class ucVehicleSearch_Admin
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
            this.dgvSearchResults = new Guna.UI2.WinForms.Guna2DataGridView();
            this.btnReset = new Guna.UI2.WinForms.Guna2Button();
            this.btnDoSearch = new Guna.UI2.WinForms.Guna2Button();
            this.guna2ControlBox3 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.lblTitle = new CSharp.Winform.UI.Label();
            this.pnlTop = new CSharp.Winform.UI.Panel();
            this.panel1 = new CSharp.Winform.UI.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label7 = new CSharp.Winform.UI.Label();
            this.txtSearchPlateNo = new Guna.UI2.WinForms.Guna2TextBox();
            this.label3 = new CSharp.Winform.UI.Label();
            this.cmbSearchType = new Guna.UI2.WinForms.Guna2ComboBox();
            this.label2 = new CSharp.Winform.UI.Label();
            this.cmbSearchStatus = new Guna.UI2.WinForms.Guna2ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearchResults)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvSearchResults
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvSearchResults.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSearchResults.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 8.25F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSearchResults.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSearchResults.ColumnHeadersHeight = 4;
            this.dgvSearchResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 8.25F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSearchResults.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvSearchResults.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvSearchResults.Location = new System.Drawing.Point(41, 249);
            this.dgvSearchResults.Name = "dgvSearchResults";
            this.dgvSearchResults.RowHeadersVisible = false;
            this.dgvSearchResults.RowHeadersWidth = 62;
            this.dgvSearchResults.RowTemplate.Height = 28;
            this.dgvSearchResults.Size = new System.Drawing.Size(907, 290);
            this.dgvSearchResults.TabIndex = 15;
            this.dgvSearchResults.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvSearchResults.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvSearchResults.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvSearchResults.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvSearchResults.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvSearchResults.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvSearchResults.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvSearchResults.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvSearchResults.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvSearchResults.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.dgvSearchResults.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvSearchResults.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvSearchResults.ThemeStyle.HeaderStyle.Height = 4;
            this.dgvSearchResults.ThemeStyle.ReadOnly = false;
            this.dgvSearchResults.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvSearchResults.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvSearchResults.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.dgvSearchResults.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvSearchResults.ThemeStyle.RowsStyle.Height = 28;
            this.dgvSearchResults.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvSearchResults.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // btnReset
            // 
            this.btnReset.BorderRadius = 15;
            this.btnReset.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnReset.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnReset.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnReset.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnReset.FillColor = System.Drawing.Color.Black;
            this.btnReset.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnReset.ForeColor = System.Drawing.Color.White;
            this.btnReset.Location = new System.Drawing.Point(729, 131);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(180, 45);
            this.btnReset.TabIndex = 13;
            this.btnReset.Text = "Tải lại";
            // 
            // btnDoSearch
            // 
            this.btnDoSearch.BorderRadius = 15;
            this.btnDoSearch.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnDoSearch.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnDoSearch.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnDoSearch.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnDoSearch.FillColor = System.Drawing.Color.Black;
            this.btnDoSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnDoSearch.ForeColor = System.Drawing.Color.White;
            this.btnDoSearch.Location = new System.Drawing.Point(729, 80);
            this.btnDoSearch.Name = "btnDoSearch";
            this.btnDoSearch.Size = new System.Drawing.Size(180, 45);
            this.btnDoSearch.TabIndex = 14;
            this.btnDoSearch.Text = "T. Kiếm";
            // 
            // guna2ControlBox3
            // 
            this.guna2ControlBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2ControlBox3.BorderRadius = 5;
            this.guna2ControlBox3.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(24)))), ((int)(((byte)(29)))));
            this.guna2ControlBox3.IconColor = System.Drawing.Color.White;
            this.guna2ControlBox3.Location = new System.Drawing.Point(915, 8);
            this.guna2ControlBox3.Name = "guna2ControlBox3";
            this.guna2ControlBox3.Size = new System.Drawing.Size(61, 39);
            this.guna2ControlBox3.TabIndex = 4;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.lblTitle.Location = new System.Drawing.Point(13, 18);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(54, 21);
            this.lblTitle.TabIndex = 85;
            this.lblTitle.Text = "label4";
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Controls.Add(this.guna2ControlBox3);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(984, 58);
            this.pnlTop.TabIndex = 17;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.pnlTop);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.dgvSearchResults);
            this.panel1.Controls.Add(this.btnReset);
            this.panel1.Controls.Add(this.btnDoSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(984, 685);
            this.panel1.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtSearchPlateNo);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cmbSearchType);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbSearchStatus);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.groupBox1.Location = new System.Drawing.Point(282, 80);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(430, 163);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tìm Kiếm";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label7.Location = new System.Drawing.Point(25, 32);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(96, 25);
            this.label7.TabIndex = 22;
            this.label7.Text = "Biển Số Xe";
            // 
            // txtSearchPlateNo
            // 
            this.txtSearchPlateNo.BorderRadius = 15;
            this.txtSearchPlateNo.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSearchPlateNo.DefaultText = "";
            this.txtSearchPlateNo.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtSearchPlateNo.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtSearchPlateNo.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtSearchPlateNo.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtSearchPlateNo.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSearchPlateNo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtSearchPlateNo.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSearchPlateNo.Location = new System.Drawing.Point(128, 32);
            this.txtSearchPlateNo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSearchPlateNo.Name = "txtSearchPlateNo";
            this.txtSearchPlateNo.PlaceholderText = "";
            this.txtSearchPlateNo.SelectedText = "";
            this.txtSearchPlateNo.Size = new System.Drawing.Size(270, 36);
            this.txtSearchPlateNo.TabIndex = 20;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label3.Location = new System.Drawing.Point(29, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 25);
            this.label3.TabIndex = 22;
            this.label3.Text = "Trạng Thái";
            // 
            // cmbSearchType
            // 
            this.cmbSearchType.BackColor = System.Drawing.Color.Transparent;
            this.cmbSearchType.BorderRadius = 15;
            this.cmbSearchType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbSearchType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSearchType.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbSearchType.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbSearchType.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.cmbSearchType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbSearchType.ItemHeight = 30;
            this.cmbSearchType.Location = new System.Drawing.Point(128, 76);
            this.cmbSearchType.Name = "cmbSearchType";
            this.cmbSearchType.Size = new System.Drawing.Size(270, 36);
            this.cmbSearchType.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label2.Location = new System.Drawing.Point(52, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 25);
            this.label2.TabIndex = 22;
            this.label2.Text = "Loại Xe";
            // 
            // cmbSearchStatus
            // 
            this.cmbSearchStatus.BackColor = System.Drawing.Color.Transparent;
            this.cmbSearchStatus.BorderRadius = 15;
            this.cmbSearchStatus.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbSearchStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSearchStatus.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbSearchStatus.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbSearchStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.cmbSearchStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbSearchStatus.ItemHeight = 30;
            this.cmbSearchStatus.Location = new System.Drawing.Point(129, 118);
            this.cmbSearchStatus.Name = "cmbSearchStatus";
            this.cmbSearchStatus.Size = new System.Drawing.Size(270, 36);
            this.cmbSearchStatus.TabIndex = 14;
            // 
            // ucVehicleSearch_Admin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.panel1);
            this.Name = "ucVehicleSearch_Admin";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Size = new System.Drawing.Size(988, 689);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearchResults)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2DataGridView dgvSearchResults;
        private Guna.UI2.WinForms.Guna2Button btnReset;
        private Guna.UI2.WinForms.Guna2Button btnDoSearch;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox3;
        private CSharp.Winform.UI.Label lblTitle;
        private CSharp.Winform.UI.Panel pnlTop;
        private CSharp.Winform.UI.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private CSharp.Winform.UI.Label label7;
        private Guna.UI2.WinForms.Guna2TextBox txtSearchPlateNo;
        private CSharp.Winform.UI.Label label3;
        private Guna.UI2.WinForms.Guna2ComboBox cmbSearchType;
        private CSharp.Winform.UI.Label label2;
        private Guna.UI2.WinForms.Guna2ComboBox cmbSearchStatus;
    }
}
