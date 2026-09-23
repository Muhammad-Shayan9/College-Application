using College_Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace College_Application.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminFaculty : Controller
    {
        private readonly MyDbContext _db;
        private readonly IWebHostEnvironment _env;

        public AdminFaculty(MyDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        // LIST ALL FACULTY
        public IActionResult Index()
        {
            var facultyList = _db.Faculty.ToList();
            return View(facultyList);
        }

        // CREATE FACULTY - GET
        public IActionResult Create()
        {
            return View();
        }

        // CREATE FACULTY - POST
        [HttpPost]
        public async Task<IActionResult> Create(Faculty faculty, IFormFile Image)
        {
            if (Image != null)
            {
                string folder = Path.Combine(_env.WebRootPath, "FacultyImages");
                Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(Image.FileName);
                string filePath = Path.Combine(folder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await Image.CopyToAsync(stream);

                faculty.ImagePath = "/FacultyImages/" + fileName;
            }

            _db.Faculty.Add(faculty);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // EDIT FACULTY - GET
        public IActionResult Edit(int id)
        {
            var faculty = _db.Faculty.Find(id);
            if (faculty == null) return NotFound();
            return View(faculty);
        }

        // EDIT FACULTY - POST
        [HttpPost]
        public async Task<IActionResult> Edit(Faculty faculty, IFormFile Image)
        {
            var dbFaculty = _db.Faculty.Find(faculty.Id);
            if (dbFaculty == null) return NotFound();

            dbFaculty.Title = faculty.Title;
            dbFaculty.Description = faculty.Description;

            if (Image != null)
            {
                // DELETE OLD IMAGE
                if (!string.IsNullOrEmpty(dbFaculty.ImagePath))
                {
                    var oldPath = Path.Combine(_env.WebRootPath, dbFaculty.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                // UPLOAD NEW IMAGE
                string folder = Path.Combine(_env.WebRootPath, "FacultyImages");
                Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(Image.FileName);
                string newPath = Path.Combine(folder, fileName);

                using var stream = new FileStream(newPath, FileMode.Create);
                await Image.CopyToAsync(stream);

                dbFaculty.ImagePath = "/FacultyImages/" + fileName;
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // DELETE FACULTY
        public IActionResult Delete(int id)
        {
            var faculty = _db.Faculty.Find(id);
            if (faculty == null) return NotFound();

            if (!string.IsNullOrEmpty(faculty.ImagePath))
            {
                var path = Path.Combine(_env.WebRootPath, faculty.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
            }

            _db.Faculty.Remove(faculty);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
