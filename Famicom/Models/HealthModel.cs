namespace Famicom.Models
{
    public class HealthModel
    {
        public string HealthData { get; private set; } = "Mock health data";

        public int UserHeightInCm { get; set; } = 0;

        public async Task FetchHealthDataAsync()
        {
            await Task.Delay(500); // Simulate network delay
            var minHeight = CalculateMinDeskHeight(UserHeightInCm);
            var maxHeight = CalculateMaxDeskHeight(UserHeightInCm);

            HealthData = $"Recommended desk height range: {minHeight:F1} cm - {maxHeight:F1} cm.";
        }

        public int CalculateMinDeskHeight(int heightInCm)
        {
            return (int)(0.48 * heightInCm);
        }

        public int CalculateMaxDeskHeight(int heightInCm)
        {
            return (int)(0.56 * heightInCm);
        }
    }
}
