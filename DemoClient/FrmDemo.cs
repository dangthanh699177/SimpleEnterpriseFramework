using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.ConnectionUI;
using SEP.Data.Common;
using SEP.Forms;

namespace DemoClient
{
    public partial class FrmDemo : Form
    {
        public FrmDemo()
        {
            InitializeComponent();
        }

        // declare (1)
        DataConnectionDialog dcd = new DataConnectionDialog();
        BindingSource bs = new BindingSource();
        SEPDataProvider provider = null;
        SEPConnection conn = null;
        
        private void MainForm_Load(object sender, EventArgs e)
        {
            // define (1.1)
            DataSource.AddStandardDataSources(dcd);

            if (DataConnectionDialog.Show(dcd) == DialogResult.OK)
            {
                // define (1.1)
                provider = SEPDataProvider.Instance(dcd.SelectedDataProvider.Name);
                conn = new SEPConnection(dcd.ConnectionString);

                // get list name of tables in database
                SEPDataAdapter da = new SEPDataAdapter(conn);
                this.cbbTableName.DataSource = da.GetListTableName();

                // set default for combobox
                this.cbbTableName.SelectedIndex = 0;
                da = new SEPDataAdapter($"select * from {this.cbbTableName.SelectedItem.ToString()}", conn);

                // view DataTable with BindingSource
                this.bs.DataSource = da.GetTable();
                this.dgvDataTable.DataSource = this.bs.DataSource;
                this.dgvDataTable.Columns[0].Visible = false;
            }
            else
            {
                Environment.Exit(0);
            }
        }

        private void cbbTableName_SelectedIndexChanged(object sender, EventArgs e)
        {
            SEPDataAdapter da = new SEPDataAdapter($"select * from {this.cbbTableName.SelectedItem.ToString()}", conn);

            this.bs.DataSource = da.GetTable();
            this.dgvDataTable.DataSource = this.bs.DataSource;
        }

        private void dgvDataTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            FormBase frm = new UpdateForm(new SEPDataRow(this.dgvDataTable));
            frm.OnHandle += Frm_OnUpdateAsync;
            frm.ShowDialog();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // assign selection for datagridview
            this.dgvDataTable.CurrentCell = this.dgvDataTable[1, 0];
            FormBase frm = new CreateForm(new SEPDataRow(this.dgvDataTable));
            frm.OnHandle += Frm_OnCreateAsync;
            frm.ShowDialog();
        }

        private async void Frm_OnUpdateAsync(SEPDataRow sepRow)
        {
            string sql = $"update {this.cbbTableName.SelectedItem.ToString()}" +
                $" set {SEPParameters.GetEntity(sepRow)}" +
                $" where {SEPParameters.GetCondition(sepRow)}";
            // deploy sql
            SEPCommand cmd = new SEPCommand(sql, this.conn);
            
            if (await cmd.Update() < 0)
            {
                MessageBox.Show("Cap nhat du lieu khong thanh cong.");
            }
            else
            {
                DataRowView rowView = (DataRowView)bs.Current;
                ConvertToDataRowView(rowView, sepRow);
                this.dgvDataTable.Refresh();

                MessageBox.Show("Cap nhat thanh cong.");
            }
        }

        private async void Frm_OnCreateAsync(SEPDataRow sepRow)
        {
            string sql = $"insert into {this.cbbTableName.SelectedItem.ToString()}" +
                $" ({SEPParameters.GetListPropertyName(sepRow)})" +
                $" values ({SEPParameters.GetListValue(sepRow)})";
            // deploy sql
            SEPCommand cmd = new SEPCommand(sql, this.conn);

            if (await cmd.Insert() < 0)
            {
                MessageBox.Show("Them du lieu khong thanh cong.");
            }
            else
            {
                DataTable dt = (DataTable)bs.DataSource;
                DataRow row = dt.NewRow();
                ConvertToDataRow(row, sepRow);
                dt.Rows.Add(row);

                MessageBox.Show("Them moi thanh cong.");
            }

        }

        private async void btnRemove_ClickAsync(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muôn xóa bản ghi đang chọn không?", "Thông báo",MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            int id = Convert.ToInt32(this.dgvDataTable.CurrentRow.Cells[0].Value);
            string sql = $"delete from {this.cbbTableName.SelectedItem.ToString()}" +
                $" where {this.dgvDataTable.Columns[0].Name} = {id}";
            // deploy sql
            SEPCommand cmd = new SEPCommand(sql, this.conn);

            if (await cmd.Delete() < 0)
            {
                MessageBox.Show("Xoa du lieu khong thanh cong.");
            }
            else
            {
                DataRowView row = (DataRowView)bs.Current;
                row.Delete();

                MessageBox.Show("Xoa du lieu thanh cong.");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        public void ConvertToDataRow(DataRow row, SEPDataRow sepRow)
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

        public void ConvertToDataRowView(DataRowView rowView, SEPDataRow sepRow)
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
