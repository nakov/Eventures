using System;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace Eventures.DesktopApp
{
    using static DataConstants;
    public partial class FormCreateEvent : Form
    {
        public string EvName { get => this.textBoxName.Text; }
        public string Place { get => this.textBoxPlace.Text; }
        public DateTime Start { get => DateTime.SpecifyKind(DateTime.Parse(this.dateTimePickerStart.Text), DateTimeKind.Utc); }
        public DateTime End { get => DateTime.SpecifyKind(DateTime.Parse(this.dateTimePickerEnd.Text), DateTimeKind.Utc); }
        public int TotalTickets { get => int.Parse(this.numboxTickets.Text); }
        public decimal PricePerTicket { get => decimal.Parse(this.numboxPrice.Text); }

        public FormCreateEvent()
        {
            InitializeComponent();
            this.dateTimePickerStart.Value = DateTime.UtcNow.AddDays(1);
            this.dateTimePickerEnd.Value = DateTime.UtcNow.AddDays(2);
        }

        private void FormCreateEvent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void buttonCreateConfirm_Click(object sender, EventArgs e)
        {
            var result = NameIsValid() 
                + PlaceIsValid() 
                + StartDateIsValid()
                + EndDateIsValid()
                + TicketsIsValid()
                + PriceIsValid();

            // If there are no errors - return
            if (this.buttonCreateConfirm.DialogResult == DialogResult.OK)
            {
                return;
            }
            if (string.IsNullOrEmpty(result.Trim()))
            {
                this.buttonCreateConfirm.DialogResult = DialogResult.OK;
                this.buttonCreateConfirm.PerformClick();
            }
            else
            {
                MessageBox.Show(result.Trim(), "Errors");
            }
        }

        private string NameIsValid()
        {
            var name = this.EvName;
            var errors = new StringBuilder();

            if(string.IsNullOrEmpty(name))
            {
                errors.AppendLine("Name field is required.");
            }

            if(name.Length > 0 && name.Length < MinEventName)
            {
                errors.AppendLine($"Name should be at least {MinEventName} characters long.");
            }

            if (name.Length > MaxEventName)
            {
                errors.AppendLine($"Name should not be more than {MaxEventName} characters long.");
            }

            return errors.ToString();
        }

        private string PlaceIsValid()
        {
            var place = this.Place;
            var errors = new StringBuilder();

            if (string.IsNullOrEmpty(place))
            {
                errors.AppendLine("Place field is required.");
            }

            if (place.Length > 0 && place.Length < MinEventPlace)
            {
                errors.AppendLine($"Place should be at least {MinEventPlace} characters long.");
            }

            if (place.Length > MaxEventPlace)
            {
                errors.AppendLine($"Place should not be more than {MaxEventPlace} characters long.");
            }

            return errors.ToString();
        }

        private string StartDateIsValid()
        {
            var startDate = this.Start;
            var errors = new StringBuilder();

            if (startDate < DateTime.UtcNow)
            {
                errors.AppendLine($"Start Date must be in the future.");
            }

            if (startDate > DateTime.Parse("01/01/2100"))
            {
                errors.AppendLine($"Start Date must be before the 2100 year.");
            }

            return errors.ToString();
        }

        private string EndDateIsValid()
        {
            var endDate = this.End;
            var errors = new StringBuilder();

            if (endDate < this.Start)
            {
                errors.AppendLine($"End Date must be after the Start Date.");
            }

            if (endDate > DateTime.Parse("01/01/2100"))
            {
                errors.AppendLine($"End Date must be before the 2100 year.");
            }

            return errors.ToString();
        }

        private string TicketsIsValid()
        {
            var ticketsCount = this.TotalTickets;
            var errors = new StringBuilder();

            if (ticketsCount < 0)
            {
                errors.AppendLine("Tickets must be a positive number.");
            }

            if(ticketsCount > MaxEventTickets)
            {
                errors.AppendLine($"Tickets must be less than {MaxEventTickets}.");
            }

            return errors.ToString();
        }

        private string PriceIsValid()
        {
            var price = this.PricePerTicket;
            var errors = new StringBuilder();

            if (price < 0)
            {
                errors.AppendLine("Price must be a positive number.");
            }

            if ((double)price > MaxEventPrice)
            {
                errors.AppendLine($"Price must be less than {MaxEventTickets}.");
            }

            return errors.ToString();
        }
    }
}
