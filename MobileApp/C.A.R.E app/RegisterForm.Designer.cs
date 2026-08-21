namespace C.A.R.E_app
{
    partial class RegisterForm
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
            label3 = new Label();
            label4 = new Label();
            txtNewPhone = new TextBox();
            txtNewPassword = new TextBox();
            txtNewCamera = new TextBox();
            label5 = new Label();
            txtNewName = new TextBox();
            btnSubmitRegister = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = SystemColors.GradientActiveCaption;
            label1.Location = new Point(629, 153);
            label1.Margin = new Padding(14, 0, 14, 0);
            label1.Name = "label1";
            label1.Size = new Size(414, 59);
            label1.TabIndex = 0;
            label1.Text = "C.A.R.E - Login";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Aharoni", 19.8000011F, FontStyle.Bold);
            label2.ForeColor = SystemColors.GradientActiveCaption;
            label2.Location = new Point(531, 333);
            label2.Margin = new Padding(14, 0, 14, 0);
            label2.Name = "label2";
            label2.Size = new Size(264, 34);
            label2.TabIndex = 1;
            label2.Text = "Phone Number:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Aharoni", 19.8000011F, FontStyle.Bold);
            label3.ForeColor = SystemColors.GradientActiveCaption;
            label3.Location = new Point(531, 379);
            label3.Margin = new Padding(14, 0, 14, 0);
            label3.Name = "label3";
            label3.Size = new Size(182, 34);
            label3.TabIndex = 2;
            label3.Text = "Password:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Aharoni", 19.8000011F, FontStyle.Bold);
            label4.ForeColor = SystemColors.GradientActiveCaption;
            label4.Location = new Point(531, 425);
            label4.Margin = new Padding(14, 0, 14, 0);
            label4.Name = "label4";
            label4.Size = new Size(234, 34);
            label4.TabIndex = 3;
            label4.Text = "Camera code:";
            // 
            // txtNewPhone
            // 
            txtNewPhone.Font = new Font("Aharoni", 19.8000011F, FontStyle.Bold);
            txtNewPhone.Location = new Point(812, 327);
            txtNewPhone.Name = "txtNewPhone";
            txtNewPhone.Size = new Size(311, 40);
            txtNewPhone.TabIndex = 6;
            // 
            // txtNewPassword
            // 
            txtNewPassword.Font = new Font("Aharoni", 19.8000011F, FontStyle.Bold);
            txtNewPassword.Location = new Point(812, 373);
            txtNewPassword.Name = "txtNewPassword";
            txtNewPassword.Size = new Size(311, 40);
            txtNewPassword.TabIndex = 7;
            // 
            // txtNewCamera
            // 
            txtNewCamera.Font = new Font("Aharoni", 19.8000011F, FontStyle.Bold);
            txtNewCamera.Location = new Point(812, 419);
            txtNewCamera.Name = "txtNewCamera";
            txtNewCamera.Size = new Size(311, 40);
            txtNewCamera.TabIndex = 8;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Aharoni", 19.8000011F, FontStyle.Bold);
            label5.ForeColor = SystemColors.GradientActiveCaption;
            label5.Location = new Point(531, 290);
            label5.Name = "label5";
            label5.Size = new Size(121, 34);
            label5.TabIndex = 9;
            label5.Text = "Name:";
            // 
            // txtNewName
            // 
            txtNewName.Font = new Font("Aharoni", 19.8000011F, FontStyle.Bold);
            txtNewName.Location = new Point(812, 281);
            txtNewName.Name = "txtNewName";
            txtNewName.Size = new Size(311, 40);
            txtNewName.TabIndex = 10;
            // 
            // btnSubmitRegister
            // 
            btnSubmitRegister.BackColor = SystemColors.ActiveCaption;
            btnSubmitRegister.Font = new Font("Aharoni", 16.2F, FontStyle.Bold);
            btnSubmitRegister.ForeColor = SystemColors.ButtonHighlight;
            btnSubmitRegister.Location = new Point(1072, 487);
            btnSubmitRegister.Name = "btnSubmitRegister";
            btnSubmitRegister.Size = new Size(136, 37);
            btnSubmitRegister.TabIndex = 12;
            btnSubmitRegister.Text = "Register";
            btnSubmitRegister.UseVisualStyleBackColor = false;
            btnSubmitRegister.Click += btnSubmitRegister_Click;
            // 
            // RegisterForm
            // 
            AutoScaleDimensions = new SizeF(36F, 59F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1924, 1055);
            Controls.Add(btnSubmitRegister);
            Controls.Add(txtNewName);
            Controls.Add(label5);
            Controls.Add(txtNewCamera);
            Controls.Add(txtNewPassword);
            Controls.Add(txtNewPhone);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Font = new Font("Aharoni", 36F, FontStyle.Bold);
            Margin = new Padding(14, 9, 14, 9);
            Name = "RegisterForm";
            Text = "RegisterForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox txtNewPhone;
        private TextBox txtNewPassword;
        private TextBox txtNewCamera;
        private Label label5;
        private TextBox txtNewName;
        private Button btnSubmitRegister;
    }
}