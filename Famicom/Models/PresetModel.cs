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
        public void EditPreset(int presetId, string presetName, int presetUser, int presetHeight, string presetOptions, string presetIcon)
        {
            presetService.UpdatePreset(presetId, presetName, presetUser, presetHeight, presetOptions, presetIcon);
        }
        public void RemovePreset(int id)
        {
            presetService.RemovePreset(id);
        }
    }
}