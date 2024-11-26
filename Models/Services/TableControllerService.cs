using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using SharedModels;
using TableController;


namespace Models.Services
{
    public interface ITableControllerService
    {
        Task<ITableController> GetTableController(string guid);
    }
    public class TableControllerService : ITableControllerService
    {
        private LinakTableController linakTableController;
        private LinakSimulatorController linakSimulatorController;
        private MockTableController mockTableController;

        private readonly TableRepository _tableRepository;

        public TableControllerService()
        {
            linakTableController = new LinakTableController();
            linakSimulatorController = new LinakSimulatorController();
            mockTableController = new MockTableController();
            _tableRepository = new TableRepository();
        }

        public Task<ITableController> GetTableController(string guid)
        {
            // var api= _tableRepository.GetTableAPI(guid);
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
    }
}