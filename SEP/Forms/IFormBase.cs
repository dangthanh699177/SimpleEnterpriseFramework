using System;

namespace SEP.Forms
{
    public interface IFormBase
    {
        event FormBase.FormHandler OnHandle;

        void btnCancel_Click(object sender, EventArgs e);
        void button1_Click(object sender, EventArgs e);
        void InitializeFormContent(string frmName, string btnName);
        void InitializeInputScope();
        string InitTextBoxContent(object value);
    }
}