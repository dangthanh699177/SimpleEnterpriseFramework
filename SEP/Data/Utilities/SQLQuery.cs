using SEP.Data.Common;
using System;

namespace SEP.Data.Utilities
{
    public class SQLQuery : ISQLQuery
    {
        private string tbName = String.Empty;

        public SQLQuery(string tbName)
        {
            this.tbName = tbName;
        }

        public string Select()
        {
            return $"select * from {this.tbName}";
        }

        public string Insert(ISEPDataRow sepRow)
        {
            IQueryHandler tb = new QueryHandler(sepRow);
            return $"insert into {this.tbName}" +
                $" ({tb.GetListPropertyName()})" +
                $" values ({tb.GetListValue()})";

        }

        public string Update(ISEPDataRow sepRow)
        {
            IQueryHandler tb = new QueryHandler(sepRow);
            return $"update {this.tbName}" +
                $" set {tb.GetEntity()}" +
                $" where {tb.GetCondition()}";

        }

        public string Delete()
        {
            IQueryHandler tb = new QueryHandler();
            return $"delete from {this.tbName}";

        }

        public string Delete(ISEPDataRow sepRow)
        {
            IQueryHandler tb = new QueryHandler(sepRow);
            return $"delete from {this.tbName}" +
                $" where {tb.GetAllCondition()}";
        }
    }
}
