using System;
using System.Globalization;
using System.Windows.Forms;

namespace Eventures.DesktopApp
{
    public partial class FormCreateEvent : Form
    {
        public string EvName { get => this.textBoxName.Text; }
        public string Place { get => this.textBoxPlace.Text; }
        public DateTime Start { get => DateTime.ParseExact(this.dateTimePickerStart.Text, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture); }
        public DateTime End { get => DateTime.ParseExact(this.dateTimePickerEnd.Text, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture); }
        public int TotalTickets { get => int.Parse(this.numboxTickets.Text); }
        public decimal PricePerTicket { get => decimal.Parse(this.numboxPrice.Text); }

        public FormCreateEvent()
        {
            InitializeComponent();
        }

        private void FormCreateEvent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }
    }
}
