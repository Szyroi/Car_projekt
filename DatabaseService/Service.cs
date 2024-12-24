using DataBaseService.CProvider;
using Model.DataModel;
using MVVM.VMBase;
using MySql.Data.MySqlClient;

namespace DataBaseService.Service
{
    internal class Service : VMBase
    {
        private ConnectionService db = new ConnectionService("localhost", "root", "root", "car");

        // Löschen der Zeile in  der MySQL Datenbank
        public void DeleteRow(int id)
        {
            using (MySqlConnection connection = db.GetConnection())
            {
                connection.Open();
                string query = "DELETE FROM autos WHERE AID = @id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        //Aktuallisieren einer zeilen
        public void UpdateRow(int id, string marke, string modell, DateTime baujahr, int km_stand, double preis)
        {
            using (MySqlConnection connection = db.GetConnection())
            {
                connection.Open();
                string query = "UPDATE autos SET Marke = @Marke, Modell = @Modell, Baujahr = @Baujahr, KM_Stand = @KM_Stand, Preis = @Preis WHERE AID = @id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@Marke", marke);
                    command.Parameters.AddWithValue("@Modell", modell);
                    command.Parameters.AddWithValue("@Baujahr", baujahr.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@KM_Stand", km_stand);
                    command.Parameters.AddWithValue("@Preis", preis);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Daten aus der MySQL-Datenbank laden
        public List<DataModel> GetItems()
        {
            var items = new List<DataModel>();
            using (MySqlConnection connection = db.GetConnection())
            {
                connection.Open();
                string query = "SELECT AID, Marke, Modell, Baujahr, KM_Stand, Preis FROM autos";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(new DataModel
                            {
                                Id = Convert.ToInt32(reader["AID"]),
                                Marke = reader["Marke"].ToString(),
                                Modell = reader["Modell"].ToString(),
                                Baujahr = Convert.ToDateTime(reader["Baujahr"]),
                                KM_Stand = Convert.ToInt32(reader["KM_Stand"]),
                                Preis = Convert.ToDouble(reader["Preis"])
                            });
                        }
                    }
                }
            }
            return items;
        }
    }
}