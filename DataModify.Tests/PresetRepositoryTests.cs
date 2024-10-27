using Moq;
using Xunit;

namespace DataModify.Tests
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
        public void InsertPreset_ShouldExecuteNonQueryWithCorrectParameters()
        {
            // Arrange
            var name = "Test Preset";
            var user = 1;
            var height = 120;
            var options = "Test Options";

            // Act
            _presetRepository.InsertPreset(name, user, height, options);

            // Assert
            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                It.Is<string>(s => s.Contains("INSERT INTO presets")),
                It.Is<(string, object)>(p => p.Item1 == "@name" && (string)p.Item2 == name),
                It.Is<(string, object)>(p => p.Item1 == "@user" && (int)p.Item2 == user),
                It.Is<(string, object)>(p => p.Item1 == "@height" && (int)p.Item2 == height),
                It.Is<(string, object)>(p => p.Item1 == "@options" && (string)p.Item2 == options)
            ), Times.Once);
        }

        [Fact]
        public void EditPresetName_ShouldExecuteNonQueryWithCorrectParameters()
        {
            // Arrange
            var presetId = 2;
            var presetName = "Updated Preset Name";

            // Act
            _presetRepository.EditPresetName(presetId, presetName);

            // Assert
            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                It.Is<string>(s => s.Contains("UPDATE presets SET p_name")),
                It.Is<(string, object)>(p => p.Item1 == "@presetName" && (string)p.Item2 == presetName),
                It.Is<(string, object)>(p => p.Item1 == "@presetId" && (int)p.Item2 == presetId)
            ), Times.Once);
        }

        [Fact]
        public void EditPresetUser_ShouldExecuteNonQueryWithCorrectParameters()
        {
            // Arrange
            var presetId = 3;
            var presetUser = 4;

            // Act
            _presetRepository.EditPresetUser(presetId, presetUser);

            // Assert
            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                It.Is<string>(s => s.Contains("UPDATE presets SET p_user")),
                It.Is<(string, object)>(p => p.Item1 == "@presetUser" && (int)p.Item2 == presetUser),
                It.Is<(string, object)>(p => p.Item1 == "@presetId" && (int)p.Item2 == presetId)
            ), Times.Once);
        }

        [Fact]
        public void EditPresetHeight_ShouldExecuteNonQueryWithCorrectParameters()
        {
            // Arrange
            var presetId = 5;
            var presetHeight = 180;

            // Act
            _presetRepository.EditPresetHeight(presetId, presetHeight);

            // Assert
            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                It.Is<string>(s => s.Contains("UPDATE presets SET p_height")),
                It.Is<(string, object)>(p => p.Item1 == "@presetHeight" && (int)p.Item2 == presetHeight),
                It.Is<(string, object)>(p => p.Item1 == "@presetId" && (int)p.Item2 == presetId)
            ), Times.Once);
        }

        [Fact]
        public void EditPresetOptions_ShouldExecuteNonQueryWithCorrectParameters()
        {
            // Arrange
            var presetId = 6;
            var presetOptions = "Updated Options";

            // Act
            _presetRepository.EditPresetOptions(presetId, presetOptions);

            // Assert
            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                It.Is<string>(s => s.Contains("UPDATE presets SET p_options")),
                It.Is<(string, object)>(p => p.Item1 == "@presetOptions" && (string)p.Item2 == presetOptions),
                It.Is<(string, object)>(p => p.Item1 == "@presetId" && (int)p.Item2 == presetId)
            ), Times.Once);
        }

        [Fact]
        public void DeletePreset_ShouldExecuteNonQueryWithCorrectParameters()
        {
            // Arrange
            var id = 7;

            // Act
            _presetRepository.DeletePreset(id);

            // Assert
            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
                It.Is<string>(s => s.Contains("DELETE FROM presets WHERE p_id")),
                It.Is<(string, object)>(p => p.Item1 == "@id" && (int)p.Item2 == id)
            ), Times.Once);
        }
    }
}
