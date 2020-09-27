/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: NFine
 * Description: NFine快速开发平台
 * Website：http://www.nfine.cn
*********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Code
{
    public class DBConnection
    {
        public static bool Encrypt { get; set; }
        public DBConnection(bool encrypt)
        {
            Encrypt = encrypt;
        }
        public static string connectionString
        {
            get
            {
                string connection = System.Configuration.ConfigurationManager.ConnectionStrings["NFineDbContext"].ConnectionString;
                if (Encrypt == true)
                {
                    return DESEncrypt.Decrypt(connection);
                }
                else
                {
                    return connection;
                }
            }
        }

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="connectionName">连接字符串名称</param>
        /// <param name="encrypt">是否加密</param>
        /// <returns></returns>
        public static string GetConnectionString(string connectionName = "", bool encrypt = false)
        {
            if(string.IsNullOrEmpty(connectionName))
            {
                connectionName = "NFineDbContext";
            }

            string connection = System.Configuration.ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;

            return encrypt ? DESEncrypt.Decrypt(connection) : connection;
        }

        //你都不懂fork怎么使用
    }
}
