using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModify
{
    public class PresetRepository
    {
        private readonly DbAccess dbAccess;

        public PresetRepository()
        {
            dbAccess = new DbAccess();
        }
        
        // Constructor for testing.
        public PresetRepository(DbAccess dbAccess)
        {
            this.dbAccess = dbAccess;
        }
        #region Insert Methods

        public void InsertPreset(string name, int user, int height, string options)
        {
            var sql = "INSERT INTO presets (p_name, p_user, p_height, p_options) VALUES (@name, @user, @height, @options)";
            dbAccess.ExecuteNonQuery(sql, ("@name", name), ("@user", user), ("@height", height), ("@options", options));
        }

        #endregion

        #region Edit Methods

        public void EditPresetName(int presetId, string presetName)
        {
            var sql = "UPDATE presets SET p_name = @presetName WHERE p_id = @presetId";
            dbAccess.ExecuteNonQuery(sql, ("@presetName", presetName), ("@presetId", presetId));
        }

        public void EditPresetUser(int presetId, int presetUser)
        {
            var sql = "UPDATE presets SET p_user = @presetUser WHERE p_id = @presetId";
            dbAccess.ExecuteNonQuery(sql, ("@presetUser", presetUser), ("@presetId", presetId));
        }

        public void EditPresetHeight(int presetId, int presetHeight)
        {
            var sql = "UPDATE presets SET p_height = @presetHeight WHERE p_id = @presetId";
            dbAccess.ExecuteNonQuery(sql, ("@presetHeight", presetHeight), ("@presetId", presetId));
        }

        public void EditPresetOptions(int presetId, string presetOptions)
        {
            var sql = "UPDATE presets SET p_options = @presetOptions WHERE p_id = @presetId";
            dbAccess.ExecuteNonQuery(sql, ("@presetOptions", presetOptions), ("@presetId", presetId));
        }

        #endregion

        #region Delete Methods

        public void DeletePreset(int id)
        {
            var sql = "DELETE FROM presets WHERE p_id = @id";
            dbAccess.ExecuteNonQuery(sql, ("@id", id));
        }

        #endregion
    }
}
