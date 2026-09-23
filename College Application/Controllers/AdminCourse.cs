using College_Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace College_Application.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminCourse : Controller
    {
        private readonly MyDbContext _db;
        private readonly IWebHostEnvironment _env;

        public AdminCourse(MyDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        // LIST ALL COURSES
        public IActionResult Index()
        {
            var courses = _db.Courses.ToList();
            return View(courses);
        }

        // CREATE COURSE - GET
        public IActionResult Create()
        {
            return View();
        }

        // CREATE COURSE - POST
        [HttpPost]
        public async Task<IActionResult> Create(Course course, IFormFile Image)
        {
            if (Image != null)
            {
                string folder = Path.Combine(_env.WebRootPath, "CourseImages");
                Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(Image.FileName);
                string filePath = Path.Combine(folder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await Image.CopyToAsync(stream);

                course.ImagePath = "/CourseImages/" + fileName;
            }

            _db.Courses.Add(course);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // EDIT COURSE - GET
        public IActionResult Edit(int id)
        {
            var course = _db.Courses.Find(id);
            if (course == null) return NotFound();
            return View(course);
        }

        // EDIT COURSE - POST
        [HttpPost]
        public async Task<IActionResult> Edit(Course course, IFormFile Image)
        {
            var dbCourse = _db.Courses.Find(course.Id);
            if (dbCourse == null) return NotFound();

            dbCourse.Title = course.Title;
            dbCourse.Description = course.Description;

            if (Image != null)
            {
                // DELETE OLD IMAGE
                if (!string.IsNullOrEmpty(dbCourse.ImagePath))
                {
                    var oldPath = Path.Combine(_env.WebRootPath, dbCourse.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                // UPLOAD NEW IMAGE
                string folder = Path.Combine(_env.WebRootPath, "CourseImages");
                string fileName = Guid.NewGuid() + Path.GetExtension(Image.FileName);
                string newPath = Path.Combine(folder, fileName);

                using var stream = new FileStream(newPath, FileMode.Create);
                await Image.CopyToAsync(stream);

                dbCourse.ImagePath = "/CourseImages/" + fileName;
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // DELETE COURSE
        public IActionResult Delete(int id)
        {
            var course = _db.Courses.Find(id);
            if (course == null) return NotFound();

            // DELETE IMAGE
            if (!string.IsNullOrEmpty(course.ImagePath))
            {
                var path = Path.Combine(_env.WebRootPath, course.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
            }

            _db.Courses.Remove(course);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}

