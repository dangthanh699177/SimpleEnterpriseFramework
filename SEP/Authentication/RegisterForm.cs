using SEP.Authentication.Security;
using SEP.Data.Client;
using SEP.Data.Common;
using SEP.Data.Utilities;
using System;
using System.Data;
using System.Windows.Forms;

namespace SEP.Authentication
{
    public partial class RegisterForm : Form
    {
        public ISEPConnection SepConn { get; }
        public ISEPDataProvider SepProvider { get; }
        public EncryptContext Encrypt { get; }

        public RegisterForm()
        {
            InitializeComponent();
        }
        public RegisterForm(ISEPConnection sepConn, ISEPDataProvider sepProvider, EncryptContext encrypt)
        {
            SepConn = sepConn;
            SepProvider = sepProvider;
            Encrypt = encrypt;
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
            LoginForm loginForm = new LoginForm(this.SepConn, this.SepProvider, Helper.GetEncrytpType(Helper.CryptType.Base64));
            this.Hide();
            loginForm.ShowDialog();
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {
            //this.btnRegister.KeyDown += BtnRegister_KeyDown;
        }

        private void BtnRegister_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    if (this.tbFirstName.Text != String.Empty &&
            //        this.tbLastName.Text != String.Empty &&
            //        this.tbUsername.Text != String.Empty && 
            //        this.tbPassword.Text != String.Empty)
            //    {
            //        btnRegister_Click(sender, e);
            //    }
            //}
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (this.tbFirstName.Text == "" || this.tbLastName.Text == "" || this.tbUsername.Text == "" || this.tbPassword.Text == "")
            {
                MessageBox.Show("Content is empty!", "Notification", MessageBoxButtons.OK);
                return;
            }
            IQuery query = Query.Instance;
            ISEPCommand command = SEPCommand.Instance(this.SepConn, this.SepProvider);
            UserAccount u = new UserAccount();

            u.FirstName = this.tbFirstName.Text.Trim();
            u.LastName = this.tbLastName.Text.Trim();
            u.Username = this.tbUsername.Text.Trim();
            u.Password = Encrypt.Encode(this.tbPassword.Text.Trim());   // manipulate strategy design pattern
            
            // check exists Username
            string commandText = query.Select("Username", "UserAccount", new Condition("Username", this.tbUsername.Text));

            if (command.ExecuteCommand(commandText) == true)
            {
                MessageBox.Show("Username is existed!", "Notification", MessageBoxButtons.OK);
            }
            else
            {
                // Username is not existed, insert new
                commandText = query.Insert("UserAccount", u);
                
                if (command.InsertNotAsync(commandText) > 0)
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
}
