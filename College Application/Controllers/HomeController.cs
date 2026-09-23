using College_Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace College_Application.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyDbContext _db; 

        public HomeController(MyDbContext db) 
        {
            _db = db;
        }
        [AllowAnonymous]
        [Authorize(Roles = "User")]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Course()
        {
            var courses = _db.Courses.ToList();  
            return View(courses);                
        }
        
        public IActionResult Department()
        {
            return View();
        }

        public IActionResult Faculty()
        {
            var facultyList = _db.Faculty.ToList();
            return View(facultyList);
        }
        public IActionResult Facilities()
        {
            return View();
        }
        public IActionResult Event()
        {
            var EventList = _db.Events.ToList();
            return View(EventList);
        }
       

        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(Contact data)
        {
            if (ModelState.IsValid)
            {
                _db.Contacts.Add(data); 
                _db.SaveChanges();
   
                return RedirectToAction("Success");
            }

            return View(data);
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
