using DataBaseService.Service;
using Model.DataModel;
using MVVM.RelayCommand;
using MVVM.VMBase;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace VM.MainWindow
{
    /// <summary>
    /// ViewModel vom MainWindow
    /// </summary>
    internal class DashboardVM : VMBase, INotifyDataErrorInfo
    {
        #region Fields und Props

        public ObservableCollection<Car> Data { get; set; }
        private Car selectedItem;
        private Car newData;
        private readonly Service dbService;


        public Car NewData
        {
            get => newData;
            set
            {
                if (newData != value)
                {
                    newData = value;
                    OnPropertyChanged();
                }
            }
        }


        public Car SelectedItem
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

        #endregion Fields und Props

        #region Button Commands

        public RelayCommand CreateCommand => new RelayCommand(execute => CreateItem());
        public RelayCommand UpdateCommand => new RelayCommand(execute => UpdateSelectedItem(), canExecute => CanUpdate());
        public RelayCommand DeleteCommand => new RelayCommand(execute => DeleteSelectedItem(), canExecute => CanDelete());
        public RelayCommand LoadCommand => new RelayCommand(execute => LoadItems());

        private bool CanUpdate() => SelectedItem != null;

        private bool CanDelete() => SelectedItem != null;

        #endregion Button Commands

        #region Constructor

        public DashboardVM()
        {
            dbService = new Service();
            NewData = new Car();
            Data = new ObservableCollection<Car>(dbService.ReadData()!);
        }

        #endregion Constructor

        #region Methoden

        /// <summary>
        /// Logik zum Erstellen neuer Datensätze in der UI & DB
        /// </summary>
        public void CreateItem()
        {
            ValidateNewData(); // Validierung der Daten

            if (HasErrors)
            {
                MessageBox.Show("Es gibt Fehler in den Eingabedaten. Bitte korrigieren Sie diese.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Fehler verhindern das Erstellen des Datensatzes
            }

            dbService.CreateRow(NewData); // Datensatz wird in die DB eingefügt
            Data.Add(NewData); // Datensatz wird in der UI hinzugefügt
            NewData = new Car(); // Neues leeres Car-Objekt
        }

        /// <summary>
        /// Logik für das Updaten von Datensätzen in der DB
        /// </summary>
        private void UpdateSelectedItem()
        {
            if (CanUpdate())
            {
                dbService.UpdateRow(SelectedItem!);
                int index = Data.IndexOf(SelectedItem!);
                if (index >= 0)
                {
                    Data[index] = new Car
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

        /// <summary>
        /// Löschen der Spalte in der UI & DB
        /// </summary>
        private void DeleteSelectedItem()
        {
            if (CanDelete())
            {
                dbService.DeleteRow(SelectedItem!.Id);
                Data.Remove(SelectedItem);
                SelectedItem = null;
            }
        }

        private void LoadItems()
        {
            // Lade die Daten erneut aus der Datenbank
            Data.Clear();
            var newData = dbService.ReadData();
            if (newData != null)
            {
                foreach (var car in newData)
                {
                    Data.Add(car);
                }
            }
        }

        #endregion Methoden

        #region INotifyDataErrorInfo Implementierung

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        // Speichern der Errors
        private readonly Dictionary<string, List<string>> errors = new();

        public bool HasErrors => errors.Any();

        public IEnumerable GetErrors(string propertyName)
        {
            if (errors.ContainsKey(propertyName))
            {
                return errors[propertyName];
            }
            return null;
        }

        // Validierungslogik
        private void ValidateNewData()
        {
            ClearErrors(); // Löschen aller Fehler

            // Validierungen der Felder
            if (string.IsNullOrWhiteSpace(NewData?.Marke))
            {
                AddError(nameof(NewData.Marke), "Marke darf nicht leer sein.");
            }

            if (string.IsNullOrWhiteSpace(NewData?.Modell))
            {
                AddError(nameof(NewData.Modell), "Modell darf nicht leer sein.");
            }

            if (NewData?.Baujahr == null)
            {
                AddError(nameof(NewData.Baujahr), "Baujahr darf nicht null sein.");
            }

            if (NewData?.KM_Stand == null || NewData.KM_Stand < 0)
            {
                AddError(nameof(NewData.KM_Stand), "Kilometerstand muss eine positive Zahl sein.");
            }

            if (NewData?.Preis == null || NewData.Preis <= 0)
            {
                AddError(nameof(NewData.Preis), "Preis muss eine positive Zahl sein.");
            }

            // Benachrichtige UI über Fehleränderungen
            OnErrorsChanged(nameof(NewData));
        }

        // Fehler für eine Eigenschaft hinzufügen
        private void AddError(string propertyName, string errorMessage)
        {
            if (!errors.ContainsKey(propertyName))
            {
                errors[propertyName] = new List<string>();
            }

            errors[propertyName].Add(errorMessage);
        }

        // Fehler für eine Eigenschaft löschen
        private void ClearErrors()
        {
            errors.Clear();
        }

        // Event auslösen, wenn Fehler geändert werden
        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        #endregion INotifyDataErrorInfo Implementierung
    }
}
