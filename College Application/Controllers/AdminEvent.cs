using College_Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace College_Application.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminEvent : Controller
    {
        private readonly MyDbContext _db;
        private readonly IWebHostEnvironment _env;

        public AdminEvent(MyDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        // LIST ALL EVENTS
        public IActionResult Index()
        {
            var eventList = _db.Events.ToList();
            return View(eventList);
        }

        // CREATE EVENT - GET
        public IActionResult Create()
        {
            return View();
        }

        // CREATE EVENT - POST
        [HttpPost]
        public async Task<IActionResult> Create(Event eventModel, IFormFile Image)
        {
            if (Image != null)
            {
                string folder = Path.Combine(_env.WebRootPath, "EventImages");
                Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(Image.FileName);
                string filePath = Path.Combine(folder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await Image.CopyToAsync(stream);

                eventModel.ImagePath = "/EventImages/" + fileName;
            }

            _db.Events.Add(eventModel);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // EDIT EVENT - GET
        public IActionResult Edit(int id)
        {
            var eventModel = _db.Events.Find(id);
            if (eventModel == null) return NotFound();
            return View(eventModel);
        }

        // EDIT EVENT - POST
        [HttpPost]
        public async Task<IActionResult> Edit(Event eventModel, IFormFile Image)
        {
            var dbEvent = _db.Events.Find(eventModel.Id);
            if (dbEvent == null) return NotFound();

            dbEvent.Title = eventModel.Title;
            dbEvent.Description = eventModel.Description;

            if (Image != null)
            {
                // DELETE OLD IMAGE
                if (!string.IsNullOrEmpty(dbEvent.ImagePath))
                {
                    var oldPath = Path.Combine(_env.WebRootPath, dbEvent.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                // UPLOAD NEW IMAGE
                string folder = Path.Combine(_env.WebRootPath, "EventImages");
                Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(Image.FileName);
                string newPath = Path.Combine(folder, fileName);

                using var stream = new FileStream(newPath, FileMode.Create);
                await Image.CopyToAsync(stream);

                dbEvent.ImagePath = "/EventImages/" + fileName;
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // DELETE EVENT
        public IActionResult Delete(int id)
        {
            var eventModel = _db.Events.Find(id);
            if (eventModel == null) return NotFound();

            if (!string.IsNullOrEmpty(eventModel.ImagePath))
            {
                var path = Path.Combine(_env.WebRootPath, eventModel.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
            }

            _db.Events.Remove(eventModel);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
