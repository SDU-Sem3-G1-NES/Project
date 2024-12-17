using DataAccess;
using SharedModels;

namespace Models.Services
{
    public class PresetService
    {
        private readonly PresetRepository presetRepository;

        public PresetService(PresetRepository presetRepository)
        {
            this.presetRepository = presetRepository;
        }

        public void AddPreset(string name, int user, int height, string options, string icon)
        {
            presetRepository.InsertPreset(name, user, height, options, icon);
        }

        public void UpdatePreset(int presetId, string presetName, int presetUser, int presetHeight, string presetOptions, string presetIcon)
        {
            presetRepository.EditPreset(presetId, presetName, presetUser, presetHeight, presetOptions, presetIcon);
        }

        public void RemovePreset(int id)
        {
            presetRepository.DeletePreset(id);
        }

        public List<Presets> GetPresetsUser(int userId)
        {
            return presetRepository.GetPresetsUser(userId);
        }
    }
}