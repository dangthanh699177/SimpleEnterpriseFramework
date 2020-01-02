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
        private readonly ISEPDataProvider provider;
        private readonly ISEPConnection dbConn;

        public LoginForm()
        {
            InitializeComponent();
        }

        public LoginForm(ISEPConnection sepConn, ISEPDataProvider sepProvider)
        {
            dbConn = sepConn;
            provider = sepProvider;

            InitTableAccount(sepConn, sepProvider);
            InitializeComponent();
        }

        private void InitTableAccount(ISEPConnection sepConn, ISEPDataProvider sepProvider)
        {
            ISEPCommand command = SEPCommand.Instance(sepConn, sepProvider);
            IQuery query = Query.Instance;

            if (command.CheckExistsTable(query.Select("UserAccount")) == false)
            {
                if (command.CheckExistsTable(query.CreateTable("UserAccount")) == false)
                {
                    MessageBox.Show("Cannot create table: UserAccount", "Notification", MessageBoxButtons.OK);
                }
            }
            //List<ISEPDataRow> list = command.ExecuteReader(query.Select("UserAccount"));
        }

        private void labelForgetPassword_MouseHover(object sender, EventArgs e)
        {
            this.labelForgetPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Italic | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }
        private void labelRegister_MouseHover(object sender, EventArgs e)
        {
            this.labelRegister.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Italic | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
            Application.Exit();
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string commandText = Query.Instance.Select("UserAccount");
            ISEPCommand command = SEPCommand.Instance(dbConn, provider);
            DataTable dt = command.GetTable(commandText);

            // check exists username and password
            foreach(DataRow row in dt.Rows)
            {
                if (this.tbUsername.Text == row["Username"].ToString() &&
                    this.tbPassword.Text == row["Password"].ToString())
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

        private void labelForgetPassword_Click(object sender, EventArgs e)
        {

        }
        private void labelRegister_Click(object sender, EventArgs e)
        {
            RegisterForm frmRegister = new RegisterForm();
            frmRegister.Show();
            this.Hide();
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
                btnLogin_Click(sender, e);
            }
            else if (e.KeyCode == Keys.Return)
            {
                btnExit_Click(sender, e);
            }
        }

        private void TbUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(sender, e);
            }
            else if (e.KeyCode == Keys.Return)
            {
                btnExit_Click(sender, e);
            }
        }
    }
}
