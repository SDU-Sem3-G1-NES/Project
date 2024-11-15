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
            HealthData = $"Fetched health data from backend. Recommended desk height: {CalculateDeskHeight(UserHeightInCm)} cm.";
        }

        public int CalculateDeskHeight(int heightInCm)
        {
            return heightInCm + 1;
        }
    }
}
