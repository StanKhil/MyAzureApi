using Azure;
using Azure.Data.Tables;

namespace MyAzureApi.Entities
{
    public class StudentEntity : ITableEntity
    {
        public string PartitionKey { get; set; } = "Group1";
        public string RowKey { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "";
        public string Grade { get; set; } = "";
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
