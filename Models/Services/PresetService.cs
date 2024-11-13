using DataAccess;
using SharedModels;

namespace Models.Services
{
    public class PresetService
    {
        private readonly PresetRepository presetRepository;

        public PresetService()
        {
            presetRepository = new PresetRepository();
        }

        public void AddPreset(string name, int user, int height, string options)
        {
            presetRepository.InsertPreset(name, user, height, options);
        }

        public void UpdatePresetName(int presetId, string presetName)
        {
            presetRepository.EditPresetName(presetId, presetName);
        }

        public void UpdatePresetUser(int presetId, int presetUser)
        {
            presetRepository.EditPresetUser(presetId, presetUser);
        }

        public void UpdatePresetHeight(int presetId, int presetHeight)
        {
            presetRepository.EditPresetHeight(presetId, presetHeight);
        }

        public void UpdatePresetOptions(int presetId, string presetOptions)
        {
            presetRepository.EditPresetOptions(presetId, presetOptions);
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