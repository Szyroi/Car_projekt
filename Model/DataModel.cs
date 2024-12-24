using MVVM.VMBase;

namespace Model.DataModel
{
    internal class DataModel : VMBase
    {
        private int id;
        private string? marke;
        private string? modell;
        private DateTime baujahr;
        private int km_Stand;
        private double preis;

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