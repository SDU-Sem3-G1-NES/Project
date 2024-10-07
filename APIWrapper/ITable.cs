namespace APIWrapper
{
    public interface ITable
    {
        int GUID { get; set; }
        string Name { get; set; }
        float Height { get; set; }
        Status Status { get; set; }
    }
}
