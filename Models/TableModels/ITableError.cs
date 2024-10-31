namespace Famicom.TableController
{
    public interface ITableError
    {
        DateTime TimeStamp { get; set; }
        string ErrorMessage { get; set; }
        int ErrorId { get; set; }
    }

}