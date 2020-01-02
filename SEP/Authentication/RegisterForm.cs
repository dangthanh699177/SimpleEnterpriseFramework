using SEP.Authentication.Security;
using SEP.Data.Client;
using SEP.Data.Common;
using SEP.Data.Utilities;
using System;
using System.Windows.Forms;

namespace SEP.Authentication
{
    public partial class RegisterForm : Form
    {
        public ISEPConnection SepConn { get; }
        public ISEPDataProvider SepProvider { get; }

        public RegisterForm()
        {
            InitializeComponent();
        }
        public RegisterForm(ISEPConnection sepConn, ISEPDataProvider sepProvider)
        {
            SepConn = sepConn;
            SepProvider = sepProvider;
            InitializeComponent();
        }

        private void lblGotoLogin_MouseEnter(object sender, EventArgs e)
        {
            this.lblGotoLogin.BackColor = System.Drawing.Color.Red;
            this.lblGotoLogin.ForeColor = System.Drawing.Color.White;
            this.lblGotoLogin.Cursor = System.Windows.Forms.Cursors.Hand;
        }
        private void lblGotoLogin_MouseLeave(object sender, EventArgs e)
        {
            this.lblGotoLogin.BackColor = System.Drawing.SystemColors.Control;
            this.lblGotoLogin.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblGotoLogin.Cursor = System.Windows.Forms.Cursors.Default;
        }
        private void lblGotoLogin_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.ShowDialog();
            this.Close();
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {
            this.btnRegister.KeyDown += BtnRegister_KeyDown;
        }

        private void BtnRegister_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.tbFirstName.Text != String.Empty &&
                    this.tbLastName.Text != String.Empty &&
                    this.tbUsername.Text != String.Empty && 
                    this.tbPassword.Text != String.Empty)
                {
                    btnRegister_Click(sender, e);
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void btnRegister_Click(object sender, EventArgs e)
        {
            IQuery query = Query.Instance;
            ISEPCommand command = SEPCommand.Instance(this.SepConn, this.SepProvider);
            Encrypt encrypt = new Encrypt();
            UserAccount u = new UserAccount();

            u.FirstName = this.tbFirstName.Text.Trim();
            u.LastName = this.tbLastName.Text.Trim();
            u.Username = this.tbUsername.Text.Trim();
            encrypt.SetEncrypt(new Base64Encrypt());
            u.Password = encrypt.Encode(this.tbPassword.Text.Trim());   // manipulate strategy design pattern

            string commandText = query.Insert("UserAccount", u);
            
            if (command.ExecuteCommand(commandText) == true)
            {
                MessageBox.Show("Successfully registration!", "Notification", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("Failly registration!", "Notification", MessageBoxButtons.OK);
            }

        }
    }
}
