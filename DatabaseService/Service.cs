using DataBaseService.CProvider;
using Model.DataModel;
using MVVM.VMBase;
using MySql.Data.MySqlClient;

namespace DataBaseService.Service
{
    internal class Service : VMBase
    {
        private ConnectionService db = new ConnectionService("localhost", "root", "Root", "car");

        #region CreateRow()

        /// <summary>
        /// Hinzufügen einer Zeile in der DB
        /// </summary>
        /// <param name="auto"></param>
        public void CreateRow(Car auto)
        {
            string query = "INSERT INTO autos (Marke,Modell,Baujahr,KM_Stand,Preis) VALUES(@Marke,@Modell,@Baujahr,@KM_Stand,@Preis)";

            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Marke", auto.Marke);
                    cmd.Parameters.AddWithValue("@Modell", auto.Modell);
                    cmd.Parameters.AddWithValue("@Baujahr", auto.Baujahr.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@KM_Stand", auto.KM_Stand);
                    cmd.Parameters.AddWithValue("@Preis", auto.Preis);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        #endregion CreateRow()

        #region UpdateRow()

        /// <summary>
        /// Aktuallisieren von zeilen in der DB
        /// </summary>
        /// <param name="auto"></param>
        public void UpdateRow(Car auto)
        {
            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = "UPDATE autos SET Marke = @Marke, Modell = @Modell, Baujahr = @Baujahr, KM_Stand = @KM_Stand, Preis = @Preis WHERE AID = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", auto.Id);
                    cmd.Parameters.AddWithValue("@Marke", auto.Marke);
                    cmd.Parameters.AddWithValue("@Modell", auto.Modell);
                    cmd.Parameters.AddWithValue("@Baujahr", auto.Baujahr.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@KM_Stand", auto.KM_Stand);
                    cmd.Parameters.AddWithValue("@Preis", auto.Preis);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        #endregion UpdateRow()

        #region DeleteRow()

        /// <summary>
        /// Löschen der Zeile in der DB
        /// </summary>
        /// <param name="id"></param>
        public void DeleteRow(int id)
        {
            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM autos WHERE AID = @id";
                using (var command = new MySqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        #endregion DeleteRow()

        #region RedData()

        /// <summary>
        /// Daten aus der DB laden
        /// </summary>
        /// <returns></returns>
        public List<Car> ReadData()
        {
            var data = new List<Car>();
            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = "SELECT AID, Marke, Modell, Baujahr, KM_Stand, Preis FROM autos";
                using (MySqlCommand command = new MySqlCommand(query, conn))

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        data.Add(new Car
                        {
                            Id = reader.GetInt32("AID"),
                            Marke = reader.GetString("Marke"),
                            Modell = reader.GetString("Modell"),
                            Baujahr = reader.GetDateTime("Baujahr"),
                            KM_Stand = reader.GetInt32("KM_Stand"),
                            Preis = reader.GetDouble("Preis")
                        });
                    }
                }
            }
            return data;
        }

        #endregion RedData()
    }
}