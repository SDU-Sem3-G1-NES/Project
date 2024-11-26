using DataAccess;
using SharedModels;

namespace Models.Services
{
    public class ApiService
    {
        private readonly ApiRepository apiRepository;

        public ApiService()
        {
            this.apiRepository = new ApiRepository();
        }

        public void AddApi(string name, string config)
        {
            apiRepository.InsertApi(name, config);
        }

        public void UpdateApiName(int apiId, string apiName)
        {
            apiRepository.EditApiName(apiId, apiName);
        }

        public void UpdateApiConfig(int apiId, string apiConfig)
        {
            apiRepository.EditApiConfig(apiId, apiConfig);
        }

        public void RemoveApi(int id)
        {
            apiRepository.DeleteApi(id);
        }

        public List<Apis> GetAllApis()
        {
            return apiRepository.GetAllApis();
        }
    }
}