using SEP.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SEP.Forms
{
    public abstract partial class FormBase : Form, IFormBase
    {
        protected ISEPDataRow row = null;
        protected ISEPDataRow listTextBox = new SEPDataRow();
        protected int y = 20;
        
        public delegate void FormHandler(ISEPDataRow row);
        public event FormHandler OnHandle = null;
        
        public FormBase()
        {
            InitializeComponent();
        }
        public FormBase(ISEPDataRow dRow)
        {
            this.row = dRow;
            InitializeComponent();
        }
        
        public void InitializeFormContent(string frmName, string btnName)
        {
            this.Text = frmName;
            this.button1.Text = btnName;

            InitializeInputScope();
        }
        public void InitializeInputScope()
        {
            Label lb = null;
            TextBox tbox = null;

            // scrolling panel
            this.panel1.AutoScroll = true;

            foreach (KeyValuePair<string, object> item in this.row.Dictionary)
            {
                lb = new Label();
                lb.Text = char.ToUpper(item.Key[0]) + item.Key.Substring(1);

                if (lb.Text == "id" ||
                    lb.Text == "iD" ||
                    lb.Text == "Id" ||
                    lb.Text == "ID")
                {
                    continue;
                }

                lb.Name = "lb" + item.Key;
                lb.TextAlign = ContentAlignment.MiddleRight;
                lb.Margin = Padding.Empty;
                lb.Padding = Padding.Empty;
                lb.Location = new Point(10, y);

                this.panel1.Controls.Add(lb);

                tbox = new TextBox();
                tbox.Name = "tb" + item.Key;
                tbox.Width = 180;
                tbox.Margin = Padding.Empty;
                tbox.Padding = Padding.Empty;
                tbox.Location = new Point(120, y);

                if (item.Key.ToLower() == "password")
                {
                    tbox.PasswordChar = '*';
                }

                // assign TextBox into SEPDataRow for storing, using intermediary object
                this.listTextBox.Add(item.Key, tbox);

                // case UpdateForm:
                // show content of TextBox
                tbox.Text = InitTextBoxContent(item.Value);

                this.panel1.Controls.Add(tbox);
                y += 25;
            }
            
        }
        public virtual string InitTextBoxContent(object value)
        {
            throw new NotImplementedException();
        }

        public void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void button1_Click(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, object> item in this.listTextBox.Dictionary)
            {
                this.row[item.Key] = ((TextBox)item.Value).Text;
            }

            if (OnHandle != null)
            {
                OnHandle(row);
            }
        }
    }
}
