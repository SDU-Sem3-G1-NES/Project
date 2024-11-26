
namespace SharedModels
{
    public class Health
    {
        public int HealthId { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public int? PresetId { get; set; }
        public int Position { get; set; }

    }
}
