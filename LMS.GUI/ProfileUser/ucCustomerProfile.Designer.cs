namespace LMS.GUI.ProfileCustomer
{
    partial class ucCustomerProfile
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucCustomerProfile));
            this.lblUsername = new System.Windows.Forms.Label();
            this.btnChangeAvatar = new Guna.UI2.WinForms.Guna2Button();
            this.picAvatar = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            this.txtFullName = new Guna.UI2.WinForms.Guna2TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPhone = new Guna.UI2.WinForms.Guna2TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtEmail = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtAddress = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtConfirmPassword = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtNewPassword = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtOldPassword = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtUsername = new Guna.UI2.WinForms.Guna2TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.chkShowPassword = new CSharp.Winform.UI.CheckBox();
            this.btnChangePassword = new Guna.UI2.WinForms.Guna2Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnEditInfo = new Guna.UI2.WinForms.Guna2Button();
            this.grpAccountInfo = new System.Windows.Forms.GroupBox();
            this.btnCancel = new Guna.UI2.WinForms.Guna2Button();
            this.btnSave = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)(this.picAvatar)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.grpAccountInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblUsername.Location = new System.Drawing.Point(40, 7);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(181, 38);
            this.lblUsername.TabIndex = 0;
            this.lblUsername.Text = "lblUsername";
            // 
            // btnChangeAvatar
            // 
            this.btnChangeAvatar.BorderRadius = 15;
            this.btnChangeAvatar.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnChangeAvatar.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnChangeAvatar.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnChangeAvatar.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnChangeAvatar.FillColor = System.Drawing.Color.Black;
            this.btnChangeAvatar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnChangeAvatar.ForeColor = System.Drawing.Color.White;
            this.btnChangeAvatar.Location = new System.Drawing.Point(47, 354);
            this.btnChangeAvatar.Name = "btnChangeAvatar";
            this.btnChangeAvatar.Size = new System.Drawing.Size(300, 45);
            this.btnChangeAvatar.TabIndex = 2;
            this.btnChangeAvatar.Text = "Đổi Ảnh Đại Diện";
            // 
            // picAvatar
            // 
            this.picAvatar.Image = ((System.Drawing.Image)(resources.GetObject("picAvatar.Image")));
            this.picAvatar.ImageRotate = 0F;
            this.picAvatar.Location = new System.Drawing.Point(47, 48);
            this.picAvatar.Name = "picAvatar";
            this.picAvatar.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.picAvatar.Size = new System.Drawing.Size(300, 300);
            this.picAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picAvatar.TabIndex = 1;
            this.picAvatar.TabStop = false;
            // 
            // txtFullName
            // 
            this.txtFullName.BorderRadius = 15;
            this.txtFullName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtFullName.DefaultText = "";
            this.txtFullName.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtFullName.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtFullName.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtFullName.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtFullName.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtFullName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtFullName.ForeColor = System.Drawing.Color.Black;
            this.txtFullName.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtFullName.Location = new System.Drawing.Point(30, 74);
            this.txtFullName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.PlaceholderText = "";
            this.txtFullName.SelectedText = "";
            this.txtFullName.Size = new System.Drawing.Size(286, 40);
            this.txtFullName.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label2.Location = new System.Drawing.Point(25, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 25);
            this.label2.TabIndex = 0;
            this.label2.Text = "Họ Tên";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label3.Location = new System.Drawing.Point(25, 129);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 25);
            this.label3.TabIndex = 0;
            this.label3.Text = "Số Điện Thoại";
            // 
            // txtPhone
            // 
            this.txtPhone.BorderRadius = 15;
            this.txtPhone.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPhone.DefaultText = "";
            this.txtPhone.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtPhone.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtPhone.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtPhone.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtPhone.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtPhone.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtPhone.ForeColor = System.Drawing.Color.Black;
            this.txtPhone.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtPhone.Location = new System.Drawing.Point(30, 167);
            this.txtPhone.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.PlaceholderText = "";
            this.txtPhone.SelectedText = "";
            this.txtPhone.Size = new System.Drawing.Size(286, 40);
            this.txtPhone.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label4.Location = new System.Drawing.Point(25, 222);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 25);
            this.label4.TabIndex = 0;
            this.label4.Text = "Email";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label5.Location = new System.Drawing.Point(25, 315);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 25);
            this.label5.TabIndex = 0;
            this.label5.Text = "Địa Chỉ";
            // 
            // txtEmail
            // 
            this.txtEmail.BorderRadius = 15;
            this.txtEmail.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtEmail.DefaultText = "";
            this.txtEmail.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtEmail.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtEmail.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtEmail.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtEmail.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtEmail.ForeColor = System.Drawing.Color.Black;
            this.txtEmail.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtEmail.Location = new System.Drawing.Point(30, 260);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.PlaceholderText = "";
            this.txtEmail.SelectedText = "";
            this.txtEmail.Size = new System.Drawing.Size(286, 40);
            this.txtEmail.TabIndex = 3;
            // 
            // txtAddress
            // 
            this.txtAddress.BorderRadius = 15;
            this.txtAddress.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtAddress.DefaultText = "";
            this.txtAddress.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtAddress.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtAddress.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtAddress.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtAddress.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtAddress.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtAddress.ForeColor = System.Drawing.Color.Black;
            this.txtAddress.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtAddress.Location = new System.Drawing.Point(30, 353);
            this.txtAddress.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.PlaceholderText = "";
            this.txtAddress.SelectedText = "";
            this.txtAddress.Size = new System.Drawing.Size(286, 40);
            this.txtAddress.TabIndex = 3;
            // 
            // txtConfirmPassword
            // 
            this.txtConfirmPassword.BorderRadius = 15;
            this.txtConfirmPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtConfirmPassword.DefaultText = "";
            this.txtConfirmPassword.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtConfirmPassword.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtConfirmPassword.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtConfirmPassword.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtConfirmPassword.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtConfirmPassword.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtConfirmPassword.ForeColor = System.Drawing.Color.Black;
            this.txtConfirmPassword.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtConfirmPassword.Location = new System.Drawing.Point(33, 353);
            this.txtConfirmPassword.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtConfirmPassword.Name = "txtConfirmPassword";
            this.txtConfirmPassword.PlaceholderText = "";
            this.txtConfirmPassword.SelectedText = "";
            this.txtConfirmPassword.Size = new System.Drawing.Size(286, 40);
            this.txtConfirmPassword.TabIndex = 8;
            // 
            // txtNewPassword
            // 
            this.txtNewPassword.BorderRadius = 15;
            this.txtNewPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNewPassword.DefaultText = "";
            this.txtNewPassword.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtNewPassword.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtNewPassword.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtNewPassword.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtNewPassword.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtNewPassword.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtNewPassword.ForeColor = System.Drawing.Color.Black;
            this.txtNewPassword.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtNewPassword.Location = new System.Drawing.Point(33, 260);
            this.txtNewPassword.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtNewPassword.Name = "txtNewPassword";
            this.txtNewPassword.PlaceholderText = "";
            this.txtNewPassword.SelectedText = "";
            this.txtNewPassword.Size = new System.Drawing.Size(286, 40);
            this.txtNewPassword.TabIndex = 9;
            // 
            // txtOldPassword
            // 
            this.txtOldPassword.BorderRadius = 15;
            this.txtOldPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtOldPassword.DefaultText = "";
            this.txtOldPassword.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtOldPassword.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtOldPassword.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtOldPassword.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtOldPassword.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtOldPassword.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtOldPassword.ForeColor = System.Drawing.Color.Black;
            this.txtOldPassword.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtOldPassword.Location = new System.Drawing.Point(33, 167);
            this.txtOldPassword.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtOldPassword.Name = "txtOldPassword";
            this.txtOldPassword.PlaceholderText = "";
            this.txtOldPassword.SelectedText = "";
            this.txtOldPassword.Size = new System.Drawing.Size(286, 40);
            this.txtOldPassword.TabIndex = 10;
            // 
            // txtUsername
            // 
            this.txtUsername.BorderRadius = 15;
            this.txtUsername.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtUsername.DefaultText = "";
            this.txtUsername.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtUsername.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtUsername.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtUsername.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtUsername.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtUsername.ForeColor = System.Drawing.Color.Black;
            this.txtUsername.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtUsername.Location = new System.Drawing.Point(33, 74);
            this.txtUsername.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.PlaceholderText = "";
            this.txtUsername.SelectedText = "";
            this.txtUsername.Size = new System.Drawing.Size(286, 40);
            this.txtUsername.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label6.Location = new System.Drawing.Point(28, 315);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(167, 25);
            this.label6.TabIndex = 4;
            this.label6.Text = "Xác Nhận Mật Khẩu";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label7.Location = new System.Drawing.Point(28, 129);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(87, 25);
            this.label7.TabIndex = 5;
            this.label7.Text = "Mật Khẩu";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label8.Location = new System.Drawing.Point(28, 222);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(123, 25);
            this.label8.TabIndex = 6;
            this.label8.Text = "Mật Khẩu Mới";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label9.Location = new System.Drawing.Point(28, 36);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(87, 25);
            this.label9.TabIndex = 7;
            this.label9.Text = "Tài Khoản";
            // 
            // chkShowPassword
            // 
            this.chkShowPassword.AutoSize = true;
            this.chkShowPassword.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.chkShowPassword.Location = new System.Drawing.Point(175, 129);
            this.chkShowPassword.Name = "chkShowPassword";
            this.chkShowPassword.Size = new System.Drawing.Size(144, 25);
            this.chkShowPassword.TabIndex = 12;
            this.chkShowPassword.Text = "Hiện Mật Khẩu";
            this.chkShowPassword.UseVisualStyleBackColor = true;
            // 
            // btnChangePassword
            // 
            this.btnChangePassword.BorderRadius = 15;
            this.btnChangePassword.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnChangePassword.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnChangePassword.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnChangePassword.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnChangePassword.FillColor = System.Drawing.Color.Black;
            this.btnChangePassword.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnChangePassword.ForeColor = System.Drawing.Color.White;
            this.btnChangePassword.Location = new System.Drawing.Point(47, 456);
            this.btnChangePassword.Name = "btnChangePassword";
            this.btnChangePassword.Size = new System.Drawing.Size(300, 45);
            this.btnChangePassword.TabIndex = 2;
            this.btnChangePassword.Text = "Đổi Mật Khẩu";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtFullName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtPhone);
            this.groupBox1.Controls.Add(this.txtEmail);
            this.groupBox1.Controls.Add(this.txtAddress);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.groupBox1.Location = new System.Drawing.Point(438, 60);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(347, 417);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Thông Tin Cá Nhân";
            // 
            // btnEditInfo
            // 
            this.btnEditInfo.BorderRadius = 15;
            this.btnEditInfo.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnEditInfo.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnEditInfo.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnEditInfo.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnEditInfo.FillColor = System.Drawing.Color.Black;
            this.btnEditInfo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnEditInfo.ForeColor = System.Drawing.Color.White;
            this.btnEditInfo.Location = new System.Drawing.Point(47, 405);
            this.btnEditInfo.Name = "btnEditInfo";
            this.btnEditInfo.Size = new System.Drawing.Size(300, 45);
            this.btnEditInfo.TabIndex = 14;
            this.btnEditInfo.Text = "Đổi Thông Tin Cá Nhân";
            // 
            // grpAccountInfo
            // 
            this.grpAccountInfo.Controls.Add(this.label9);
            this.grpAccountInfo.Controls.Add(this.label8);
            this.grpAccountInfo.Controls.Add(this.label7);
            this.grpAccountInfo.Controls.Add(this.chkShowPassword);
            this.grpAccountInfo.Controls.Add(this.label6);
            this.grpAccountInfo.Controls.Add(this.txtConfirmPassword);
            this.grpAccountInfo.Controls.Add(this.txtUsername);
            this.grpAccountInfo.Controls.Add(this.txtNewPassword);
            this.grpAccountInfo.Controls.Add(this.txtOldPassword);
            this.grpAccountInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.grpAccountInfo.Location = new System.Drawing.Point(826, 60);
            this.grpAccountInfo.Name = "grpAccountInfo";
            this.grpAccountInfo.Size = new System.Drawing.Size(347, 417);
            this.grpAccountInfo.TabIndex = 14;
            this.grpAccountInfo.TabStop = false;
            this.grpAccountInfo.Text = "Thông Tin Tài Khoản";
            // 
            // btnCancel
            // 
            this.btnCancel.BorderRadius = 15;
            this.btnCancel.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCancel.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnCancel.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnCancel.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnCancel.FillColor = System.Drawing.Color.Black;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(47, 558);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(347, 45);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Không Lưu Thông Tin";
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
            this.btnSave.Location = new System.Drawing.Point(47, 507);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(347, 45);
            this.btnSave.TabIndex = 14;
            this.btnSave.Text = "Lưu Thông Tin";
            // 
            // ucCustomerProfile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpAccountInfo);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnEditInfo);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnChangePassword);
            this.Controls.Add(this.btnChangeAvatar);
            this.Controls.Add(this.picAvatar);
            this.Controls.Add(this.lblUsername);
            this.Name = "ucCustomerProfile";
            this.Size = new System.Drawing.Size(1199, 689);
            ((System.ComponentModel.ISupportInitialize)(this.picAvatar)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpAccountInfo.ResumeLayout(false);
            this.grpAccountInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblUsername;
        private Guna.UI2.WinForms.Guna2CirclePictureBox picAvatar;
        private Guna.UI2.WinForms.Guna2Button btnChangeAvatar;
        private Guna.UI2.WinForms.Guna2TextBox txtFullName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private Guna.UI2.WinForms.Guna2TextBox txtPhone;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private Guna.UI2.WinForms.Guna2TextBox txtEmail;
        private Guna.UI2.WinForms.Guna2TextBox txtAddress;
        private Guna.UI2.WinForms.Guna2TextBox txtConfirmPassword;
        private Guna.UI2.WinForms.Guna2TextBox txtNewPassword;
        private Guna.UI2.WinForms.Guna2TextBox txtOldPassword;
        private Guna.UI2.WinForms.Guna2TextBox txtUsername;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private CSharp.Winform.UI.CheckBox chkShowPassword;
        private Guna.UI2.WinForms.Guna2Button btnChangePassword;
        private System.Windows.Forms.GroupBox groupBox1;
        private Guna.UI2.WinForms.Guna2Button btnEditInfo;
        private System.Windows.Forms.GroupBox grpAccountInfo;
        private Guna.UI2.WinForms.Guna2Button btnCancel;
        private Guna.UI2.WinForms.Guna2Button btnSave;
    }
}
