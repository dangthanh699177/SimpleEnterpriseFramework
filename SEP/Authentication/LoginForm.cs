using System;
using System.Windows.Forms;

namespace SEP.Authentication
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void labelForgetPassword_MouseHover(object sender, EventArgs e)
        {
            this.labelForgetPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Italic | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        private void labelRegister_MouseHover(object sender, EventArgs e)
        {
            this.labelRegister.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Italic | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

        }

        private void labelForgetPassword_Click(object sender, EventArgs e)
        {

        }

        private void labelRegister_Click(object sender, EventArgs e)
        {

        }
    }
}
