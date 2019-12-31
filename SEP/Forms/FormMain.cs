using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.ConnectionUI;
using SEP.Data.Client;
using SEP.Data.Common;
using SEP.Data.Utilities;

namespace SEP.Forms
{
    public partial class FormMain : Form, IFormMain
    {
        public FormMain()
        {
            InitializeComponent();
        }

        // declare (1)
        DataConnectionDialog dcd = new DataConnectionDialog();
        BindingSource bs = new BindingSource();
        ISEPDataProvider provider = null;
        ISEPConnection conn = null;
        ISEPDataAdapter sepAdapter = null;
        ISQLQuery query = null;
        ISEPCommand cmd = null;
        ISEPDataRow sepRow = null;

        public void MainForm_Load(object sender, EventArgs e)
        {
            // define (1.1)
            DataSource.AddStandardDataSources(dcd);

            // Show Connection Dialog
            if (DataConnectionDialog.Show(dcd) == DialogResult.OK)
            {
                // define (1.1)
                this.provider = SEPDataProvider.Instance;
                this.provider.SetName(dcd.SelectedDataProvider.Name);
                this.conn = SEPConnection.Instance;
                this.conn.SetPath(dcd.ConnectionString);

                // get list name of tables in database
                this.sepAdapter = new SEPDataAdapter(this.conn, this.provider);
                this.cbbTableName.DataSource = sepAdapter.GetListTableName();

                // set default for combobox
                this.cbbTableName.SelectedIndex = 0;
                this.query = new SQLQuery(this.cbbTableName.SelectedItem.ToString());
                this.sepAdapter = new SEPDataAdapter(this.query.Select(), this.conn, this.provider);

                // view DataTable with BindingSource
                this.bs.DataSource = this.sepAdapter.GetTable();
                this.dgvDataTable.DataSource = this.bs.DataSource;

                // hidden id column
                this.dgvDataTable.Columns[0].Visible = false;
            }
            else
            {
                Environment.Exit(0);
            }
        }

        public void cbbTableName_SelectedIndexChanged(object sender, EventArgs e)
        {
            // select * from table
            this.query = new SQLQuery(this.cbbTableName.SelectedItem.ToString());
            this.sepAdapter = new SEPDataAdapter(this.query.Select(), this.conn, this.provider);

            // binding source
            this.bs.DataSource = this.sepAdapter.GetTable();
            this.dgvDataTable.DataSource = this.bs.DataSource;
        }

        public void dgvDataTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // update bindingSource
            this.bs.Position = e.RowIndex;

            this.sepRow = new SEPDataRow(this.dgvDataTable);
            FormBase frm = new FormUpdate(this.sepRow);
            frm.OnHandle += Frm_OnUpdateAsync;
            frm.ShowDialog();
        }

        public void btnAdd_Click(object sender, EventArgs e)
        {
            // assign selection for datagridview
            this.dgvDataTable.CurrentCell = this.dgvDataTable[1, 0];

            // handle form
            this.sepRow = new SEPDataRow(this.dgvDataTable);
            FormBase frm = new FormCreate(this.sepRow);
            frm.OnHandle += Frm_OnCreateAsync;
            frm.ShowDialog();
        }

        public async void Frm_OnUpdateAsync(ISEPDataRow dRow)
        {
            this.query = new SQLQuery(this.cbbTableName.SelectedItem.ToString());
            this.cmd = new SEPCommand(this.query.Update(sepRow), this.conn, this.provider);
            
            if (await this.cmd.Update() < 0)
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

        public async void Frm_OnCreateAsync(ISEPDataRow dRow)
        {
            this.query = new SQLQuery(this.cbbTableName.SelectedItem.ToString());
            this.cmd = new SEPCommand(this.query.Insert(sepRow), this.conn, this.provider);

            if (await cmd.Insert() < 0)
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

        public async void btnRemove_ClickAsync(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muôn xóa bản ghi đang chọn không?", "Thông báo",MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }
            
            this.query = new SQLQuery(this.cbbTableName.SelectedItem.ToString());
            this.cmd = new SEPCommand(this.query.Delete(this.sepRow), this.conn, this.provider);

            if (await cmd.Delete() < 0)
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

        public void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        public void ConvertToDataRow(DataRow row, ISEPDataRow sepRow)
        {
            foreach (KeyValuePair<string, object> item in sepRow.Dictionary)
            {
                if (item.Key == "id" || item.Key == "iD" || item.Key == "Id" || item.Key == "ID")
                {
                    continue;
                }

                row[item.Key] = item.Value;
            }
        }

        public void ConvertToDataRowView(DataRowView rowView, ISEPDataRow sepRow)
        {
            foreach (KeyValuePair<string, object> item in sepRow.Dictionary)
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
