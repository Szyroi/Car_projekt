using System.Windows;
using Car_projekt.Helper;
using Helper.DBHelper;
using MySql.Data.MySqlClient;

namespace Car_projekt.DatabaseService
{
    public class AuthService
    {
        public static bool LoginUser(string username, string password)
        {
            try
            {
                using (var db = new DBHelper().GetConnection())
                {
                    db.Open();

                    // Abfrage für Benutzerinformationen
                    var query = "SELECT hash, salt FROM users WHERE username = @username";
                    using (var cmd = new MySqlCommand(query, db))
                    {
                        cmd.Parameters.AddWithValue("@username", username);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Benutzer gefunden, Hash und Salt abrufen
                                string storedHash = reader.GetString("hash");
                                string storedSalt = reader.GetString("salt");

                                // Passwort überprüfen
                                if (SecurityHelper.VerifyPassword(password, storedSalt, storedHash))
                                {
                                    return true; // Login erfolgreich
                                }
                            }
                        }
                    }
                }

                return false; // Benutzername oder Passwort falsch
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false; // Fehler beim Login
            }
        }

        public static bool RegisterUser(string username, string password)
        {
            try
            {
                // Passwort-Hash und Salt generieren
                string salt;
                string hash = SecurityHelper.HashPassword(password, out salt);

                // Verbindung zur Datenbank herstellen
                using (var db = new DBHelper().GetConnection())
                {
                    db.Open();
                    var query = "INSERT INTO users (username, hash, salt) VALUES (@username, @hash, @salt)";

                    using (var cmd = new MySqlCommand(query, db))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@hash", hash);
                        cmd.Parameters.AddWithValue("@salt", salt);

                        cmd.ExecuteNonQuery();
                    }
                }

                return true; // Erfolgreich registriert
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false; // Fehler bei der Registrierung
            }
        }
    }
}
