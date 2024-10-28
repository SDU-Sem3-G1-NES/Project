namespace Famicom.TableController
{
    public class LinakTable : ITable
    {
        public LinakTable(int guid, string name, int height, string status)
        {
            GUID = guid;
            Name = name;
            Height = height;
            Status = status;
            Manufacturer = "Linak";
            SitStandCount = new SitStandCounter
            {
                SitCount = 0,
                StandCount = 0
            };
        }
        public int GUID { get; set; } 
        public string Name { get; set; }
        public string Manufacturer { get; private set; }
        public int Height { get; private set; }
        public string Status { get; set; }
        public float Speed { get; set; }
        public List<ITableError>? ErrorList { get; set; }
        public int ActivationCounter { get; set; }
        public SitStandCounter SitStandCount { get; set; }
    }

    public class SitStandCounter : ISitStandCounter
    {
        public int SitCount { get; set; }
        public int StandCount { get; set; }
    }
}