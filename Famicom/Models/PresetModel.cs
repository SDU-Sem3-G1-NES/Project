using SharedModels;
using Models.Services;
using TableController;
using System.Diagnostics;

namespace Famicom.Models
{
    public class PresetsModel
    {
        private readonly PresetService presetService;
        private readonly TableControllerService TableControllerService;
        private readonly HttpClient client;
        private readonly Progress<ITableStatusReport> progress;
        
        public PresetsModel(HttpClient client, TableControllerService tableControllerService, PresetService presetService)
        {
            this.presetService = presetService;
            this.TableControllerService = tableControllerService;
            this.client = client;
            this.progress = new Progress<ITableStatusReport>(message =>
            {
                Debug.WriteLine(message);
            });
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
        public async Task<ITableStatusReport> SetPresetHeight(int presetHeight, string tableGUID)
        {
            var tableController = await TableControllerService.GetTableController(tableGUID, client);

            var tcs = new TaskCompletionSource<ITableStatusReport>();

            var progress = new Progress<ITableStatusReport>(message =>
            {
                Debug.WriteLine(message);
                tcs.TrySetResult(message);
            });

            await tableController.SetTableHeight(presetHeight, tableGUID, progress);
            
            return await tcs.Task;
        }
    }
}