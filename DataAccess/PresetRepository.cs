using System.Text;
using SharedModels;

namespace DataAccess
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

        public void InsertPreset(string name, int user, int height, string options, string icon)
        {
            var sql = "INSERT INTO presets (p_name, p_user, p_height, p_options, p_icon) VALUES (@name, @user, @height, @options::jsonb, @icon)";
            dbAccess.ExecuteNonQuery(sql, ("@name", name), ("@user", user), ("@height", height), ("@options", options), ("@icon", icon));
        }
       
        #endregion

        #region Edit Methods

        public void EditPreset(int presetId, string presetName, int presetUser, int presetHeight, string presetOptions, string presetIcon)
        {
            var sql = "UPDATE presets SET p_name = @presetName, p_user = @presetUser, p_height = @presetHeight, p_options = @presetOptions::jsonb, p_icon = @presetIcon WHERE p_id = @presetId";
            dbAccess.ExecuteNonQuery(sql, ("@presetName", presetName), ("@presetUser", presetUser), ("@presetHeight", presetHeight), ("@presetOptions", presetOptions), ("@presetIcon", presetIcon), ("@presetId", presetId));
        }

        #endregion

        #region Delete Methods

        public void DeletePreset(int id)
        {
            var sql = "DELETE FROM presets WHERE p_id = @id";
            dbAccess.ExecuteNonQuery(sql, ("@id", id));
        }

        #endregion

        #region Get Methods

        public List<Presets> GetPresetsUser(int userId)
        {
            var sql = $"SELECT * FROM presets WHERE p_user = {userId}";

            List<Presets> presets = new List<Presets>();

            using (var cmd = dbAccess.dbDataSource.CreateCommand(sql))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Presets preset = new Presets()
                        {
                            PresetId = reader.GetInt32(0),
                            PresetName = reader.GetString(1),
                            UserId = reader.GetInt32(2),
                            Height = reader.GetInt32(3),
                            Options = reader.GetString(4),
                            Icon = reader.GetString(5)

                        };
                        presets.Add(preset);
                    }
                }
            }

            return presets;
        }

        #endregion
    }
}
