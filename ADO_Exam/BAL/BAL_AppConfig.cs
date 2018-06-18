using System.Configuration;
using System.Data.SqlClient;

namespace ADO_Exam
{
    public static partial class BAL
    {
        private static Configuration config = null;
        private static string currentConnectionString = null;

        public static void UpdateCurrentConfigAndConnectionStrings()
        {
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            currentConnectionString = ((ConnectionStringsSection)config.GetSection("connectionStrings")).ConnectionStrings["DB"].ConnectionString;
        }

        public static void ChangeAppConfig(string dataSource, string initialCatalog, string userID, string password)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = dataSource,
                InitialCatalog = initialCatalog,
                IntegratedSecurity = true,
                ApplicationName = "EntityFramework",
                MultipleActiveResultSets = true,
                Password = password,
                UserID = userID,
                ConnectTimeout = 5
            };

            ((ConnectionStringsSection)config.GetSection("connectionStrings")).ConnectionStrings["DB"].ConnectionString = builder.ToString();
            config.Save();

            ConfigurationManager.RefreshSection("connectionStrings");
        }
    }
}
