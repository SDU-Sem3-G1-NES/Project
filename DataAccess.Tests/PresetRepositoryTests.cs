using Xunit;
using Moq;
using DataAccess;

namespace DataAccess.Tests
{
    public class PresetRepositoryTests
    {
        private readonly Mock<DbAccess> _dbAccessMock;
        private readonly PresetRepository _presetRepository;

        public PresetRepositoryTests()
        {
            _dbAccessMock = new Mock<DbAccess>();
            _presetRepository = new PresetRepository(_dbAccessMock.Object);
        }

        [Fact]
        public void InsertPreset_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            string name = "Preset1";
            int user = 1;
            int height = 100;
            string options = "Option1";
            string icon = "Icon1";

            // Act
            _presetRepository.InsertPreset(name, user, height, options, icon);

            // Assert
            var param1 = ("@name", (object)name);
            var param2 = ("@user", (object)user);
            var param3 = ("@height", (object)height);
            var param4 = ("@options", (object)options);
            var param5 = ("@icon", (object)icon);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "INSERT INTO presets (p_name, p_user, p_height, p_options, p_icon) VALUES (@name, @user, @height, @options::jsonb, @icon)",
                param1, param2, param3, param4, param5
            ), Times.Once);
        }

        [Fact]
        public void EditPreset_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            int presetId = 1;
            string presetName = "Preset1";
            int presetUser = 1;
            int presetHeight = 100;
            string presetOptions = "Option1";
            string presetIcon = "Icon1";

            // Act
            _presetRepository.EditPreset(presetId, presetName, presetUser, presetHeight, presetOptions, presetIcon);

            // Assert
            var param1 = ("@presetName", (object)presetName);
            var param2 = ("@presetUser", (object)presetUser);
            var param3 = ("@presetHeight", (object)presetHeight);
            var param4 = ("@presetOptions", (object)presetOptions);
            var param5 = ("@presetIcon", (object)presetIcon);
            var param6 = ("@presetId", (object)presetId);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "UPDATE presets SET p_name = @presetName, p_user = @presetUser, p_height = @presetHeight, p_options = @presetOptions::jsonb, p_icon = @presetIcon WHERE p_id = @presetId",
                param1, param2, param3, param4, param5, param6
            ), Times.Once);
        }

        [Fact]
        public void DeletePreset_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            int presetId = 1;

            // Act
            _presetRepository.DeletePreset(presetId);

            // Assert
            var param1 = ("@id", (object)presetId);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "DELETE FROM presets WHERE p_id = @id",
                param1
            ), Times.Once);
        }
    }
}
