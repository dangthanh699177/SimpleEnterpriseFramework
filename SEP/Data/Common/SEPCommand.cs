using SEP.Data.Client;
using SEP.Data.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace SEP.Data.Common
{
    public class SEPCommand : ISEPCommand
    {
        private static DbConnection dbConn;
        private static ISEPDataProvider provider;
        private static SEPCommand instance;

        private SEPCommand(ISEPConnection sepConn, ISEPDataProvider sepProvider)
        {
            provider = sepProvider;
            dbConn = sepConn.CreateConnection(provider);
        }

        public static SEPCommand Instance(ISEPConnection sepConn, ISEPDataProvider sepProvider)
        {
            if (instance == null)
            {
                instance = new SEPCommand(sepConn, sepProvider);
            }
            return instance;
        }

        public async Task<int> Insert(string commandText)
        {
            using (DbCommand cmd = dbConn.CreateCommand())
            {
                if (dbConn.State == ConnectionState.Closed)
                {
                    dbConn.Open();
                }

                cmd.CommandText = commandText;

                DbDataAdapter adapter = SEPDataAdapter.Instance(provider).CreateDataAdapter();
                adapter.InsertCommand = cmd;
                int x = await adapter.InsertCommand.ExecuteNonQueryAsync();

                dbConn.Close();
                return x;
            }
        }
        public int InsertNotAsync(string commandText)
        {
            DbCommand cmd = dbConn.CreateCommand();
            if (dbConn.State == ConnectionState.Closed)
            {
                dbConn.Open();
            }

            cmd.CommandText = commandText;

            DbDataAdapter adapter = SEPDataAdapter.Instance(provider).CreateDataAdapter();
            adapter.InsertCommand = cmd;
            int x = adapter.InsertCommand.ExecuteNonQuery();

            dbConn.Close();
            return x;
        }
        public async Task<int> Update(string commandText)
        {
            using (DbCommand cmd = dbConn.CreateCommand())
            {
                if (dbConn.State == ConnectionState.Closed)
                {
                    dbConn.Open();
                }

                cmd.CommandText = commandText;
                DbDataAdapter adapter = SEPDataAdapter.Instance(provider).CreateDataAdapter();
                adapter.UpdateCommand = cmd;
                int x = await adapter.UpdateCommand.ExecuteNonQueryAsync();

                dbConn.Close();
                return x;
            }
        }
        public async Task<int> Delete(string commandText)
        {
            using (DbCommand cmd = dbConn.CreateCommand())
            {
                if (dbConn.State == ConnectionState.Closed)
                {
                    dbConn.Open();
                }

                cmd.CommandText = commandText;

                DbDataAdapter adapter = SEPDataAdapter.Instance(provider).CreateDataAdapter();
                adapter.DeleteCommand = cmd;
                int x = await adapter.DeleteCommand.ExecuteNonQueryAsync();

                dbConn.Close();
                return x;
            }
        }
        public List<string> GetListTableName()
        {
            if (dbConn.State == ConnectionState.Closed)
            {
                dbConn.Open();
            }

            DataTable dataTable = dbConn.GetSchema("Tables");
            List<string> listName = new List<string>();

            foreach (DataRow row in dataTable.Rows)
            {
                listName.Add((string)row[2]);
            }

            dbConn.Close();
            return listName;
        }
        public List<ISEPDataRow> ExecuteReader(string commandText)
        {
            List<ISEPDataRow> list = new List<ISEPDataRow>();
            DbCommand cmd = dbConn.CreateCommand();

            if (dbConn.State == ConnectionState.Closed)
            {
                dbConn.Open();
            }

            cmd.CommandText = commandText;
            DbDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ISEPDataRow row = new SEPDataRow();
                
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row.Add(reader.GetName(i), reader.GetValue(i));
                }
                list.Add(row);
            }

            dbConn.Close();
            return list;
        }
        public DataTable GetTable(string commandText)
        {
            using (DbCommand cmd = dbConn.CreateCommand())
            {
                if (dbConn.State == ConnectionState.Closed)
                {
                    dbConn.Open();
                }

                cmd.CommandText = commandText;

                DataTable table = new DataTable();
                DbDataAdapter adapter = SEPDataAdapter.Instance(provider).CreateDataAdapter();
                adapter.SelectCommand = cmd;
                adapter.Fill(table);

                dbConn.Close();
                return table;
            }
        }
        public bool ExecuteCommand(string commandText)
        {
            DbCommand cmd = dbConn.CreateCommand();

            if (dbConn.State == ConnectionState.Closed)
            {
                dbConn.Open();
            }

            cmd.CommandText = commandText;
            try
            {
                if (cmd.ExecuteScalar() == null)
                {
                    dbConn.Close();
                    return false;
                }
                dbConn.Close();
                return true;
            }
            catch
            {
                dbConn.Close();
                return false;
            }
        }
    }
}
