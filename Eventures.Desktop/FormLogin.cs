using System.Windows.Forms;

namespace Eventures_Desktop
{
    public partial class FormLogin : Form
    {
        public string Username { get => this.textBoxUsername.Text; }
        public string Password { get => this.textBoxPassword.Text; }

        public FormLogin()
        {
            InitializeComponent();
        }

        private void FormLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }
    }
}
