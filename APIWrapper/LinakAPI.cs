namespace APIWrapper
{
    public class LinakAPI : IAPIWrapper
    {
        string Manufacturer { get; set; }
        public ITable Table { get; set; }

        void OnCollisionError()
        {
            
        }

        void OnActivation()
        {
            
        }

        public float GetTableHeight()
        {
            return 0.0f;
        }

        public float SetTableHeight()
        {
            return 0.0f;
        }

        public void OnScheduleReceived()
        {
            
        }
    }
}
