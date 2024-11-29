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
    }
    public class TableControllerService : ITableControllerService
    {
        private readonly TableRepository _tableRepository;

        public TableControllerService()
        {
            _tableRepository = new TableRepository();
        }

        public Task<ITableController> GetTableController(string guid, HttpClient client)
        {
            var api= _tableRepository.GetTableAPI(guid);
            if (api== null) return Task.FromException<ITableController>(new Exception("Table not found."));

            switch (api)
            {
                case "LinakSimulatorController":
                    return Task.FromResult<ITableController>(new LinakSimulatorController(client));
                case "LinakTableController":
                    return Task.FromResult<ITableController>(new LinakTableController());
                case "MockTableController":
                    return Task.FromResult<ITableController>(new MockTableController());
                default:
                    return Task.FromException<ITableController>(new Exception("Invalid API."));
            }

        }
        public Task<ITableController> GetTableControllerByApiName(string api, HttpClient client)
        {
            switch (api)
            {
                case "LinakSimulatorController":
                    return Task.FromResult<ITableController>(new LinakSimulatorController(client));
                case "LinakTableController":
                    return Task.FromResult<ITableController>(new LinakTableController());
                case "MockTableController":
                    return Task.FromResult<ITableController>(new MockTableController());
                default:
                    return Task.FromException<ITableController>(new Exception("Invalid API."));
            }
        }
    }
}