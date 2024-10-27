namespace Famicom.Components.Classes
{
    public class Presets
    {
        public int PresetId { get; set; }
        public string PresetName { get; set; }
        public int UserId { get; set; }
        public int Height { get; set; }
        public string Options { get; set; }

        public Presets(int presetId, string presetName, int userId, int height, string options)
        {
            PresetId = presetId;
            PresetName = presetName;
            UserId = userId;
            Height = height;
            Options = options;
        }
    }
}
