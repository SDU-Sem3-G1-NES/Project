namespace SharedModels
{
    public interface ITable
    {
        string GUID { get; set; }
        string Name { get; set; }
        string Manufacturer { get; set; }
        int? Height { get; }
    }
}