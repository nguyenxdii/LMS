namespace LMS.GUI.Main
{
    partial class frmadmintest
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.button1 = new CSharp.Winform.UI.Button();
            this.button9 = new CSharp.Winform.UI.Button();
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(33)))), ((int)(((byte)(36)))));
            this.flowLayoutPanel1.Controls.Add(this.button1);
            this.flowLayoutPanel1.Controls.Add(this.button9);
            this.flowLayoutPanel1.Controls.Add(this.guna2Button1);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(289, 81);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(301, 557);
            this.flowLayoutPanel1.TabIndex = 11;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(24)))), ((int)(((byte)(29)))));
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Image = global::LMS.GUI.Properties.Resources.toolmini;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.Name = "button1";
            this.button1.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.button1.Size = new System.Drawing.Size(301, 58);
            this.button1.TabIndex = 2;
            this.button1.Text = "       Chức Năng";
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button1.UseVisualStyleBackColor = false;
            // 
            // button9
            // 
            this.button9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(33)))), ((int)(((byte)(36)))));
            this.button9.FlatAppearance.BorderSize = 0;
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button9.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button9.ForeColor = System.Drawing.Color.White;
            this.button9.Image = global::LMS.GUI.Properties.Resources.dot_mini;
            this.button9.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button9.Location = new System.Drawing.Point(0, 58);
            this.button9.Margin = new System.Windows.Forms.Padding(0);
            this.button9.Name = "button9";
            this.button9.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.button9.Size = new System.Drawing.Size(301, 58);
            this.button9.TabIndex = 18;
            this.button9.Text = "            Quản Lý Tuyến Đường";
            this.button9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button9.UseVisualStyleBackColor = false;
            // 
            // guna2Button1
            // 
            this.guna2Button1.CustomBorderThickness = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.guna2Button1.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button1.FillColor = System.Drawing.Color.Transparent;
            this.guna2Button1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2Button1.ForeColor = System.Drawing.Color.White;
            this.guna2Button1.HoverState.CustomBorderColor = System.Drawing.Color.Red;
            this.guna2Button1.HoverState.FillColor = System.Drawing.Color.Red;
            this.guna2Button1.Location = new System.Drawing.Point(3, 119);
            this.guna2Button1.Name = "guna2Button1";
            this.guna2Button1.Size = new System.Drawing.Size(298, 45);
            this.guna2Button1.TabIndex = 20;
            this.guna2Button1.Text = "guna2Button1";
            // 
            // frmadmintest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1478, 744);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "frmadmintest";
            this.Text = "frmadmintest";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private CSharp.Winform.UI.Button button1;
        private CSharp.Winform.UI.Button button9;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
    }
}