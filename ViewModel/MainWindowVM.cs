using DataBaseService.Service;
using Model.DataModel;
using MVVM.RelayCommand;
using MVVM.VMBase;
using System.Collections.ObjectModel;

namespace VM.MainWindow
{
    internal class MainWindowVM : VMBase
    {
        private ObservableCollection<DataModel?> data;
        private DataModel? selectedItem;
        private readonly Service dbService;
        private DataModel? newData;

        public ObservableCollection<DataModel?> Data
        {
            get => data;
            set => SetProperty(ref data, value);
        }

        public DataModel? SelectedItem
        {
            get => selectedItem;
            set
            {
                SetProperty(ref selectedItem, value);
                if (selectedItem != null)
                {
                    NewData!.Marke = selectedItem.Marke;
                    NewData!.Modell = selectedItem.Modell;
                    NewData!.Baujahr = selectedItem.Baujahr;
                    NewData!.KM_Stand = selectedItem.KM_Stand;
                    NewData!.Preis = selectedItem.Preis;
                }
            }
        }

        public DataModel? NewData
        {
            get => newData;
            set => SetProperty(ref newData, value);
        }

        public RelayCommand CreateCommand => new RelayCommand(execute => Create(), canExecute => CanCreate());
        public RelayCommand UpdateCommand => new RelayCommand(execute => UpdateSelectedItem(), canExecute => CanUpdate());
        public RelayCommand DeleteCommand => new RelayCommand(execute => DeleteSelectedItem(), canExecute => CanDelete());

        private bool CanCreate() => string.IsNullOrEmpty(NewData?.Marke) &&
                                    string.IsNullOrEmpty(NewData?.Modell) &&
                                    NewData.KM_Stand >= 0 &&
                                    NewData.Preis >= 0;

        private bool CanUpdate() => SelectedItem != null;

        private bool CanDelete() => SelectedItem != null;

        public MainWindowVM()
        {
            dbService = new Service();
            NewData = new DataModel();
            Data = new ObservableCollection<DataModel?>(dbService.ReadData()!);
        }

        public void Create()
        {
            if (CanCreate())
            {
                dbService.CreateRow(NewData!); //Neue Zeile in der DB
                Data.Add(NewData); // Neuer Eintrag in der UI
                NewData = new DataModel(); // Neues Objekt für weitere Eingaben
            }
        }

        private void UpdateSelectedItem()
        {
            if (CanUpdate())
            {
                dbService.UpdateRow(SelectedItem!); // Zeilen Updaten in der DB
                int index = Data.IndexOf(SelectedItem!); // Zeilen Updaten in der UI
                if (index >= 0)
                {
                    Data[index] = new DataModel
                    {
                        Marke = SelectedItem!.Marke,
                        Modell = SelectedItem!.Modell,
                        KM_Stand = SelectedItem!.KM_Stand,
                        Preis = SelectedItem!.Preis,
                        Baujahr = SelectedItem!.Baujahr
                    };
                }
            }
        }

        private void DeleteSelectedItem()
        {
            if (CanDelete())
            {
                dbService.DeleteRow(SelectedItem!.Id); // Zeile aus der DB löschen
                Data.Remove(SelectedItem); // Zeile aus der UI löschen
                SelectedItem = null; // Auswahl zurücksetzen
            }
        }
    }
}