namespace Model.DataModel
{
    internal class DataModel
    {
        public int Id { get; set; }
        public string? Marke { get; set; }
        public string? Modell { get; set; }
        public int KM_Stand { get; set; }
        public double Preis { get; set; }
        public DateTime Baujahr { get; set; }
    }
}