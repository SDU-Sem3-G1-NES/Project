using System.Threading.Tasks;

namespace Famicom.Models
{
    public class HealthModel
    {
        public string HealthData { get; private set; } = "Mock health data";

        public int UserHeightInCm { get; set; } = 170;

        public async Task FetchHealthDataAsync()
        {
            await Task.Delay(500); // Simulate network delay
            var minHeight = CalculateMinDeskHeight(UserHeightInCm);
            var maxHeight = CalculateMaxDeskHeight(UserHeightInCm);

            HealthData = $"Recommended desk height range: {minHeight:F1} cm - {maxHeight:F1} cm.";
        }

        public double CalculateMinDeskHeight(int heightInCm)
        {
            return 1.2037 * heightInCm - 16.9633;
        }

        public double CalculateMaxDeskHeight(int heightInCm)
        {
            return 1.4067 * heightInCm - 23.9376;
        }
    }
}
