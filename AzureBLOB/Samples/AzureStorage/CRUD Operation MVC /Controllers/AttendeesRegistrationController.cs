using AzureBLOBsample.Data;
using AzureBLOBsample.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AzureBLOBsample.Controllers
{
    public class AttendeesRegistrationController : Controller
    {
        // GET: AttendeesRegistrationController
        private readonly ITablestorageService _tablestorage;
        private readonly IBlobStorageService _blobstorage;
        public AttendeesRegistrationController(ITablestorageService service,IBlobStorageService blobstorage)
        {
           this. _tablestorage = service;
            this._blobstorage = blobstorage;
        }
        public  async Task<ActionResult> Index()
        {
            var data = await _tablestorage.GetAttendees();
             foreach ( var item in data)
            {

                if (!string.IsNullOrEmpty(item.ImageName))
                { item.ImageName = await _blobstorage.GetBlobUrl(item.ImageName); }
                 
            }
            return View(data);
        }

        // GET: AttendeesRegistrationController/Details/5
        public   async Task<ActionResult> DetailsAsync(string Profession, string id)
        {
            var data = await _tablestorage.GetAttendee(Profession, id);
            return View(data);
        }

        // GET: AttendeesRegistrationController/Create
        public async Task<ActionResult> Create()
        {
            
            return View();
        }

        // POST: AttendeesRegistrationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AttendeeEntity Attend, IFormFile file)
        {
            try
            {
                var id = Guid.NewGuid().ToString();
                Attend.PartitionKey = Attend.Profession;
                Attend.RowKey = id;
                if (file != null && file.Length > 0)
                {
                    Attend.ImageName = await _blobstorage.UploadBlob(file, id);
                }

                await _tablestorage.AddAttendee(Attend);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AttendeesRegistrationController/Edit/5
        public  async Task<ActionResult> Edit(string Profession,string id)
        {
            var data = await _tablestorage.GetAttendee(Profession, id);
            return View(data);
           
        }

        // POST: AttendeesRegistrationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, AttendeeEntity attendee)
        {
           
                try
                {
                    attendee.PartitionKey = attendee.Profession;
                    attendee.RowKey = Guid.NewGuid().ToString();

                    await _tablestorage.AddAttendee(attendee);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            
        

       

        // POST: AttendeesRegistrationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string Profession, string id)
        {
            try
            {
                var data = await _tablestorage.GetAttendee(Profession, id);
                await _tablestorage.DeleteAttendee(Profession, id);
                await _blobstorage.RemoveBlob(data.ImageName);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
