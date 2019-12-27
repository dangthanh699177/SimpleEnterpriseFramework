using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEP.Data.Common
{
    public class SEPDataProvider
    {
        private static string dpName;
        private static SEPDataProvider instance;
        private SEPDataProvider(string dataProviderName) { }
        public static SEPDataProvider Instance(string dataProviderName)
        {
            if (instance == null)
            {
                instance = new SEPDataProvider(dataProviderName);
                dpName = dataProviderName;
            }
            return instance;
        }

        /// <summary>
        /// Trả về .NET Framework DataProvider mà client đã lựa chọn kết nối
        /// </summary>
        public static DbProviderFactory Factory() => 
            dpName == String.Empty || dpName == null
            ? throw new Exception("string DataConnectionDialog.SelectedDataProvider.Name is not right!")
            : DbProviderFactories.GetFactory(dpName);
    }
}
