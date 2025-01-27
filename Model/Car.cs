using MVVM.VMBase;

namespace Model.DataModel
{
    /// <summary>
    /// POCOS
    /// </summary>
    internal class Car : VMBase
    {
        /// <summary>
        /// Private Backing Fields
        /// </summary>

        private int id;
        private string? marke;
        private string? modell;
        private DateTime baujahr = DateTime.Now; // DateTime.Now ist der Standartwert
        private int km_Stand;
        private double preis;

        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        public string? Marke
        {
            get => marke;
            set
            {
                marke = value;
                OnPropertyChanged();
            }
        }

        public string? Modell
        {
            get => modell;
            set
            {
                modell = value;
                OnPropertyChanged();
            }
        }

        public DateTime Baujahr
        {
            get => baujahr;
            set
            {
                baujahr = value;
                OnPropertyChanged();
            }
        }

        public int KM_Stand
        {
            get => km_Stand;
            set
            {
                km_Stand = value;
                OnPropertyChanged();
            }
        }

        public double Preis
        {
            get => preis;
            set
            {
                preis = value;
                OnPropertyChanged();
            }
        }
    }
}
