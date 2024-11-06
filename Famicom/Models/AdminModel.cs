using SharedModels;
using System.Collections.Generic;

namespace Famicom.Models
{
    public class AdminModel
    {
        public List<ITable> GetAllTables()
        {
            // Mock data for now, replace with backend data retrieval logic as needed
            return new List<ITable>
            {
                new LinakTable("01:23:45:67:89:AB", "Table 1"),
                new LinakTable("98:76:54:32:10:FE", "Table 2"),
                new LinakTable("AA:BB:CC:DD:EE:FF", "Table 3")
                // Add more mock tables here
            };
        }
    }
}
