namespace APIWrapper
{
    public interface IAPIWrapper
    {
        ITable Table { get; set; }
        float GetTableHeight();
        float SetTableHeight();
        void OnScheduleReceived();
    }
}