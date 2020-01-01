using System;
using System.Windows.Forms;

namespace SEP.Authentication
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void labelRegister_MouseHover(object sender, EventArgs e)
        {
            this.labelGotoLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Italic | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            this.Hide();
            LoginForm loginForm = new LoginForm();
            loginForm.ShowDialog();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
