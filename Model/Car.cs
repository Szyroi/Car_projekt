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

        /// <summary>
        /// Public Props SetProperty Methode Benachrichtigt die UI bei Veränderungen.
        /// </summary>

        public int Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        public string? Marke
        {
            get => marke;
            set => SetProperty(ref marke, value);
        }

        public string? Modell
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
            get => km_Stand;
            set => SetProperty(ref km_Stand, value);
        }

        public double Preis
        {
            get => preis;
            set => SetProperty(ref preis, value);
        }
    }
}
