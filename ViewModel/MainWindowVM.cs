using DataBaseService.CProvider;
using DataBaseService.Service;
using Model.DataModel;
using MVVM.RelayCommand;
using MVVM.VMBase;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;

namespace VM.MainWindow
{
    internal class MainWindowVM : VMBase
    {
        private int id;
        private string marke;
        private string modell;
        private DateTime baujahr;
        private int km_stand;
        private double preis;

        public int Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        public string Marke
        {
            get => marke;
            set => SetProperty(ref marke, value);
        }

        public string Modell
        {
            get => modell;
            set => SetProperty(ref modell, value);
        }

        public DateTime Baujahr
        {
            get => baujahr;
            set => SetProperty(ref baujahr, value);
        }

        public int KM_Stand
        {
            get => km_stand;
            set => SetProperty(ref km_stand, value);
        }

        public double Preis
        {
            get => preis;
            set => SetProperty(ref preis, value);
        }

        private ObservableCollection<DataModel> data;

        public ObservableCollection<DataModel> Data

        {
            get => this.data;
            set => SetProperty(ref data, value);
        }

        private readonly Service DBservice;

        public ObservableCollection<DataModel> Items { get; set; }

        private DataModel selectedItem;

        public DataModel SelectedItem
        {
            get => selectedItem;
            set => SetProperty(ref selectedItem, value);
        }

        public RelayCommand LoadDataCommand => new RelayCommand(execute => LoadData());

        public RelayCommand DeleteCommand => new RelayCommand(execute => DeleteSelectedItem());
        public RelayCommand SaveCommand => new RelayCommand(execute => Save(), canExecute => CanSave());

        private ConnectionService db = new ConnectionService("localhost", "root", "root", "car");

        public MainWindowVM()
        {
            Data = new ObservableCollection<DataModel>();
            DBservice = new Service();
            Items = new ObservableCollection<DataModel>(DBservice.GetItems());
            Baujahr = DateTime.Now.Date;
            LoadData();
        }

        public void LoadData()
        {
            Data.Clear();
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
                            Data.Add(new DataModel
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
        }

        public void Save()
        {
            string query = "INSERT INTO autos (Marke,Modell,Baujahr,KM_Stand,Preis) VALUES(@Marke,@Modell,@Baujahr,@KM_Stand,@Preis)";

            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Marke", Marke);
                    cmd.Parameters.AddWithValue("@Modell", Modell);
                    cmd.Parameters.AddWithValue("@Baujahr", Baujahr.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@KM_Stand", KM_Stand);
                    cmd.Parameters.AddWithValue("@Preis", Preis);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void DeleteSelectedItem()
        {
            if (SelectedItem == null)
            {
                return;
            }

            // Zeile aus der MySQL-Datenbank löschen
            DBservice.DeleteRow(SelectedItem.Id);

            // Zeile aus der ObservableCollection entfernen
            Items.Remove(SelectedItem);

            SelectedItem = null; // Auswahl zurücksetzen
        }

        private bool CanSave()
        {
            //Check
            return true;
        }
    }
}