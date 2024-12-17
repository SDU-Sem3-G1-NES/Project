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
        Task<ITableController> GetTableController(string guid, HttpClient client);
        Task<ITableController> GetTableControllerByApiName(string api, HttpClient client);
    }
    public class TableControllerService : ITableControllerService
    {
        private readonly TableRepository _tableRepository;

        public TableControllerService()
        {
            _tableRepository = new TableRepository();
            linakSimulatorController = new LinakSimulatorController();
            linakTableController = new LinakTableController();
            mockTableController = new MockTableController();
        }
        private LinakSimulatorController linakSimulatorController;
        private LinakTableController linakTableController; 
        private MockTableController mockTableController;
        public Task<ITableController> GetTableController(string guid, HttpClient client)
        {
            var api= _tableRepository.GetTableAPI(guid);
            if (api== null) return Task.FromException<ITableController>(new Exception("Table not found."));

            switch (api)
            {
                case "LinakSimulatorController":
                    linakSimulatorController.HttpClient = client;
                    return Task.FromResult<ITableController>(linakSimulatorController);
                case "LinakTableController":
                    return Task.FromResult<ITableController>(linakTableController);
                case "MockTableController":
                    return Task.FromResult<ITableController>(mockTableController);
                default:
                    return Task.FromException<ITableController>(new Exception("Invalid API."));
            }

        }
        public Task<ITableController> GetTableControllerByApiName(string api, HttpClient client)
        {
            switch (api)
            {
                case "Linak Simulator API V2":
                    linakSimulatorController.HttpClient = client;
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