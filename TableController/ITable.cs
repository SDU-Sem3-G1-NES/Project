namespace Famicom.TableController
{
    public interface ITable
    {
        int GUID { get; set; }
        string Name { get; set; }
        string Manufacturer { get; }
        int Height { get; }
        string Status { get; set; }
    }
}