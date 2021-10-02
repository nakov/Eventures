namespace Eventures.DesktopApp
{
    partial class FormAllEvents
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
            this.buttonLogin = new System.Windows.Forms.Button();
            this.buttonRegister = new System.Windows.Forms.Button();
            this.buttonCreate = new System.Windows.Forms.Button();
            this.buttonReload = new System.Windows.Forms.Button();
            this.listViewEvents = new System.Windows.Forms.ListView();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonLogin
            // 
            this.buttonLogin.AccessibleName = "login button";
            this.buttonLogin.Location = new System.Drawing.Point(12, 12);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(79, 29);
            this.buttonLogin.TabIndex = 9;
            this.buttonLogin.Text = "➞ Login";
            this.buttonLogin.UseVisualStyleBackColor = true;
            this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
            this.buttonLogin.Enabled = false;
            // 
            // buttonRegister
            // 
            this.buttonRegister.AccessibleName = "login button";
            this.buttonRegister.Location = new System.Drawing.Point(106, 12);
            this.buttonRegister.Name = "buttonRegister";
            this.buttonRegister.Size = new System.Drawing.Size(99, 29);
            this.buttonRegister.TabIndex = 10;
            this.buttonRegister.Text = "✓ Register";
            this.buttonRegister.UseVisualStyleBackColor = true;
            this.buttonRegister.Click += new System.EventHandler(this.buttonRegister_Click);
            this.buttonRegister.Enabled = false;
            // 
            // buttonCreate
            // 
            this.buttonCreate.AccessibleName = "create button";
            this.buttonCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCreate.Location = new System.Drawing.Point(708, 12);
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Size = new System.Drawing.Size(79, 29);
            this.buttonCreate.TabIndex = 8;
            this.buttonCreate.Text = "✚ Create";
            this.buttonCreate.UseVisualStyleBackColor = true;
            this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
            this.buttonCreate.Enabled = false;
            // 
            // buttonReload
            // 
            this.buttonReload.AccessibleName = "reload button";
            this.buttonReload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonReload.Location = new System.Drawing.Point(802, 12);
            this.buttonReload.Name = "buttonReload";
            this.buttonReload.Size = new System.Drawing.Size(94, 29);
            this.buttonReload.TabIndex = 3;
            this.buttonReload.Text = "⟳ Reload";
            this.buttonReload.UseVisualStyleBackColor = true;
            this.buttonReload.Click += new System.EventHandler(this.buttonReload_Click);
            this.buttonReload.Enabled = false;
            // 
            // listViewEvents
            // 
            this.listViewEvents.AccessibleName = "events list box";
            this.listViewEvents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewEvents.FullRowSelect = true;
            this.listViewEvents.HideSelection = false;
            this.listViewEvents.LabelWrap = false;
            this.listViewEvents.Location = new System.Drawing.Point(0, 53);
            this.listViewEvents.MultiSelect = false;
            this.listViewEvents.Name = "listViewEvents";
            this.listViewEvents.Size = new System.Drawing.Size(908, 507);
            this.listViewEvents.TabIndex = 4;
            this.listViewEvents.UseCompatibleStateImageBehavior = false;
            this.listViewEvents.View = System.Windows.Forms.View.Details;
            // 
            // statusStrip
            // 
            this.statusStrip.AccessibleName = "statusStrip";
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.statusStrip.Location = new System.Drawing.Point(0, 559);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(908, 26);
            this.statusStrip.TabIndex = 7;
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.AccessibleName = "statusBox";
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(97, 20);
            this.toolStripStatusLabel.Text = "Connecting ...";
            // 
            // FormAllEvents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(908, 585);
            this.Controls.Add(this.buttonRegister);
            this.Controls.Add(this.buttonLogin);
            this.Controls.Add(this.buttonCreate);
            this.Controls.Add(this.buttonReload);
            this.Controls.Add(this.listViewEvents);
            this.Controls.Add(this.statusStrip);
            this.MinimumSize = new System.Drawing.Size(900, 500);
            this.Name = "FormAllEvents";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Event Board";
            this.Shown += new System.EventHandler(this.EventBoardForm_Shown);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Button buttonLogin;
        private System.Windows.Forms.Button buttonRegister;
        private System.Windows.Forms.Button buttonCreate;
        private System.Windows.Forms.Button buttonReload;
        private System.Windows.Forms.ListView listViewEvents;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
    }
}