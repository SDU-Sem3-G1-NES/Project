using SharedModels;

namespace Famicom.Models;

public class TableModel {
    private List<ITable> table = new();
    

    public TableModel() {

    }

    public List<ITable> GetTable() {
        // Logic to get shit from the backend WOULD BE here, for now, you just mock. -N
        var linakTable = new LinakTable(
            "cd:fb:1a:53:fb:e6",
            "DESK 4486"
        );
        table.Add(linakTable);

        var linakTable2 = new LinakTable(
            "cd:ft:1r:23:fb:e6",
            "Ironman"
            );
        table.Add(linakTable2);

        var linakTable3 = new LinakTable(
            "ct:ht:1r:23:fb:t2",
            "Hulk"
            );
        table.Add(linakTable3);
        

        
        return this.table; 
    }
}