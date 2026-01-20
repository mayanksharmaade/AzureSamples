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
        public AttendeesRegistrationController(ITablestorageService service)
        {
            _tablestorage = service;
        }
        public  async Task<ActionResult> Index()
        {
            var data = await _tablestorage.GetAttendees();
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
        public async Task<ActionResult> Create(AttendeeEntity Attend)
        {
            try
            {
                Attend.PartitionKey = Attend.Profession;
                Attend.RowKey = Guid.NewGuid().ToString();

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
                await _tablestorage.DeleteAttendee(Profession, id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
