namespace SharedModels
{
    public class LinakTable : ITable
    {
        public LinakTable(string guid, string name)
        {
            GUID = guid;
            Name = name;
            Manufacturer = "Linak A/S";
        }
        public string GUID { get; set; } 
        public string Name { get; set; }
        public string Manufacturer { get; private set; }
        public int? Height { get; private set; }
        public float Speed { get; set; }
        public List<ITableError>? ErrorList { get; set; }
        public int ActivationCounter { get; set; }
    }
}