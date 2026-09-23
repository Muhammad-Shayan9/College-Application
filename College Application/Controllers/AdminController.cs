using College_Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;

namespace College_Application.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private MyDbContext DatabaseConnection;
        public AdminController(MyDbContext MyDb)
        {
            DatabaseConnection = MyDb;
        }


        public IActionResult Dashboard()
        {
            ViewBag.TotalAdmissions = DatabaseConnection.AdmissonDetails.Count();
            ViewBag.TotalCourses = DatabaseConnection.Courses.Count();
            ViewBag.TotalFaculty = DatabaseConnection.Faculty.Count();
            ViewBag.PendingAdmissions = DatabaseConnection.AdmissonDetails.Count(a => a.Status == "Waiting");
            ViewBag.ApprovedAdmissions = DatabaseConnection.AdmissonDetails.Count(a => a.Status == "Approved");

            //For Chart 
            int total = DatabaseConnection.AdmissonDetails.Count();

            int scienceCount = DatabaseConnection.AdmissonDetails.Count(a => a.Stream == "Science");
            int commerceCount = DatabaseConnection.AdmissonDetails.Count(a => a.Stream == "Commerce");
            int artsCount = DatabaseConnection.AdmissonDetails.Count(a => a.Stream == "Arts");

            // Percentage
            ViewBag.StreamLabels = new[] { "Science", "Commerce", "Arts" };
            ViewBag.StreamPercent = new[] {
            total == 0 ? 0 : (scienceCount * 100) / total,
            total == 0 ? 0 : (commerceCount * 100) / total,
            total == 0 ? 0 : (artsCount * 100) / total };
            return View();
        }
        
        public IActionResult NewAddmission(string searchGuid)
        {
            var admissions = DatabaseConnection.AdmissonDetails.AsQueryable();

            if (!string.IsNullOrEmpty(searchGuid))
            {
                if (Guid.TryParse(searchGuid, out Guid guid))
                {
                    admissions = admissions.Where(a => a.UniqueId == guid);
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid GUID format!";
                }
            }

            return View(admissions.ToList());
        }

        public IActionResult ShowChart()
        {

            return View();
        }
        public IActionResult Feedback()
        {
            var feedback = DatabaseConnection.Contacts.ToList();

            return View(feedback);
        }

    }
}
