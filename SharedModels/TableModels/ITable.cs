namespace SharedModels
{
    public interface ITable
    {
        string GUID { get; set; }
        string Name { get; set; }
        string Manufacturer { get; set; }
        int? Height { get; set; }
        int? Speed { get; set; }
        string? Status { get; set; }
    }
}