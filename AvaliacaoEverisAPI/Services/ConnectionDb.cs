using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Services
{
    public class ConnectionDb
    {
        public static string connectionString { get; set; }
        public static string providerName { get; set; }

        static ConnectionDb()
        {
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["everis"].ConnectionString;
                providerName = ConfigurationManager.ConnectionStrings["everis"].ProviderName;
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao se conectar no banco de dados");
            }
        }

        public static string ConnectionString
        {
            get { return connectionString; }
        }

        public static string ProviderName
        {
            get { return providerName; }
        }
    }
}