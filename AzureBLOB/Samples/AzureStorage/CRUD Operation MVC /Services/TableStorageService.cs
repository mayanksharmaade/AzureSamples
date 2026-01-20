using Azure;
using Azure.Data.Tables;
using AzureBLOBsample.Data;
using System.Reflection.Metadata;

namespace AzureBLOBsample.Services
{
    public class TableStorageService:ITablestorageService
    {
        private readonly IConfiguration config;
        private const string Tablename = "Attendees";
        public TableStorageService( IConfiguration _config)
        {
            config = _config;
        }
        public  async Task<AttendeeEntity> GetAttendee( string Profession,string id)
        {
            var tableclient = await GetTableClient();
            return await tableclient.GetEntityAsync<AttendeeEntity>(Profession, id);
        }

        public  async Task<List<AttendeeEntity>> GetAttendees()
        {
            var tableclient = await GetTableClient();
            Pageable<AttendeeEntity> Attendees = tableclient.Query<AttendeeEntity>();
            return Attendees.ToList();
        }

        public async  Task AddAttendee(AttendeeEntity attend)
        {
            var tableclient = await GetTableClient();
            await tableclient.UpsertEntityAsync(attend);
        }
        public async Task DeleteAttendee(string Profession, string id)
        {

            var tableclient = await GetTableClient();
            await tableclient.DeleteEntityAsync(Profession, id);

        }

        private async Task <TableClient> GetTableClient()
        {
            var serviceclient = new TableServiceClient(config["StorageConnectionStrings"]);
            var tableclient = serviceclient.GetTableClient(Tablename);
            await tableclient.CreateIfNotExistsAsync();

            return tableclient;
        }
    }
}
