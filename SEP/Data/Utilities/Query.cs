using SEP.Authentication;
using SEP.Data.Common;
using System;
using System.Collections.Generic;

namespace SEP.Data.Utilities
{
    public class Query : IQuery
    {
        private static Query instance;

        private Query()
        {
        }

        public static Query Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Query();
                }
                return instance;
            }
        }
        
        public string Select(string tbName)
        {
            return $"select * from {tbName}";
        }
        public string Select(string field, string tbName, Condition c)
        {
            return $"select {field} from {tbName} where {c.Name} = '{c.Value}'";
        }
        public string Insert(string tbName, ISEPDataRow sepRow)
        {
            IQueryHelper q = new QueryHelper(sepRow);
            return $"insert into {tbName}" +
                $" ({q.GetListPropertyName()})" +
                $" values ({q.GetListValue()})";

        }
        public string Insert(string tbName, UserAccount u)
        {
            return $"insert into {tbName} values (N'{u.FirstName}', N'{u.LastName}', '{u.Username}', '{u.Password}')";
        }
        public string Update(string tbName, ISEPDataRow sepRow)
        {
            IQueryHelper q = new QueryHelper(sepRow);
            return $"update {tbName}" +
                $" set {q.GetEntity()}" +
                $" where {q.GetCondition()}";

        }
        public string Delete(string tbName)
        {
            return $"delete from {tbName}";
        }
        public string Delete(string tbName, ISEPDataRow sepRow)
        {
            IQueryHelper q = new QueryHelper(sepRow);
            return $"delete from {tbName}" +
                $" where {q.GetAllCondition()}";
        }
        public string CreateTable(string tbName)
        {
            return $"if not exists (select name from sys.tables where name = '{tbName}') " +
                    $"create table {tbName} " +
                    "(" +
                    "   Id  int identity(1,1) not null," +
                    "	FirstName nvarchar(50) not null," +
                    "	LastName nvarchar(50) not null," +
                    "	Username varchar(50) not null," +
                    "	Password varchar(1000) not null" +
                    "   constraint PK_UserAccountId primary key clustered(Id)" +
                    ") " +
                    $"insert into {tbName} values (N'Dang', N'Huynh Thanh', 'itcui', 'MTIzNDU2')";
        }
    }
}
