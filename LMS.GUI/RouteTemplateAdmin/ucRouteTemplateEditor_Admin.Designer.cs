namespace LMS.GUI.RouteTemplateAdmin
{
    partial class ucRouteTemplateEditor_Admin
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
            this.btnSave = new Guna.UI2.WinForms.Guna2Button();
            this.btnMoveDown = new Guna.UI2.WinForms.Guna2Button();
            this.btnMoveUp = new Guna.UI2.WinForms.Guna2Button();
            this.dgvSelectedStops = new CSharp.Winform.UI.DataGridView();
            this.dgvAvailableStops = new CSharp.Winform.UI.DataGridView();
            this.groupBox1 = new CSharp.Winform.UI.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTemplateName = new Guna.UI2.WinForms.Guna2TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbFromZone = new Guna.UI2.WinForms.Guna2ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbStopWarehouse = new Guna.UI2.WinForms.Guna2ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbToZone = new Guna.UI2.WinForms.Guna2ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbStopZone = new Guna.UI2.WinForms.Guna2ComboBox();
            this.pnlTop = new CSharp.Winform.UI.Panel();
            this.guna2ControlBox3 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.lblTitle = new CSharp.Winform.UI.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectedStops)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAvailableStops)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnMoveDown);
            this.panel1.Controls.Add(this.btnMoveUp);
            this.panel1.Controls.Add(this.dgvSelectedStops);
            this.panel1.Controls.Add(this.dgvAvailableStops);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.pnlTop);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1384, 763);
            this.panel1.TabIndex = 9;
            // 
            // btnSave
            // 
            this.btnSave.BorderRadius = 15;
            this.btnSave.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSave.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnSave.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnSave.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnSave.FillColor = System.Drawing.Color.Black;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(1097, 290);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(180, 45);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "Lưu Tuyến đường";
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.BorderRadius = 15;
            this.btnMoveDown.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnMoveDown.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnMoveDown.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnMoveDown.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnMoveDown.FillColor = System.Drawing.Color.Black;
            this.btnMoveDown.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnMoveDown.ForeColor = System.Drawing.Color.White;
            this.btnMoveDown.Location = new System.Drawing.Point(911, 290);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(180, 45);
            this.btnMoveDown.TabIndex = 13;
            this.btnMoveDown.Text = "Xuống";
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.BorderRadius = 15;
            this.btnMoveUp.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnMoveUp.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnMoveUp.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnMoveUp.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnMoveUp.FillColor = System.Drawing.Color.Black;
            this.btnMoveUp.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnMoveUp.ForeColor = System.Drawing.Color.White;
            this.btnMoveUp.Location = new System.Drawing.Point(725, 290);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(180, 45);
            this.btnMoveUp.TabIndex = 14;
            this.btnMoveUp.Text = "Lên";
            // 
            // dgvSelectedStops
            // 
            this.dgvSelectedStops.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSelectedStops.Location = new System.Drawing.Point(725, 341);
            this.dgvSelectedStops.Name = "dgvSelectedStops";
            this.dgvSelectedStops.RowHeadersWidth = 62;
            this.dgvSelectedStops.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.dgvSelectedStops.RowTemplate.Height = 28;
            this.dgvSelectedStops.Size = new System.Drawing.Size(646, 396);
            this.dgvSelectedStops.TabIndex = 12;
            // 
            // dgvAvailableStops
            // 
            this.dgvAvailableStops.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAvailableStops.Location = new System.Drawing.Point(14, 341);
            this.dgvAvailableStops.Name = "dgvAvailableStops";
            this.dgvAvailableStops.RowHeadersWidth = 62;
            this.dgvAvailableStops.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.dgvAvailableStops.RowTemplate.Height = 28;
            this.dgvAvailableStops.Size = new System.Drawing.Size(646, 396);
            this.dgvAvailableStops.TabIndex = 12;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtTemplateName);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cmbFromZone);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cmbStopWarehouse);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbToZone);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbStopZone);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.groupBox1.Location = new System.Drawing.Point(14, 55);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(398, 260);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Thông tin Tuyến đường";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label5.Location = new System.Drawing.Point(38, 208);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 25);
            this.label5.TabIndex = 13;
            this.label5.Text = "Kho Nhận";
            // 
            // txtTemplateName
            // 
            this.txtTemplateName.BorderRadius = 15;
            this.txtTemplateName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTemplateName.DefaultText = "";
            this.txtTemplateName.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtTemplateName.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtTemplateName.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTemplateName.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTemplateName.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTemplateName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtTemplateName.ForeColor = System.Drawing.Color.Black;
            this.txtTemplateName.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTemplateName.Location = new System.Drawing.Point(138, 30);
            this.txtTemplateName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTemplateName.Name = "txtTemplateName";
            this.txtTemplateName.PlaceholderText = "";
            this.txtTemplateName.SelectedText = "";
            this.txtTemplateName.Size = new System.Drawing.Size(236, 36);
            this.txtTemplateName.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label4.Location = new System.Drawing.Point(29, 166);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 25);
            this.label4.TabIndex = 13;
            this.label4.Text = "Vùng Nhận";
            // 
            // cmbFromZone
            // 
            this.cmbFromZone.BackColor = System.Drawing.Color.Transparent;
            this.cmbFromZone.BorderRadius = 15;
            this.cmbFromZone.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbFromZone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFromZone.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbFromZone.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbFromZone.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.cmbFromZone.ForeColor = System.Drawing.Color.Black;
            this.cmbFromZone.ItemHeight = 30;
            this.cmbFromZone.Location = new System.Drawing.Point(138, 74);
            this.cmbFromZone.Name = "cmbFromZone";
            this.cmbFromZone.Size = new System.Drawing.Size(236, 36);
            this.cmbFromZone.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label3.Location = new System.Drawing.Point(60, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 25);
            this.label3.TabIndex = 13;
            this.label3.Text = "Kho Đi";
            // 
            // cmbStopWarehouse
            // 
            this.cmbStopWarehouse.BackColor = System.Drawing.Color.Transparent;
            this.cmbStopWarehouse.BorderRadius = 15;
            this.cmbStopWarehouse.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbStopWarehouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStopWarehouse.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbStopWarehouse.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbStopWarehouse.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.cmbStopWarehouse.ForeColor = System.Drawing.Color.Black;
            this.cmbStopWarehouse.ItemHeight = 30;
            this.cmbStopWarehouse.Location = new System.Drawing.Point(138, 208);
            this.cmbStopWarehouse.Name = "cmbStopWarehouse";
            this.cmbStopWarehouse.Size = new System.Drawing.Size(236, 36);
            this.cmbStopWarehouse.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label2.Location = new System.Drawing.Point(51, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 25);
            this.label2.TabIndex = 13;
            this.label2.Text = "Vùng Đi";
            // 
            // cmbToZone
            // 
            this.cmbToZone.BackColor = System.Drawing.Color.Transparent;
            this.cmbToZone.BorderRadius = 15;
            this.cmbToZone.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbToZone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbToZone.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbToZone.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbToZone.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.cmbToZone.ForeColor = System.Drawing.Color.Black;
            this.cmbToZone.ItemHeight = 30;
            this.cmbToZone.Location = new System.Drawing.Point(138, 124);
            this.cmbToZone.Name = "cmbToZone";
            this.cmbToZone.Size = new System.Drawing.Size(236, 36);
            this.cmbToZone.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label1.Location = new System.Drawing.Point(22, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 25);
            this.label1.TabIndex = 13;
            this.label1.Text = "Tên Chuyến";
            // 
            // cmbStopZone
            // 
            this.cmbStopZone.BackColor = System.Drawing.Color.Transparent;
            this.cmbStopZone.BorderRadius = 15;
            this.cmbStopZone.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbStopZone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStopZone.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbStopZone.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbStopZone.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.cmbStopZone.ForeColor = System.Drawing.Color.Black;
            this.cmbStopZone.ItemHeight = 30;
            this.cmbStopZone.Location = new System.Drawing.Point(138, 166);
            this.cmbStopZone.Name = "cmbStopZone";
            this.cmbStopZone.Size = new System.Drawing.Size(236, 36);
            this.cmbStopZone.TabIndex = 11;
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
            this.pnlTop.Size = new System.Drawing.Size(1384, 49);
            this.pnlTop.TabIndex = 9;
            // 
            // guna2ControlBox3
            // 
            this.guna2ControlBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2ControlBox3.BorderRadius = 5;
            this.guna2ControlBox3.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(24)))), ((int)(((byte)(29)))));
            this.guna2ControlBox3.IconColor = System.Drawing.Color.White;
            this.guna2ControlBox3.Location = new System.Drawing.Point(1332, 5);
            this.guna2ControlBox3.Name = "guna2ControlBox3";
            this.guna2ControlBox3.Size = new System.Drawing.Size(46, 35);
            this.guna2ControlBox3.TabIndex = 5;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(490, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(335, 38);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Thêm Tuyến đường Mới";
            // 
            // ucRouteTemplateEditor_Admin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.panel1);
            this.Name = "ucRouteTemplateEditor_Admin";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Size = new System.Drawing.Size(1388, 767);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectedStops)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAvailableStops)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private CSharp.Winform.UI.Panel panel1;
        private CSharp.Winform.UI.Panel pnlTop;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox3;
        private CSharp.Winform.UI.Label lblTitle;
        private CSharp.Winform.UI.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2TextBox txtTemplateName;
        private Guna.UI2.WinForms.Guna2ComboBox cmbFromZone;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private Guna.UI2.WinForms.Guna2ComboBox cmbStopZone;
        private Guna.UI2.WinForms.Guna2ComboBox cmbToZone;
        private Guna.UI2.WinForms.Guna2ComboBox cmbStopWarehouse;
        private CSharp.Winform.UI.DataGridView dgvAvailableStops;
        private CSharp.Winform.UI.DataGridView dgvSelectedStops;
        private Guna.UI2.WinForms.Guna2Button btnSave;
        private Guna.UI2.WinForms.Guna2Button btnMoveDown;
        private Guna.UI2.WinForms.Guna2Button btnMoveUp;
    }
}
