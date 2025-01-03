﻿using DataAccess;
using SharedModels;

namespace Models.Services
{
    public class HealthService
    {
        private readonly HealthRepository healthRepository;

        public HealthService()
        {
            healthRepository = new HealthRepository();
        }

        public void AddHealth(int userId, int? presetID, int position)
        {
            healthRepository.InsertHealth(userId, presetID, position);
        }

        public List<Health> GetHealth(int userID,DateTime? startTime = null ,DateTime? endtime = null)
        {
            return healthRepository.GetHealthByUser(userID, startTime ,endtime);
        }
    }
}
