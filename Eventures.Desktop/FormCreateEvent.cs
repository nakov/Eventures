using System;
using System.Windows.Forms;

namespace Eventures_Desktop
{
    public partial class FormCreateEvent : Form
    {
        public string EvName { get => this.textBoxName.Text; }
        public string Place { get => this.textBoxPlace.Text; }
        public DateTime Start { get => DateTime.Parse(this.dateTimePickerStart.Text); }
        public DateTime End { get => DateTime.Parse(this.dateTimePickerEnd.Text); }
        public int TotalTickets { get =>int.Parse(this.numboxTickets.Text); }
        public decimal PricePerTicket { get =>decimal.Parse(this.numboxPrice.Text); }

        public FormCreateEvent()
        {
            InitializeComponent();
        }

        private void FormConnect_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }
    }
}
