﻿using System.Windows.Forms;

namespace Eventures_Desktop
{
    partial class FormCreateEvent
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
            this.labelName = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.buttonCreate = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.textBoxPlace = new System.Windows.Forms.TextBox();
            this.labelPlace = new System.Windows.Forms.Label();
            this.dateTimePickerStart = new System.Windows.Forms.DateTimePicker();
            this.labelStart = new System.Windows.Forms.Label();
            this.dateTimePickerEnd = new System.Windows.Forms.DateTimePicker();
            this.labelEnd = new System.Windows.Forms.Label();
            this.numboxTickets = new System.Windows.Forms.NumericUpDown();
            this.labelTickets = new System.Windows.Forms.Label();
            this.labelPrice = new System.Windows.Forms.Label();
            this.numboxPrice = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numboxTickets)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numboxPrice)).BeginInit();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(12, 21);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(52, 20);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "Name:";
            // 
            // textBoxName
            // 
            this.textBoxName.AccessibleName = "name box";
            this.textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxName.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBoxName.Location = new System.Drawing.Point(73, 16);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(458, 30);
            this.textBoxName.TabIndex = 1;
            // 
            // buttonCreate
            // 
            this.buttonCreate.AccessibleName = "create button";
            this.buttonCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCreate.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonCreate.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.buttonCreate.Location = new System.Drawing.Point(326, 278);
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Size = new System.Drawing.Size(168, 47);
            this.buttonCreate.TabIndex = 4;
            this.buttonCreate.Text = "✓ Create";
            this.buttonCreate.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.AccessibleName = "cancel button";
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.buttonCancel.Location = new System.Drawing.Point(56, 278);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(164, 47);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "✕ Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // textBoxPlace
            // 
            this.textBoxPlace.AccessibleName = "place box";
            this.textBoxPlace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPlace.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBoxPlace.Location = new System.Drawing.Point(73, 62);
            this.textBoxPlace.Name = "textBoxPlace";
            this.textBoxPlace.Size = new System.Drawing.Size(458, 30);
            this.textBoxPlace.TabIndex = 6;
            // 
            // labelPlace
            // 
            this.labelPlace.AutoSize = true;
            this.labelPlace.Location = new System.Drawing.Point(12, 67);
            this.labelPlace.Name = "labelPlace";
            this.labelPlace.Size = new System.Drawing.Size(47, 20);
            this.labelPlace.TabIndex = 5;
            this.labelPlace.Text = "Place:";
            // 
            // dateTimePickerStart
            // 
            this.dateTimePickerStart.AccessibleName = "start box";
            this.dateTimePickerStart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerStart.CustomFormat = "dd/MM/yyyy hh:mm:ss";
            this.dateTimePickerStart.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dateTimePickerStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerStart.Location = new System.Drawing.Point(73, 109);
            this.dateTimePickerStart.Name = "dateTimePickerStart";
            this.dateTimePickerStart.Size = new System.Drawing.Size(207, 30);
            this.dateTimePickerStart.TabIndex = 8;
            // 
            // labelStart
            // 
            this.labelStart.AutoSize = true;
            this.labelStart.Location = new System.Drawing.Point(12, 117);
            this.labelStart.Name = "labelStart";
            this.labelStart.Size = new System.Drawing.Size(43, 20);
            this.labelStart.TabIndex = 7;
            this.labelStart.Text = "Start:";
            // 
            // dateTimePickerEnd
            // 
            this.dateTimePickerEnd.AccessibleName = "end box";
            this.dateTimePickerEnd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerEnd.CustomFormat = "dd/MM/yyyy hh:mm:ss";
            this.dateTimePickerEnd.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dateTimePickerEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerEnd.Location = new System.Drawing.Point(72, 155);
            this.dateTimePickerEnd.Name = "dateTimePickerEnd";
            this.dateTimePickerEnd.Size = new System.Drawing.Size(207, 30);
            this.dateTimePickerEnd.TabIndex = 10;
            // 
            // labelEnd
            // 
            this.labelEnd.AutoSize = true;
            this.labelEnd.Location = new System.Drawing.Point(12, 163);
            this.labelEnd.Name = "labelEnd";
            this.labelEnd.Size = new System.Drawing.Size(37, 20);
            this.labelEnd.TabIndex = 9;
            this.labelEnd.Text = "End:";
            // 
            // numboxTickets
            // 
            this.numboxTickets.AccessibleName = "tickets box";
            this.numboxTickets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numboxTickets.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.numboxTickets.Location = new System.Drawing.Point(112, 206);
            this.numboxTickets.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numboxTickets.Name = "numboxTickets";
            this.numboxTickets.Size = new System.Drawing.Size(80, 30);
            this.numboxTickets.TabIndex = 12;
            // 
            // labelTickets
            // 
            this.labelTickets.AutoSize = true;
            this.labelTickets.Location = new System.Drawing.Point(12, 210);
            this.labelTickets.Name = "labelTickets";
            this.labelTickets.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelTickets.Size = new System.Drawing.Size(94, 20);
            this.labelTickets.TabIndex = 11;
            this.labelTickets.Text = "Total Tickets:";
            // 
            // labelPrice
            // 
            this.labelPrice.AutoSize = true;
            this.labelPrice.Location = new System.Drawing.Point(279, 210);
            this.labelPrice.Name = "labelPrice";
            this.labelPrice.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelPrice.Size = new System.Drawing.Size(111, 20);
            this.labelPrice.TabIndex = 13;
            this.labelPrice.Text = "Price Per Ticket:";
            // 
            // numboxPrice
            // 
            this.numboxPrice.AccessibleName = "tickets box";
            this.numboxPrice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numboxPrice.DecimalPlaces = 2;
            this.numboxPrice.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.numboxPrice.Location = new System.Drawing.Point(399, 206);
            this.numboxPrice.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numboxPrice.Name = "numericUpDown1";
            this.numboxPrice.Size = new System.Drawing.Size(131, 30);
            this.numboxPrice.TabIndex = 14;
            // 
            // FormCreateEvent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(556, 353);
            this.Controls.Add(this.numboxPrice);
            this.Controls.Add(this.labelPrice);
            this.Controls.Add(this.numboxTickets);
            this.Controls.Add(this.labelTickets);
            this.Controls.Add(this.dateTimePickerEnd);
            this.Controls.Add(this.labelEnd);
            this.Controls.Add(this.dateTimePickerStart);
            this.Controls.Add(this.labelStart);
            this.Controls.Add(this.textBoxPlace);
            this.Controls.Add(this.labelPlace);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonCreate);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.labelName);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 400);
            this.Name = "FormCreateEvent";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create New Event";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormConnect_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.numboxTickets)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numboxPrice)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label labelName;
        private TextBox textBoxName;
        private Label labelPlace;
        private TextBox textBoxPlace;
        private Label labelStart;
        private DateTimePicker dateTimePickerStart;
        private Label labelEnd;
        private DateTimePicker dateTimePickerEnd;
        private Label labelTickets;
        private NumericUpDown numboxTickets;
        private Label labelPrice;
        private NumericUpDown numboxPrice;
        private Button buttonCancel;
        private Button buttonCreate;
    }
}
