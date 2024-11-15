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
        public void EditPresetName_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            int presetId = 1;
            string presetName = "UpdatedPreset";

            // Act
            _presetRepository.EditPresetName(presetId, presetName);

            // Assert
            var param1 = ("@presetName", (object)presetName);
            var param2 = ("@presetId", (object)presetId);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "UPDATE presets SET p_name = @presetName WHERE p_id = @presetId",
                param1, param2
            ), Times.Once);
        }

        [Fact]
        public void EditPresetUser_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            int presetId = 1;
            int presetUser = 2;

            // Act
            _presetRepository.EditPresetUser(presetId, presetUser);

            // Assert
            var param1 = ("@presetUser", (object)presetUser);
            var param2 = ("@presetId", (object)presetId);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "UPDATE presets SET p_user = @presetUser WHERE p_id = @presetId",
                param1, param2
            ), Times.Once);
        }

        [Fact]
        public void EditPresetHeight_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            int presetId = 1;
            int presetHeight = 200;

            // Act
            _presetRepository.EditPresetHeight(presetId, presetHeight);

            // Assert
            var param1 = ("@presetHeight", (object)presetHeight);
            var param2 = ("@presetId", (object)presetId);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "UPDATE presets SET p_height = @presetHeight WHERE p_id = @presetId",
                param1, param2
            ), Times.Once);
        }

        [Fact]
        public void EditPresetOptions_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            int presetId = 1;
            string presetOptions = "NewOptions";

            // Act
            _presetRepository.EditPresetOptions(presetId, presetOptions);

            // Assert
            var param1 = ("@presetOptions", (object)presetOptions);
            var param2 = ("@presetId", (object)presetId);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "UPDATE presets SET p_options = @presetOptions WHERE p_id = @presetId",
                param1, param2
            ), Times.Once);
        }
        [Fact]
        public void EditPresetIcon_ShouldExecuteNonQuery_WithCorrectParameters()
        {
            // Arrange
            int presetId = 1;
            string presetIcon = "NewIcon";

            // Act
            _presetRepository.EditPresetIcon(presetId, presetIcon);

            // Assert
            var param1 = ("@presetIcon", (object)presetIcon);
            var param2 = ("@presetId", (object)presetId);

            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                "UPDATE presets SET p_icon = @presetIcon WHERE p_id = @presetId",
                param1, param2
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
