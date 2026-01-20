using Azure;
using Azure.Data.Tables;

namespace AzureBLOBsample.Data
{
    public class AttendeeEntity : ITableEntity
    {
         public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email {get;set;}

        public string Profession { get; set; }
        public string PartitionKey { get ; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
