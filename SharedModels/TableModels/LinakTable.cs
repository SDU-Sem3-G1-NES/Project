namespace SharedModels
{
    public class LinakTable : ITable
    {
        public LinakTable(string guid, string name, string? manufacturer = null)
        {
            GUID = guid;
            Name = name;
            if (manufacturer != null)
            {
                Manufacturer = manufacturer;
            }
            else
            {
                Manufacturer = "Linak A/S";
            }
        }
        public string GUID { get; set; } 
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public int? Height { get; set; }
        public int? Speed { get; set; }
        public List<ITableError>? ErrorList { get; set; }
        public int ActivationCounter { get; set; }
    }
}