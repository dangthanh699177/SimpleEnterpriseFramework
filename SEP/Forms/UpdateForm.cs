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
    public partial class UpdateForm : FormBase
    {
        public UpdateForm(SEPDataRow row) : base(row)
        {
            this.InitializeFormContent("Update Row", "Update");
        }

        protected override string GetTextTBox(object value) => value.ToString();
    }
}
