﻿using System;
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

        // declare (1)
        DataConnectionDialog dcd = new DataConnectionDialog();
        BindingSource bs = new BindingSource();
        ISEPDataProvider sepProvider = null;
        ISEPConnection sepConn = null;
        ISEPDataAdapter sepAdapter = null;
        IQuery query = null;
        ISEPCommand cmd = null;
        ISEPDataRow sepRow = null;
        string commandText = String.Empty;
        string tableName = String.Empty;

        public void Run()
        {
            InitializeComponent();
            // Open this form dialog
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Show();
        }

        protected void MainForm_Load(object sender, EventArgs e)
        {
            // define (1.1)
            DataSource.AddStandardDataSources(dcd);
            
            if (DataConnectionDialog.Show(dcd) == DialogResult.OK)
            {
                sepProvider = SEPDataProvider.Instance;
                sepProvider.SetName(dcd.SelectedDataProvider.Name);
                sepConn = SEPConnection.Instance;
                sepConn.SetPath(dcd.ConnectionString);
                query = Query.Instance;
                cmd = SEPCommand.Instance(sepConn, sepProvider);

                //LoginForm frmLogin = new LoginForm();
                LoginForm frmLogin = new LoginForm(sepConn, sepProvider);

                if (frmLogin.ShowDialog() == DialogResult.OK)
                {
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
                }
                else
                {
                    Application.Exit();
                }
            }
            else
            {
                Application.Exit();
            }
        }

        protected void cbbTableName_SelectedIndexChanged(object sender, EventArgs e)
        {
            // select * from table
            // binding source
            tableName = this.cbbTableName.SelectedItem.ToString();
            commandText = query.Select(tableName);
            this.bs.DataSource = cmd.GetTable(commandText);
            this.dgvDataTable.DataSource = this.bs.DataSource;
        }

        protected void dgvDataTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // update bindingSource
            this.bs.Position = e.RowIndex;

            this.sepRow = new SEPDataRow(this.dgvDataTable);
            FormBase frm = new FormUpdate(this.sepRow);
            frm.OnHandle += Frm_OnUpdateAsync;
            frm.ShowDialog();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            // assign selection for datagridview
            this.dgvDataTable.CurrentCell = this.dgvDataTable[1, 0];

            // handle form
            this.sepRow = new SEPDataRow(this.dgvDataTable);
            FormBase frm = new FormCreate(this.sepRow);
            frm.OnHandle += Frm_OnCreateAsync;
            frm.ShowDialog();
        }

        protected async void Frm_OnUpdateAsync(ISEPDataRow dRow)
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

        protected async void Frm_OnCreateAsync(ISEPDataRow dRow)
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

        protected async void btnRemove_ClickAsync(object sender, EventArgs e)
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

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
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
