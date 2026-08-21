namespace C.A.R.E_app
{
    partial class popUpView
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblStatus = new Label();
            picCamera = new PictureBox();
            btnReset = new Button();
            ((System.ComponentModel.ISupportInitialize)picCamera).BeginInit();
            SuspendLayout();
            // 
            // lblStatus
            // 
            lblStatus.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblStatus.Font = new Font("Britannic Bold", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblStatus.Location = new Point(70, 235);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(321, 126);
            lblStatus.TabIndex = 0;
            // 
            // picCamera
            // 
            picCamera.Location = new Point(88, 24);
            picCamera.Name = "picCamera";
            picCamera.Size = new Size(288, 192);
            picCamera.SizeMode = PictureBoxSizeMode.Zoom;
            picCamera.TabIndex = 1;
            picCamera.TabStop = false;
            // 
            // btnReset
            // 
            btnReset.Font = new Font("Aharoni", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnReset.Location = new Point(173, 386);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(108, 40);
            btnReset.TabIndex = 2;
            btnReset.Text = "OK";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += btnReset_Click;
            // 
            // popUpView
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(491, 450);
            Controls.Add(btnReset);
            Controls.Add(picCamera);
            Controls.Add(lblStatus);
            ForeColor = SystemColors.Desktop;
            Name = "popUpView";
            Text = "popUpView";
            Load += popUpView_Load;
            ((System.ComponentModel.ISupportInitialize)picCamera).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label lblStatus;
        private PictureBox picCamera;
        private Button btnReset;
    }
}
