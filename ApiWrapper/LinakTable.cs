namespace Famicom.ApiWrapper
{
    public class LinakTable : ITable
    {
        public List<ITableError> ErrorList { get; set; }
        public int GUID { get; set; }
        public string Name { get; set; }
        public float Height { get; set; }
        public Status Status { get; set; }
    }
}