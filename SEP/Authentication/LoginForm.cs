using SEP.Authentication.Security;
using SEP.Data.Client;
using SEP.Data.Common;
using SEP.Data.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace SEP.Authentication
{
    public partial class LoginForm : Form
    {
        public ISEPConnection SepConn { get; }
        public ISEPDataProvider SepProvider { get; }

        public LoginForm()
        {
            InitializeComponent();
        }

        public LoginForm(ISEPConnection sepConn, ISEPDataProvider sepProvider)
        {
            InitTableAccount(sepConn, sepProvider);
            InitializeComponent();
            SepConn = sepConn;
            SepProvider = sepProvider;
        }

        private void InitTableAccount(ISEPConnection sepConn, ISEPDataProvider sepProvider)
        {
            ISEPCommand command = SEPCommand.Instance(sepConn, sepProvider);
            IQuery query = Query.Instance;

            if (command.ExecuteCommand(query.Select("UserAccount")) == false)
            {
                if (command.ExecuteCommand(query.CreateTable("UserAccount")) == false)
                {
                    MessageBox.Show("Cannot create table: UserAccount", "Notification", MessageBoxButtons.OK);
                }
            }
            //List<ISEPDataRow> list = command.ExecuteReader(query.Select("UserAccount"));
        }
        
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
            Application.Exit();
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            // encrypt password
            Encrypt encrypt = new Encrypt();
            encrypt.SetEncrypt(new Base64Encrypt());
            string pass = encrypt.Encode(this.tbPassword.Text);

            // fetch datatable from database
            string commandText = Query.Instance.Select("UserAccount");
            ISEPCommand command = SEPCommand.Instance(this.SepConn, this.SepProvider);
            DataTable dt = command.GetTable(commandText);
            
            foreach(DataRow row in dt.Rows)
            {
                if (this.tbUsername.Text == row["Username"].ToString() &&
                    pass == row["Password"].ToString())
                {
                    this.DialogResult = DialogResult.OK;
                    return;
                }
            }

            if (MessageBox.Show("Wrong Username or Password!", "notification", MessageBoxButtons.OK) == DialogResult.OK)
            {
                this.tbUsername.Text = "";
                this.tbPassword.Text = "";
                this.tbUsername.Focus();
            }
        }
        
        private void LoginForm_Load(object sender, EventArgs e)
        {
            this.tbUsername.KeyDown += TbUsername_KeyDown;
            this.tbPassword.KeyDown += TbPassword_KeyDown;
        }

        private void TbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.tbUsername.Text != String.Empty && this.tbPassword.Text != String.Empty)
                {
                    btnLogin_Click(sender, e);
                }
            }
        }
        private void TbUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.tbUsername.Text != String.Empty && this.tbPassword.Text != String.Empty)
                {
                    btnLogin_Click(sender, e);
                }
            }
            else if (e.KeyCode == Keys.Return)
            {
                btnExit_Click(sender, e);
            }
        }
        
        private void lblRegister_Click(object sender, EventArgs e)
        {
            RegisterForm frmRegister = new RegisterForm(this.SepConn, this.SepProvider);
            frmRegister.ShowDialog();
            this.Close();
        }
        private void lblRegister_MouseEnter(object sender, EventArgs e)
        {
            this.lblRegister.BackColor = System.Drawing.Color.Red;
            this.lblRegister.ForeColor = System.Drawing.Color.White;
            this.lblRegister.Cursor = System.Windows.Forms.Cursors.Hand;
        }
        private void lblRegister_MouseLeave(object sender, EventArgs e)
        {
            this.lblRegister.BackColor = System.Drawing.SystemColors.Control;
            this.lblRegister.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblRegister.Cursor = System.Windows.Forms.Cursors.Default;
        }
    }
}
