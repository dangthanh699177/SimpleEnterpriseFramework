using SEP.Data.Common;

namespace SEP.Forms
{
    public partial class FormUpdate : FormBase
    {
        public FormUpdate(ISEPDataRow row) : base(row)
        {
            this.InitializeFormContent("Update Row", "Update");
        }

        public override string InitTextBoxContent(object value) => value.ToString();
    }
}
