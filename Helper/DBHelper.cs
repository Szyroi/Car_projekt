using MySql.Data.MySqlClient;

namespace Helper.DBHelper
{
    public class DBHelper
    {
        private string connectionString = $"server=localhost;uid=root;pwd=root;database=user_data;";

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}
