using MySql.Data.MySqlClient;

namespace DataBaseService.CProvider
{
    public class ConnectionService
    {
        private string connectionString { get; set; }

        public ConnectionService(string server, string uid, string password, string database)
        {
            this.connectionString = $"server={server};uid={uid};pwd={password};database={database}";
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}