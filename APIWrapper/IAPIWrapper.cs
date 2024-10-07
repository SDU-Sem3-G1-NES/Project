namespace APIWrapper
{
    public interface IAPIWrapper
    {
        float GetTableHeight(ITable Table);
        float SetTableHeight(ITable Table);
        void OnScheduleReceived();
    }
}