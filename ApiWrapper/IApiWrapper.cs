namespace Famicom.ApiWrapper
{
    public interface IApiWrapper
    {
        float GetTableHeight(ITable Table);
        float SetTableHeight(ITable Table);
        void OnScheduleReceived();
    }
}