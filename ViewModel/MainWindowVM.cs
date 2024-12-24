using DataBaseService.Service;
using Model.DataModel;
using MVVM.RelayCommand;
using MVVM.VMBase;
using System.Collections.ObjectModel;

namespace VM.MainWindow
{
    internal class MainWindowVM : VMBase
    {
        private ObservableCollection<DataModel> data;

        public ObservableCollection<DataModel> Data

        {
            get => this.data;
            set => SetProperty(ref data, value);
        }

        private DataModel selectedItem;

        public DataModel SelectedItem
        {
            get => selectedItem;
            set => SetProperty(ref selectedItem, value);
        }

        private readonly Service dbService;
        public DataModel NewData { get; set; }

        public RelayCommand CreateCommand => new RelayCommand(execute => Create(), execute => CanCreate());
        public RelayCommand UpdateCommand => new RelayCommand(execute => UpdateSelectedItem(), canExecute => CanUpdate());
        public RelayCommand DeleteCommand => new RelayCommand(execute => DeleteSelectedItem(), canExecute => CanDelete());

        private bool CanCreate() => !string.IsNullOrEmpty(NewData.Marke) &&
                               !string.IsNullOrEmpty(NewData.Modell) &&
                               NewData.KM_Stand > 0 &&
                               NewData.Preis > 0;

        private bool CanUpdate() => SelectedItem != null;

        private bool CanDelete() => SelectedItem != null;

        public MainWindowVM()
        {
            dbService = new Service();
            NewData = new DataModel();
            Data = new ObservableCollection<DataModel>(dbService.ReadData());
            NewData = new DataModel { Baujahr = DateTime.Now };
        }

        public void Create()
        {
            if (CanCreate())
            {
                dbService.CreateRow(NewData); //Neue Zeile in der DB
                Data.Add(NewData); // Neuer Eintrag in der UI
                NewData = new DataModel(); // Neues Objekt für weitere Eingaben
            }
        }

        private void UpdateSelectedItem()
        {
            if (CanUpdate())
            {
                // Zeile aktualisieren
                dbService.UpdateRow(SelectedItem);
                int index = Data.IndexOf(SelectedItem);
                Data[index] = SelectedItem;
            }
        }

        private void DeleteSelectedItem()
        {
            if (CanDelete())
            {
                // Zeile aus der MySQL-Datenbank löschen
                dbService.DeleteRow(SelectedItem.Id);

                // Zeile aus der ObservableCollection entfernen
                Data.Remove(SelectedItem);

                SelectedItem = null; // Auswahl zurücksetzen
            }
        }
    }
}