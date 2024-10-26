namespace Famicom.TableController
{
    public class LinakTable : ITable
    {
        public int GUID { get; set; } 
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public float Height { get; set; }
        public string Status { get; set; }
        public float Speed { get; set; }
        public List<ITableError> ErrorList { get; set; }
        public int ActivationCounter { get; set; }
        public int SitStandCounter { get; set; }
    }
}