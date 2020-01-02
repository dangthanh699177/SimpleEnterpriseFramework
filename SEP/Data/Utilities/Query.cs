using SEP.Authentication;
using SEP.Data.Common;
using System;

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

        public string Insert(string tbName, ISEPDataRow sepRow)
        {
            IQueryHelper q = new QueryHelper(sepRow);
            return $"insert into {tbName}" +
                $" ({q.GetListPropertyName()})" +
                $" values ({q.GetListValue()})";

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
            return $"if exists (select * from sys.tables where name like '{tbName}')" +
                    $"drop table {tbName}" +
                    $"create table {tbName}" +
                    "(" +
                    "   Id  int identity(1,1) not null," +
                    "	FirstName nvarchar(50) not null," +
                    "	LastName nvarchar(50) not null," +
                    "	Username varchar(50) not null," +
                    "	Password varchar(16) not null" +
                    "   constraint PK_UserAccountId primary key clustered(Id)" +
                    ")";
        }
    }
}
