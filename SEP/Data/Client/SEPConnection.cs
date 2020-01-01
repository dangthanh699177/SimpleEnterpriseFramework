using SEP.Data.Common;
using System;
using System.Data.Common;

namespace SEP.Data.Client
{
    public class SEPConnection : ISEPConnection
    {
        public static string path;
        private static SEPConnection instance;
        private SEPConnection() { }
        public static SEPConnection Instance
        {
            get {
                if (instance == null)
                {
                    instance = new SEPConnection();
                }
                return instance;
            }
        }

        public void SetPath(string newPath)
        {
            path = newPath;
        }

        public DbConnection CreateConnection(ISEPDataProvider sepDataProvider)
        {
            if (path == String.Empty || path == null)
            {
                throw new Exception("string DataConnectionDialog.ConnectionString is not right!");
            }
            DbConnection dbConn = sepDataProvider.Factory().CreateConnection();
            dbConn.ConnectionString = path;
            return dbConn;
        }
    }
}
