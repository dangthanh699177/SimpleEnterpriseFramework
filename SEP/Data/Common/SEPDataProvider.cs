using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEP.Data.Common
{
    public class SEPDataProvider : ISEPDataProvider
    {
        public static string dpName;
        private static SEPDataProvider instance;
        private SEPDataProvider() { }
        public static SEPDataProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SEPDataProvider();
                }
                return instance;
            }
        }

        public void SetName(string dataProviderName)
        {
            dpName = dataProviderName;
        }

        /// <summary>
        /// Trả về .NET Framework DataProvider mà client đã lựa chọn kết nối
        /// </summary>
        public DbProviderFactory Factory() => 
            dpName == String.Empty || dpName == null
            ? throw new Exception("The name of SEPDataProvider is not right!")
            : DbProviderFactories.GetFactory(dpName);
    }
}
