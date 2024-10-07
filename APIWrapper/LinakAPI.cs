namespace APIWrapper
{
    public class LinakAPIWrapper : IAPIWrapper
    {
        string Manufacturer { get; set; }
        public LinakAPIWrapper()
        {
            Manufacturer = "Linak";
        }

        void OnCollisionError()
        {
            throw new NotImplementedException();
        }

        void OnActivation()
        {
            throw new NotImplementedException();
        }

        public float GetTableHeight()
        {
            throw new NotImplementedException();
        }

        public float SetTableHeight()
        {
            throw new NotImplementedException();
        }

        public void OnScheduleReceived()
        {
            throw new NotImplementedException();
        }
    }
}
