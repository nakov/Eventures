namespace Eventures.DesktopApp
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
            this.SuspendLayout();
            // 
            // labelApiUrl
            // 
            this.labelApiUrl.AutoSize = true;
            this.labelApiUrl.Location = new System.Drawing.Point(12, 21);
            this.labelApiUrl.Name = "labelApiUrl";
            this.labelApiUrl.Size = new System.Drawing.Size(122, 20);
            this.labelApiUrl.TabIndex = 0;
            this.labelApiUrl.Text = "Eventures API url:";
            // 
            // textBoxApiUrl
            // 
            this.textBoxApiUrl.AccessibleName = "URL text box";
            this.textBoxApiUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxApiUrl.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBoxApiUrl.Location = new System.Drawing.Point(148, 16);
            this.textBoxApiUrl.Name = "textBoxApiUrl";
            this.textBoxApiUrl.Size = new System.Drawing.Size(254, 30);
            this.textBoxApiUrl.TabIndex = 1;
            this.textBoxApiUrl.Text = "http://localhost:4000/api/";
            // 
            // buttonConnect
            // 
            this.buttonConnect.AccessibleName = "connect button";
            this.buttonConnect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConnect.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonConnect.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.buttonConnect.Location = new System.Drawing.Point(12, 66);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(390, 47);
            this.buttonConnect.TabIndex = 6;
            this.buttonConnect.Text = "✓ Connect";
            this.buttonConnect.UseVisualStyleBackColor = false;
            // 
            // FormConnect
            // 
            this.AcceptButton = this.buttonConnect;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 133);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.textBoxApiUrl);
            this.Controls.Add(this.labelApiUrl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(2000, 180);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 180);
            this.Name = "FormConnect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Connect to Eventures API";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormConnect_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelApiUrl;
        private System.Windows.Forms.TextBox textBoxApiUrl;
        private System.Windows.Forms.Button buttonConnect;
    }
}

