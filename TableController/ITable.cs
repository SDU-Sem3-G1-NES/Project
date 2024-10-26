namespace Famicom.TableController
{
    public interface ITable
    {
        int GUID { get; set; }
        string Name { get; set; }
        string Manufacturer { get; set; }
        float Height { get; set; }
        string Status { get; set; }
    }
}