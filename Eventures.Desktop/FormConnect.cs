using System.Windows.Forms;

namespace Eventures_Desktop
{
    public partial class FormConnect : Form
    {
        public string ApiUrl { get => this.textBoxApiUrl.Text; }
        public string Username { get => this.textBoxUsername.Text; }
        public string Password { get => this.textBoxPassword.Text; }

        public FormConnect()
        {
            InitializeComponent();
        }
    }
}
