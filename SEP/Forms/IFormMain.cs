using System;
using System.Data;
using System.Windows.Forms;
using SEP.Data.Common;

namespace SEP.Forms
{
    public interface IFormMain
    {
        void btnAdd_Click(object sender, EventArgs e);
        void btnExit_Click(object sender, EventArgs e);
        void btnRemove_ClickAsync(object sender, EventArgs e);
        void cbbTableName_SelectedIndexChanged(object sender, EventArgs e);
        void ConvertToDataRow(DataRow row, ISEPDataRow sepRow);
        void ConvertToDataRowView(DataRowView rowView, ISEPDataRow sepRow);
        void dgvDataTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e);
        void Frm_OnCreateAsync(ISEPDataRow dRow);
        void Frm_OnUpdateAsync(ISEPDataRow dRow);
        void MainForm_Load(object sender, EventArgs e);
    }
}