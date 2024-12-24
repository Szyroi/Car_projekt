using DataBaseService.CProvider;
using Model.DataModel;
using MVVM.VMBase;
using MySql.Data.MySqlClient;

namespace DataBaseService.Service
{
    internal class Service : VMBase
    {
        private ConnectionService db = new ConnectionService("localhost", "root", "root", "car");

        private int id;

        public int Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

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