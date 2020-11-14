namespace Baer.Code
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
                string connection = System.Configuration.ConfigurationManager.ConnectionStrings["BaerDbContext"].ConnectionString;
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
                connectionName = "BaerDbContext";
            }

            string connection = System.Configuration.ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;

            return encrypt ? DESEncrypt.Decrypt(connection) : connection;
        }
    }
}
