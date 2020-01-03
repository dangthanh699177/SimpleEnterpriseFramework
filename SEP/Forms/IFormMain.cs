using System;
using System.Data;
using System.Windows.Forms;
using SEP.Data.Common;

namespace SEP.Forms
{
    public interface IFormMain
    {
        void ConvertToDataRow(DataRow row, ISEPDataRow sepRow);
        void ConvertToDataRowView(DataRowView rowView, ISEPDataRow sepRow);
        bool Run();
    }
}