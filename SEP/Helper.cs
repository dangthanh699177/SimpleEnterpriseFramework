using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEP
{
    public class Helper
    {
        public enum DbName
        {
            SQLServer,
            MySQL,
            Unknow
        }

        public static DbName GetDbName(string dbName)
        {
            switch(dbName.ToLower())
            {
                case "sqlserver":
                    return DbName.SQLServer;
                case "mysql":
                    return DbName.MySQL;
                default:
                    return DbName.Unknow;
            }
        }
    }
}
