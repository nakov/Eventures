using System.Windows.Forms;

namespace Eventures_Desktop
{
    public partial class FormRegister : Form
    {
        public string Username { get => this.textBoxUsername.Text; }
        public string Email { get => this.textBoxEmail.Text; }
        public string Password { get => this.textBoxPassword.Text; }
        public string ConfirmPassword { get => this.textBoxConfirmPassword.Text; }

        public FormRegister()
        {
            InitializeComponent();
        }

        private void FormRegister_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }
    }
}
