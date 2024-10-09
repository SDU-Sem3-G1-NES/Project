namespace Famicom.ApiWrapper
{
    public class LinakAPIWrapper : IApiWrapper
    {
        string Manufacturer { get; set; }
        public LinakAPIWrapper()
        {
            Manufacturer = "LINAK";
        }

        void OnCollisionError()
        {
            throw new NotImplementedException();
        }

        void OnActivation()
        {
            throw new NotImplementedException();
        }

        public float GetTableHeight(ITable Table)
        {
            throw new NotImplementedException();
        }

        public float SetTableHeight(ITable Table)
        {
            throw new NotImplementedException();
        }

        public void OnScheduleReceived()
        {
            throw new NotImplementedException();
        }
    }
}
