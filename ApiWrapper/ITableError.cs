namespace APIWrapper
{
    public interface ITableError
    {
        DateTime TimeStamp { get; set; }
        string ErrorMessage { get; set; }
        int ErrorId { get; set; }
    }
}