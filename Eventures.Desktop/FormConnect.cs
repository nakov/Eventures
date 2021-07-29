using System.Windows.Forms;

namespace Eventures_Desktop
{
    public partial class FormConnect : Form
    {
        public string ApiUrl { get => this.textBoxApiUrl.Text; }
        public string Username { get => this.textBoxUsername.Text; }
        public string Email { get => this.textBoxEmail.Text; }
        public string Password { get => this.textBoxPassword.Text; }
        public string ConfirmPassword { get => this.textBoxConfirmPassword.Text; }

        public FormConnect()
        {
            InitializeComponent();
        }
    }
}
