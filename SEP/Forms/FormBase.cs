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
    public abstract partial class FormBase : Form
    {
        // some properties
        protected SEPDataRow row = null;
        protected SEPDataRow listTextBox = new SEPDataRow();

        //System.Timers.Timer STimer;
        private int y = 20;

        // some Events
        public delegate void FormHandler(SEPDataRow row);
        public event FormHandler OnHandle = null;

        // some constructors
        public FormBase()
        {
            InitializeComponent();
        }

        public FormBase(SEPDataRow dRow)
        {
            this.row = dRow;
            InitializeComponent();
        }

        // some definitions
        protected void InitializeFormContent(string frmName, string btnName)
        {
            this.Text = frmName;
            this.button1.Text = btnName;

            InitializeInputScope();
        }

        protected void InitializeInputScope()
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
                lb.Margin = Padding.Empty;
                lb.Padding = Padding.Empty;
                tbox.Location = new Point(120, y);

                // assign TextBox into SEPDataRow for storing, using intermediary object
                this.listTextBox.Add(item.Key, tbox);

                // case UpdateForm:
                // show content of TextBox
                tbox.Text = GetTextTBox(item.Value);

                this.panel1.Controls.Add(tbox);
                y += 25;
            }
            
        }

        protected virtual string GetTextTBox(object value)
        {
            throw new NotImplementedException();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected void button1_Click(object sender, EventArgs e)
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

        protected void FormBase_Load(object sender, EventArgs e)
        {
        }
        
    }
}
