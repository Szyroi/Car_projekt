using System.Collections;
using DataBaseService.Service;
using Model.DataModel;
using MVVM.RelayCommand;
using MVVM.VMBase;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Helper.ValidationHelper;


namespace VM.MainWindow
{
    /// <summary>
    /// ViewModel vom MainWindow
    /// </summary>
    internal class DashboardVM : VMBase
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
                newData = value;
                OnPropertyChanged();
                ValidateNewData();
            }
        }


        public Car SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                OnPropertyChanged();
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

        public ValidationHelper Helper { get; } = new ValidationHelper();

        public bool HasErrors => Helper.HasErrors;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string? propertyName) => Helper.GetErrors(propertyName);

        public DashboardVM()
        {
            dbService = new Service();
            NewData = new Car();
            Data = new ObservableCollection<Car>(dbService.ReadData()!);
            Helper.ErrorsChanged += (_, e) =>
            {
                OnPropertyChanged();
                ErrorsChanged?.Invoke(this, e);
            };
        }

        #endregion Constructor


        #region Methoden


        private void ValidateNewData()
        {
            Helper.ClearErrors();

            if (string.IsNullOrWhiteSpace(NewData.Marke))
            {
                Helper.AddError("Bitte geben sie valide Zeichen ein!");
            }
            if (string.IsNullOrEmpty(NewData.Modell))
            {
                Helper.AddError("Bitte geben Sie einen Modell ein!");
            }
            if (NewData.Baujahr.Year < 1900)
            {
                Helper.AddError("Bitte geben Sie einen Baujahr ein!");
            }
            if (NewData.KM_Stand <= 0)
            {
                Helper.AddError("Bitte geben Sie einen KM_Stand ein!");
            }
            if (NewData.Preis <= 0)
            {
                Helper.AddError("Bitte geben Sie einen Preis ein!");
            }
        }




        public void CreateItem()
        {
            dbService.CreateRow(NewData); // Datensatz wird in die DB eingefügt
            Data.Add(NewData); // Datensatz wird in der UI hinzugefügt
            NewData = new Car(); // Neues leeres Car-Objekt
        }


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
    }
}
