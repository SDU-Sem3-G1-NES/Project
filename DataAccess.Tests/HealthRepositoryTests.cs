using Xunit;
using Moq;
using DataAccess;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataAccess.Tests
{
    public class HealthRepositoryTests
    {
        private readonly Mock<DbAccess> _dbAccessMock;
        private readonly HealthRepository _healthRepository;

        public HealthRepositoryTests()
        {
            _dbAccessMock = new Mock<DbAccess>();
            _healthRepository = new HealthRepository(_dbAccessMock.Object);
        }

        [Fact]
        public void InsertHealth_ShouldExecuteNonQuery_WithCorrectParameters_WhenPresetIdIsNotNull()
        {
            // Arrange
            int userId = 1;
            int? presetID = 1;
            int position = 1;

            // Act
            _healthRepository.InsertHealth(userId, presetID, position);

            // Assert
            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
            "INSERT INTO HEALTH (H_DATE, U_ID, P_ID, H_POSITION) VALUES (@H_DATE, @U_ID, @P_ID, @H_POSITION)",
        It.Is<(string, object)[]>(parameters =>
        parameters.Any(p => p.Item1 == "@H_DATE" && p.Item2 is DateTime) && // Match H_DATE is DateTime
        parameters.Any(p => p.Item1 == "@U_ID" && (int)p.Item2 == userId) && // Match U_ID
        parameters.Any(p => p.Item1 == "@P_ID" && (int)p.Item2 == presetID) && // Match P_ID
        parameters.Any(p => p.Item1 == "@H_POSITION" && (int)p.Item2 == position) // Match H_POSITION
    )
), Times.Once);
        }

        [Fact]
        public void InsertHealth_ShouldExecuteNonQuery_WithCorrectParameters_WhenPresetIdIsNull()
        {
            // Arrange
            int userId = 1;
            int? presetID = null;
            int position = 1;

            // Act
            _healthRepository.InsertHealth(userId, presetID, position);

            // Assert
            _dbAccessMock.Verify(db => db.ExecuteNonQuery(
            "INSERT INTO HEALTH (H_DATE, U_ID, H_POSITION) VALUES (@H_DATE, @U_ID, @H_POSITION)",
            It.Is<(string, object)[]>(parameters =>
            parameters.Any(p => p.Item1 == "@H_DATE" && p.Item2 is DateTime) && // Match H_DATE is DateTime
            parameters.Any(p => p.Item1 == "@U_ID" && (int)p.Item2 == userId) && // Match U_ID
            parameters.Any(p => p.Item1 == "@H_POSITION" && (int)p.Item2 == position) // Match H_POSITION
            )
        ), Times.Once);
        }

    }
}
