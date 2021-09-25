using System;
using System.Globalization;
using System.Windows.Forms;

namespace Eventures.DesktopApp
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
            this.buttonCreateConfirm = new System.Windows.Forms.Button();
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
            this.textBoxName.Location = new System.Drawing.Point(66, 16);
            this.textBoxName.MaxLength = 50;
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.PlaceholderText = "Enter event name ...";
            this.textBoxName.Size = new System.Drawing.Size(462, 30);
            this.textBoxName.TabIndex = 1;
            // 
            // buttonCreateConfirm
            // 
            this.buttonCreateConfirm.AccessibleName = "confirm create button";
            this.buttonCreateConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCreateConfirm.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonCreateConfirm.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.buttonCreateConfirm.Location = new System.Drawing.Point(310, 203);
            this.buttonCreateConfirm.Name = "buttonCreateConfirm";
            this.buttonCreateConfirm.Size = new System.Drawing.Size(168, 47);
            this.buttonCreateConfirm.TabIndex = 7;
            this.buttonCreateConfirm.Text = "✓ Create";
            this.buttonCreateConfirm.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.AccessibleName = "cancel button";
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.buttonCancel.Location = new System.Drawing.Point(55, 203);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(164, 47);
            this.buttonCancel.TabIndex = 8;
            this.buttonCancel.Text = "✕ Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // textBoxPlace
            // 
            this.textBoxPlace.AccessibleName = "place box";
            this.textBoxPlace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPlace.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBoxPlace.Location = new System.Drawing.Point(66, 62);
            this.textBoxPlace.MaxLength = 70;
            this.textBoxPlace.Name = "textBoxPlace";
            this.textBoxPlace.PlaceholderText = "Enter the event venue (place) ...";
            this.textBoxPlace.Size = new System.Drawing.Size(462, 30);
            this.textBoxPlace.TabIndex = 2;
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
            this.dateTimePickerStart.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dateTimePickerStart.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dateTimePickerStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerStart.Location = new System.Drawing.Point(66, 109);
            this.dateTimePickerStart.Name = "dateTimePickerStart";
            this.dateTimePickerStart.Size = new System.Drawing.Size(188, 30);
            this.dateTimePickerStart.TabIndex = 3;
            this.dateTimePickerStart.Text = DateTime.ParseExact(DateTime.UtcNow.AddDays(1).ToString("dd/MM/yyyy HH:mm"), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture).ToString();
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
            this.dateTimePickerEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerEnd.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dateTimePickerEnd.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dateTimePickerEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerEnd.Location = new System.Drawing.Point(325, 109);
            this.dateTimePickerEnd.Name = "dateTimePickerEnd";
            this.dateTimePickerEnd.Size = new System.Drawing.Size(203, 30);
            this.dateTimePickerEnd.TabIndex = 4;
            this.dateTimePickerEnd.Text = DateTime.ParseExact(DateTime.UtcNow.AddDays(2).ToString("dd/MM/yyyy HH:mm"), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture).ToString();
            // 
            // labelEnd
            // 
            this.labelEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelEnd.AutoSize = true;
            this.labelEnd.Location = new System.Drawing.Point(282, 115);
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
            this.numboxTickets.Location = new System.Drawing.Point(116, 154);
            this.numboxTickets.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numboxTickets.Name = "numboxTickets";
            this.numboxTickets.Size = new System.Drawing.Size(138, 30);
            this.numboxTickets.TabIndex = 5;
            this.numboxTickets.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labelTickets
            // 
            this.labelTickets.AutoSize = true;
            this.labelTickets.Location = new System.Drawing.Point(16, 158);
            this.labelTickets.Name = "labelTickets";
            this.labelTickets.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelTickets.Size = new System.Drawing.Size(94, 20);
            this.labelTickets.TabIndex = 11;
            this.labelTickets.Text = "Total Tickets:";
            // 
            // labelPrice
            // 
            this.labelPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPrice.AutoSize = true;
            this.labelPrice.Location = new System.Drawing.Point(277, 158);
            this.labelPrice.Name = "labelPrice";
            this.labelPrice.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelPrice.Size = new System.Drawing.Size(111, 20);
            this.labelPrice.TabIndex = 13;
            this.labelPrice.Text = "Price Per Ticket:";
            // 
            // numboxPrice
            // 
            this.numboxPrice.AccessibleName = "price box";
            this.numboxPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numboxPrice.DecimalPlaces = 2;
            this.numboxPrice.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.numboxPrice.Location = new System.Drawing.Point(392, 154);
            this.numboxPrice.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numboxPrice.Name = "numboxPrice";
            this.numboxPrice.Size = new System.Drawing.Size(136, 30);
            this.numboxPrice.TabIndex = 6;
            // 
            // FormCreateEvent
            // 
            this.AcceptButton = this.buttonCreateConfirm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(545, 272);
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
            this.Controls.Add(this.buttonCreateConfirm);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.labelName);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 300);
            this.Name = "FormCreateEvent";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create a New Event";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormCreateEvent_KeyDown);
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
        private Button buttonCreateConfirm;
    }
}

