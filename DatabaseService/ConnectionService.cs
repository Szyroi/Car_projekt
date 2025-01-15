using MySql.Data.MySqlClient;

namespace DataBaseService.CProvider
{
    /// <summary>
    /// Helper Class für Globel verbindung in den datein
    /// </summary>
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