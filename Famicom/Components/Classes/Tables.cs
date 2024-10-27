namespace Famicom.Components.Classes
{
    public class Tables
    {
        public int TableId { get; set; }
        public string TableName { get; set; }
        public string TableManufacturer { get; set; }
        public int TableApi { get; set; }

        public Tables(int tableId, string tableName, string tableManufacturer, int tableApi)
        {
            TableId = tableId;
            TableName = tableName;
            TableManufacturer = tableManufacturer;
            TableApi = tableApi;
        }

        public Tables()
        {
        }
    }

}
