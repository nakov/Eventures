namespace Eventures_Desktop
{
    partial class FormConnect
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
            this.labelApiUrl = new System.Windows.Forms.Label();
            this.textBoxApiUrl = new System.Windows.Forms.TextBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.labelUsername = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.labelRegister = new System.Windows.Forms.Label();
            this.textBoxEmail = new System.Windows.Forms.TextBox();
            this.labelEmail = new System.Windows.Forms.Label();
            this.textBoxConfirmPassword = new System.Windows.Forms.TextBox();
            this.labelConfirmPassword = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelApiUrl
            // 
            this.labelApiUrl.AutoSize = true;
            this.labelApiUrl.Location = new System.Drawing.Point(12, 21);
            this.labelApiUrl.Name = "labelApiUrl";
            this.labelApiUrl.Size = new System.Drawing.Size(101, 20);
            this.labelApiUrl.TabIndex = 0;
            this.labelApiUrl.Text = "Eventures API:";
            // 
            // textBoxApiUrl
            // 
            this.textBoxApiUrl.AccessibleName = "URL text box";
            this.textBoxApiUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxApiUrl.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBoxApiUrl.Location = new System.Drawing.Point(148, 16);
            this.textBoxApiUrl.Name = "textBoxApiUrl";
            this.textBoxApiUrl.Size = new System.Drawing.Size(387, 30);
            this.textBoxApiUrl.TabIndex = 1;
            this.textBoxApiUrl.Text = "https://localhost:44359/api/";
            // 
            // buttonConnect
            // 
            this.buttonConnect.AccessibleName = "connect button";
            this.buttonConnect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConnect.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonConnect.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.buttonConnect.Location = new System.Drawing.Point(12, 249);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(523, 47);
            this.buttonConnect.TabIndex = 2;
            this.buttonConnect.Text = "✓ Connect";
            this.buttonConnect.UseVisualStyleBackColor = false;
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.AccessibleName = "Username text box";
            this.textBoxUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxUsername.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBoxUsername.Location = new System.Drawing.Point(148, 92);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(387, 30);
            this.textBoxUsername.TabIndex = 4;
            this.textBoxUsername.Text = "Your username here...";
            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.Location = new System.Drawing.Point(14, 97);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(78, 20);
            this.labelUsername.TabIndex = 3;
            this.labelUsername.Text = "Username:";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.AccessibleName = "Password text box";
            this.textBoxPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPassword.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBoxPassword.Location = new System.Drawing.Point(148, 164);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(387, 30);
            this.textBoxPassword.TabIndex = 6;
            this.textBoxPassword.Text = "Your password here...";
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(14, 169);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(73, 20);
            this.labelPassword.TabIndex = 5;
            this.labelPassword.Text = "Password:";
            // 
            // labelRegister
            // 
            this.labelRegister.AutoSize = true;
            this.labelRegister.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelRegister.Location = new System.Drawing.Point(248, 55);
            this.labelRegister.Name = "labelRegister";
            this.labelRegister.Size = new System.Drawing.Size(90, 28);
            this.labelRegister.TabIndex = 7;
            this.labelRegister.Text = "Register";
            this.labelRegister.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // textBoxEmail
            // 
            this.textBoxEmail.AccessibleName = "Email text box";
            this.textBoxEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxEmail.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBoxEmail.Location = new System.Drawing.Point(148, 128);
            this.textBoxEmail.Name = "textBoxEmail";
            this.textBoxEmail.Size = new System.Drawing.Size(387, 30);
            this.textBoxEmail.TabIndex = 9;
            this.textBoxEmail.Text = "Your email here...";
            // 
            // labelEmail
            // 
            this.labelEmail.AutoSize = true;
            this.labelEmail.Location = new System.Drawing.Point(14, 133);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(49, 20);
            this.labelEmail.TabIndex = 8;
            this.labelEmail.Text = "Email:";
            // 
            // textBoxConfirmPassword
            // 
            this.textBoxConfirmPassword.AccessibleName = "Confirm Password text box";
            this.textBoxConfirmPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxConfirmPassword.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBoxConfirmPassword.Location = new System.Drawing.Point(148, 202);
            this.textBoxConfirmPassword.Name = "textBoxConfirmPassword";
            this.textBoxConfirmPassword.Size = new System.Drawing.Size(387, 30);
            this.textBoxConfirmPassword.TabIndex = 11;
            this.textBoxConfirmPassword.Text = "Your confirm password here...";
            // 
            // labelConfirmPassword
            // 
            this.labelConfirmPassword.AutoSize = true;
            this.labelConfirmPassword.Location = new System.Drawing.Point(15, 207);
            this.labelConfirmPassword.Name = "labelConfirmPassword";
            this.labelConfirmPassword.Size = new System.Drawing.Size(130, 20);
            this.labelConfirmPassword.TabIndex = 10;
            this.labelConfirmPassword.Text = "Confirm Password:";
            // 
            // FormConnect
            // 
            this.AcceptButton = this.buttonConnect;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 312);
            this.Controls.Add(this.textBoxConfirmPassword);
            this.Controls.Add(this.labelConfirmPassword);
            this.Controls.Add(this.textBoxEmail);
            this.Controls.Add(this.labelEmail);
            this.Controls.Add(this.labelRegister);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.textBoxUsername);
            this.Controls.Add(this.labelUsername);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.textBoxApiUrl);
            this.Controls.Add(this.labelApiUrl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "FormConnect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Connect to Eventures API";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelApiUrl;
        private System.Windows.Forms.TextBox textBoxApiUrl;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.TextBox textBoxUsername;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.Label labelRegister;
        private System.Windows.Forms.TextBox textBoxEmail;
        private System.Windows.Forms.Label labelEmail;
        private System.Windows.Forms.TextBox textBoxConfirmPassword;
        private System.Windows.Forms.Label labelConfirmPassword;
    }
}

