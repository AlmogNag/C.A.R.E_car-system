namespace C.A.R.E_app
{
    partial class LoginForm
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
            label1 = new Label();
            label2 = new Label();
            txtPhone = new TextBox();
            label3 = new Label();
            txtPassword = new TextBox();
            btnLogin = new Button();
            lnkRegister = new LinkLabel();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Aharoni", 36F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(605, 77);
            label1.Margin = new Padding(8, 0, 8, 0);
            label1.Name = "label1";
            label1.Size = new Size(414, 59);
            label1.TabIndex = 0;
            label1.Text = "C.A.R.E - Login";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(605, 246);
            label2.Name = "label2";
            label2.Size = new Size(264, 34);
            label2.TabIndex = 1;
            label2.Text = "Phone Number:";
            // 
            // txtPhone
            // 
            txtPhone.Location = new Point(864, 243);
            txtPhone.Name = "txtPhone";
            txtPhone.Size = new Size(291, 40);
            txtPhone.TabIndex = 2;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(605, 293);
            label3.Name = "label3";
            label3.Size = new Size(182, 34);
            label3.TabIndex = 3;
            label3.Text = "Password:";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(864, 293);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(291, 40);
            txtPassword.TabIndex = 4;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = SystemColors.ActiveCaption;
            btnLogin.Font = new Font("Aharoni", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLogin.ForeColor = Color.Transparent;
            btnLogin.Location = new Point(1108, 361);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(142, 38);
            btnLogin.TabIndex = 5;
            btnLogin.Text = "Sign In";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // lnkRegister
            // 
            lnkRegister.AutoSize = true;
            lnkRegister.Font = new Font("Aharoni", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lnkRegister.LinkColor = Color.FromArgb(192, 192, 255);
            lnkRegister.Location = new Point(1256, 370);
            lnkRegister.Name = "lnkRegister";
            lnkRegister.Size = new Size(118, 23);
            lnkRegister.TabIndex = 6;
            lnkRegister.TabStop = true;
            lnkRegister.Text = "New User";
            lnkRegister.LinkClicked += lnkRegister_LinkClicked;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(20F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1924, 720);
            Controls.Add(lnkRegister);
            Controls.Add(btnLogin);
            Controls.Add(txtPassword);
            Controls.Add(label3);
            Controls.Add(txtPhone);
            Controls.Add(label2);
            Controls.Add(label1);
            Font = new Font("Aharoni", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ForeColor = SystemColors.GradientActiveCaption;
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Margin = new Padding(8, 5, 8, 5);
            MaximizeBox = false;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "C.A.R.E - Login";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox txtPhone;
        private Label label3;
        private TextBox txtPassword;
        private Button btnLogin;
        private LinkLabel lnkRegister;
    }
}