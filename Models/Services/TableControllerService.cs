using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using SharedModels;
using TableController;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;



namespace Models.Services
{
    public interface ITableControllerService
    {
        Task<ITableController> GetTableController(string guid);
        Task<ITableController> GetTableController(string guid, HttpClient client);
        Task<ITableController> GetTableControllerByApiName(string api);
        Task<ITableController> GetTableControllerByApiName(string api, HttpClient client);
    }
    public class TableControllerService : ITableControllerService
    {
        private readonly TableRepository _tableRepository;

        public TableControllerService()
        {
            _tableRepository = new TableRepository();
        }
        private LinakSimulatorController linakSimulatorController = new LinakSimulatorController();
        private LinakTableController linakTableController = new LinakTableController();
        private MockTableController mockTableController =new MockTableController();
        public Task<ITableController> GetTableController(string guid) => GetTableController(guid, null!);
        public Task<ITableController> GetTableController(string guid, HttpClient client)
        {
            var api= _tableRepository.GetTableAPI(guid);
            if (api== null) return Task.FromException<ITableController>(new Exception("Table not found."));

            switch (api)
            {
                case "LinakSimulatorController":
                    return Task.FromResult<ITableController>(linakSimulatorController);
                case "LinakTableController":
                    return Task.FromResult<ITableController>(linakTableController);
                case "MockTableController":
                    return Task.FromResult<ITableController>(mockTableController);
                default:
                    return Task.FromException<ITableController>(new Exception("Invalid API."));
            }

        }
        public Task<ITableController> GetTableControllerByApiName(string api) => GetTableControllerByApiName(api, null!);
        public Task<ITableController> GetTableControllerByApiName(string api, HttpClient client)
        {
            switch (api)
            {
                case "Linak Simulator API V2":
                    return Task.FromResult<ITableController>(linakSimulatorController);
                case "Linak API":
                    return Task.FromResult<ITableController>(linakTableController);
                case "Mock API":
                    return Task.FromResult<ITableController>(mockTableController);
                default:
                    return Task.FromException<ITableController>(new Exception("Invalid API."));
            }
        }
    }
}