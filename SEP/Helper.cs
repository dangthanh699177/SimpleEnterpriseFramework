using SEP.Authentication.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEP
{
    public class Helper
    {
        //public enum DbName
        //{
        //    SQLServer,
        //    MySQL,
        //    Oracle,
        //    Unknow
        //}

        public enum CryptType
        {
            Base64,
            Sha256
        }

        //public static DbName GetDbType(string dbName)
        //{
        //    switch(dbName.ToLower())
        //    {
        //        case "sqlserver":
        //            return DbName.SQLServer;
        //        case "mysql":
        //            return DbName.MySQL;
        //        case "oracle":
        //            return DbName.Oracle;
        //        default:
        //            return DbName.Unknow;
        //    }
        //}

        public static EncryptContext GetEncrytpType(CryptType type)
        {
            EncryptContext encrypt = new EncryptContext();
            switch (type)
            {
                case CryptType.Base64:
                    encrypt.SetEncrypt(new Base64Encrypt());
                    break;
                case CryptType.Sha256:
                    encrypt.SetEncrypt(new Sha256Encrypt());
                    break;
            }
            return encrypt;
        }
        public static DecryptContext GetDecrytpType(CryptType type)
        {
            DecryptContext decrypt = new DecryptContext();
            switch (type)
            {
                case CryptType.Base64:
                    decrypt.SetDecrypt(new Base64Decrypt());
                    break;
                case CryptType.Sha256:
                    decrypt.SetDecrypt(new Sha256Decrypt());
                    break;
            }
            return decrypt;
        }
    }
}
