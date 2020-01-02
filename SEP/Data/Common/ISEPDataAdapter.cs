using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace SEP.Data.Common
{
    public interface ISEPDataAdapter
    {
        DbDataAdapter CreateDataAdapter();
    }
}