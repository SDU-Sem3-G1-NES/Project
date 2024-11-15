using SharedModels;
using Models.Services;

namespace Famicom.Models
{
    public class PresetsModel
    {
        private readonly PresetService presetService;
        public PresetsModel()
        {
            this.presetService = new PresetService();
        }
        public List<Presets> GetAllPresets(int userId)
        {
            return presetService.GetPresetsUser(userId);
        }
        public void AddPreset(string name, int user, int height, string options, string icon)
        {
            presetService.AddPreset(name, user, height, options, icon);
        }

        public void EditPresetName(int presetId, string presetName)
        {
            presetService.UpdatePresetName(presetId, presetName);
        }

        public void EditPresetHeight(int presetId, int presetHeight)
        {
            presetService.UpdatePresetHeight(presetId, presetHeight);
        }
        public void EditPresetIcon(int presetId, string presetIcon)
        {
            presetService.UpdatePresetIcon(presetId, presetIcon);
        }

        public void RemovePreset(int id)
        {
            presetService.RemovePreset(id);
        }
    }
}