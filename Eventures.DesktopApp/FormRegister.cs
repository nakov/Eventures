using System.Net.Mail;
using System.Text;
using System.Windows.Forms;

namespace Eventures.DesktopApp
{
    using static DataConstants;
    public partial class FormRegister : Form
    {
        public string Username { get => this.textBoxUsername.Text; }
        public string Email { get => this.textBoxEmail.Text; }
        public string Password { get => this.textBoxPassword.Text; }
        public string ConfirmPassword { get => this.textBoxConfirmPassword.Text; }
        public string FirstName { get => this.textBoxFirstName.Text; }
        public string LastName { get => this.textBoxLastName.Text; }

        public FormRegister()
        {
            InitializeComponent();
        }

        private void FormRegister_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void buttonRegisterConfirm_Click(object sender, System.EventArgs e)
        {
            var result = NamesAreValid() 
                + PasswordIsValid()
                + ConfirmPasswordIsValid();

            // If there are no erros - return
            if (this.buttonRegisterConfirm.DialogResult == DialogResult.OK)
            {
                return;
            }
            if (string.IsNullOrEmpty(result.Trim()))
            {
                this.buttonRegisterConfirm.DialogResult = DialogResult.OK;
                this.buttonRegisterConfirm.PerformClick();
            }
            else
            {
                MessageBox.Show(result.Trim());
            }
        }

        private string NamesAreValid()
        {
            var errors = new StringBuilder();

            if (string.IsNullOrEmpty(this.Username))
            {
                errors.AppendLine("Username field is required.");
            }

            if (string.IsNullOrEmpty(this.Email))
            {
                errors.AppendLine("Email field is required.");
            }
            else
            {
                try
                {
                    var validEmail = new MailAddress(this.Email);
                }
                catch
                {
                    errors.AppendLine("Email field must have a valid email address.");
                }
            }

            if (string.IsNullOrEmpty(this.FirstName))
            {
                errors.AppendLine("First name field is required.");
            }

            if (string.IsNullOrEmpty(this.LastName))
            {
                errors.AppendLine("Last name field is required.");
            }

            return errors.ToString();
        }

        private string PasswordIsValid()
        {
            var password = this.Password;
            var errors = new StringBuilder();

            if (string.IsNullOrEmpty(password))
            {
                errors.AppendLine("Password field is required.");
            }

            if (password.Length > 0 && password.Length < MinPassword)
            {
                errors.AppendLine($"Password must be at least {MinPassword} characters long.");
            }

            if (password.Length > MaxPassword)
            {
                errors.AppendLine($"Password must be less than {MaxPassword} characters long.");
            }

            return errors.ToString();
        }

        private string ConfirmPasswordIsValid()
        {
            var confirmPassword = this.ConfirmPassword;
            var errors = new StringBuilder();

            if (string.IsNullOrEmpty(confirmPassword))
            {
                errors.AppendLine("Confirm Password field is required.");
            }

            if(confirmPassword.Length > 0 && confirmPassword != this.Password)
            {
                errors.AppendLine("Confirm Password and Password don't match.");
            }

            return errors.ToString();
        }
    }
}
