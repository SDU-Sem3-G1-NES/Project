namespace Famicom.Models
{
    public class HealthModel
    {
        public int SittingHeightPreset { get; set; } = 70; // Default sitting desk height in cm
        public int StandingHeightPreset { get; set; } = 110; // Default standing desk height in cm

        public string HealthData { get; private set; } = string.Empty;
        public async Task FetchHealthDataAsync()
        {
            await Task.Delay(500); // Simulate network delay
            HealthData = $"Sitting Desk Height: {SittingHeightPreset} cm, Standing Desk Height: {StandingHeightPreset} cm.";
        }

        public bool ValidatePresets(out string validationMessage)
        {
            if (SittingHeightPreset < 50 || SittingHeightPreset > 100)
            {
                validationMessage = "Sitting height should be between 50 cm and 100 cm.";
                return false;
            }

            if (StandingHeightPreset < 90 || StandingHeightPreset > 130)
            {
                validationMessage = "Standing height should be between 90 cm and 130 cm.";
                return false;
            }

            if (SittingHeightPreset >= StandingHeightPreset)
            {
                validationMessage = "Sitting height should be less than standing height.";
                return false;
            }

            validationMessage = "Presets are valid.";
            return true;
        }

        public void ResetPresets()
        {
            SittingHeightPreset = 70;
            StandingHeightPreset = 110;
        }
    }
}
