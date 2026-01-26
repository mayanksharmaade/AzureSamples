using Newtonsoft.Json;
using SalesDataDemo.Models;
using System.Text;
using System.Text.Json.Serialization;

namespace SalesDataDemo.Controllers
{
    
    [ApiController]
    public class SalesDataController : ControllerBase
    {
        [HttpPost]
        [Route("Sales/AddSalesRecord")]
        public async Task<SalesRecord> AddSalesRecord([FromBody]SalesRecord record)
        {
            var SalesRecordSerialized = JsonConvert.SerializeObject(record);
            var SalesrecordContent = new StringContent(SalesRecordSerialized, Encoding.UTF8, "application/json");
            var httpclient = new HttpClient();
            await httpclient.PostAsync("https://prod-**.northcentralus.logic.azure.com:***/workflows/f4774f54536a4d419727dadfb427451d/triggers/When_an_HTTP_request_is_received/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2FWhen_an_HTTP_request_is_received%2Frun&sv=1.0&sig=3LnJbPQfO7NJRDhpWUP6CVicebQ-iLgdoYM9VNZRzr4",
                SalesrecordContent);
            return record;
        }
    }
}
