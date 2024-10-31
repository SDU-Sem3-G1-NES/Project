using SharedModels;

namespace Famicom.Models;

public class TableModel {
    private ITable? table;

    public TableModel() {

    }

    public ITable GetTable() {
        // Logic to get shit from the backend WOULD BE here, for now, you just mock. -N
        var linakTable = new LinakTable(
            "cd:fb:1a:53:fb:e6",
            "DESK 4486"
        );
        this.table = linakTable;
        return this.table;
    }
}