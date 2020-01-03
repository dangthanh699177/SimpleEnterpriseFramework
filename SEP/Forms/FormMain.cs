using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.ConnectionUI;
using SEP.Authentication;
using SEP.Data.Client;
using SEP.Data.Common;
using SEP.Data.Utilities;

namespace SEP.Forms
{
    public partial class FormMain : Form, IFormMain
    {
        public FormMain()
        {
        }
        
        DataConnectionDialog dcd = new DataConnectionDialog();
        BindingSource bs = new BindingSource();
        ISEPDataProvider sepProvider = null;
        ISEPConnection sepConn = null;
        IQuery query = null;
        ISEPCommand cmd = null;
        ISEPDataRow sepRow = null;
        string commandText = String.Empty;
        string tableName = String.Empty;
        bool dontRunHandler = true;

        public void Run()
        {
            InitializeComponent();
            DataSource.AddStandardDataSources(dcd);

            if (DataConnectionDialog.Show(dcd) == DialogResult.None)
            {
                Application.Exit();
            }

            sepProvider = SEPDataProvider.Instance;
            sepProvider.SetName(dcd.SelectedDataProvider.Name);
            sepConn = SEPConnection.Instance;
            sepConn.SetPath(dcd.ConnectionString);
            query = Query.Instance;
            cmd = SEPCommand.Instance(sepConn, sepProvider);

            LoginForm frmLogin = new LoginForm(sepConn, sepProvider, Helper.GetEncrytpType(Helper.CryptType.Base64));

            if (frmLogin.ShowDialog() == DialogResult.None)
            {
                Application.Exit();
            }

            // get list name of tables in database
            this.cbbTableName.DataSource = this.cmd.GetListTableName();

            // set default for combobox
            this.cbbTableName.SelectedIndex = 0;

            // view DataTable with BindingSource
            tableName = this.cbbTableName.SelectedItem.ToString();
            commandText = query.Select(tableName);
            bs.DataSource = cmd.GetTable(commandText);
            this.dgvDataTable.DataSource = bs.DataSource;

            // hidden id column
            this.dgvDataTable.Columns[0].Visible = false;
            this.Show();
        }
        
        private void cbbTableName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbbTableName.SelectedItem.ToString() == "UserAccount")
            {
                this.btnAdd.Enabled = false;
                this.btnRemove.Enabled = false;
                this.dontRunHandler = true;
            }
            else
            {
                this.btnAdd.Enabled = true;
                this.btnRemove.Enabled = true;
                this.dontRunHandler = false;
            }
            // select * from table
            // binding source
            tableName = this.cbbTableName.SelectedItem.ToString();
            commandText = query.Select(tableName);
            this.bs.DataSource = cmd.GetTable(commandText);
            this.dgvDataTable.DataSource = this.bs.DataSource;
        }

        private void dgvDataTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dontRunHandler)
            {
                return;
            }

            // update bindingSource
            this.bs.Position = e.RowIndex;

            this.sepRow = new SEPDataRow(this.dgvDataTable);
            FormBase frm = new FormUpdate(this.sepRow);
            frm.OnHandle += Frm_OnUpdateAsync;
            frm.ShowDialog();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // assign selection for datagridview
            this.dgvDataTable.CurrentCell = this.dgvDataTable[1, 0];

            // handle form
            this.sepRow = new SEPDataRow(this.dgvDataTable);
            FormBase frm = new FormCreate(this.sepRow);
            frm.OnHandle += Frm_OnCreateAsync;
            frm.ShowDialog();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
        private async void btnRemove_ClickAsync(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muôn xóa bản ghi đang chọn không?", "Thông báo",MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            this.sepRow = new SEPDataRow(this.dgvDataTable);
            tableName = this.cbbTableName.SelectedItem.ToString();
            commandText = query.Delete(tableName, this.sepRow);

            if (await cmd.Delete(commandText) < 0)
            {
                MessageBox.Show("Xoa du lieu khong thanh cong.");
            }
            else
            {
                // update DataGridView again
                DataRowView row = (DataRowView)bs.Current;
                row.Delete();

                MessageBox.Show("Xoa du lieu thanh cong.");
            }
        }
        private async void Frm_OnUpdateAsync(ISEPDataRow dRow)
        {
            tableName = this.cbbTableName.SelectedItem.ToString();
            commandText = query.Update(tableName, dRow);
            
            if (await cmd.Update(commandText) < 0)
            {
                MessageBox.Show("Cap nhat du lieu khong thanh cong.");
            }
            else
            {
                // update DataGridView again
                DataRowView rowView = (DataRowView)bs.Current;
                ConvertToDataRowView(rowView, dRow);
                this.dgvDataTable.Refresh();

                MessageBox.Show("Cap nhat thanh cong.");
            }
        }
        private async void Frm_OnCreateAsync(ISEPDataRow dRow)
        {
            tableName = this.cbbTableName.SelectedItem.ToString();
            commandText = query.Insert(tableName, dRow);

            if (await cmd.Insert(commandText) < 0)
            {
                MessageBox.Show("Them du lieu khong thanh cong.");
            }
            else
            {
                // update DataGridView again
                DataTable dt = (DataTable)bs.DataSource;
                DataRow row = dt.NewRow();
                ConvertToDataRow(row, dRow);
                dt.Rows.Add(row);

                MessageBox.Show("Them moi thanh cong.");
            }

        }
        
        public void ConvertToDataRow(DataRow row, ISEPDataRow dRow)
        {
            foreach (KeyValuePair<string, object> item in dRow.Dictionary)
            {
                if (item.Key == "id" || item.Key == "iD" || item.Key == "Id" || item.Key == "ID")
                {
                    continue;
                }

                row[item.Key] = item.Value;
            }
        }
        public void ConvertToDataRowView(DataRowView rowView, ISEPDataRow dRow)
        {
            foreach (KeyValuePair<string, object> item in dRow.Dictionary)
            {
                if (item.Key == "id" || item.Key == "iD" || item.Key == "Id" || item.Key == "ID")
                {
                    continue;
                }

                rowView[item.Key] = item.Value;
            }
        }
    }
}
