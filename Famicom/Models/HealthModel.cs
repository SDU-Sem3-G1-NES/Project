using SharedModels;

namespace Famicom.Models
{
    public class HealthModel
    {
        public string HealthData { get; private set; } = "Mock health data";

        public void FetchHealthData()
        {
            // Logic to get health data from backend
            HealthData = "Fetched health data from backend";
        }
    }
}
