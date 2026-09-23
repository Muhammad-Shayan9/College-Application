using College_Application.Areas.Identity.Data;
using College_Application.Models;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace College_Application.Controllers
{
    public class AdmissionController : Controller
    {
        private readonly MyDbContext _db;
        private readonly IConverter _pdfConverter;

        public AdmissionController(MyDbContext db, IConverter pdfConverter)
        {
            _db = db;
            _pdfConverter = pdfConverter;
        }

        // GET: Admission Form
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult AdmissionForm()
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("/Identity/Account/Login");

            // Check if user already submitted
            var existingAdmission = _db.AdmissonDetails
                .FirstOrDefault(a => a.UserId == User.Identity.Name);

            if (existingAdmission != null)
                return RedirectToAction("CheckStatus");

            return View();
        }

        // POST: Admission Form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdmissionForm(Admission data)
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("/Identity/Account/Login");

            if (ModelState.IsValid)
            {
                // Prevent duplicate submission
                var existingAdmission = _db.AdmissonDetails
                    .FirstOrDefault(a => a.UserId == User.Identity.Name);

                if (existingAdmission != null)
                    return RedirectToAction("CheckStatus");

                // System fields
                data.UniqueId = Guid.NewGuid();
                data.Status = "Waiting";
                data.UserId = User.Identity.Name;

                _db.AdmissonDetails.Add(data);
                _db.SaveChanges();

                return RedirectToAction("Success", new { uid = data.UniqueId });
            }

            return View(data);
        }

        // GET: Success page
        public IActionResult Success(Guid uid)
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("/Identity/Account/Login");

            ViewBag.UniqueId = uid;
            return View();
        }

        // GET: Check Admission Status
        public IActionResult CheckStatus()
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("/Identity/Account/Login");

            var admission = _db.AdmissonDetails
                .FirstOrDefault(a => a.UserId == User.Identity.Name);

            if (admission == null)
                return RedirectToAction("AdmissionForm");

            return View(admission);
        }

        // ------------------- ADMIN -------------------
        [Authorize(Roles = "Admin")]
        public IActionResult ReviewAdmissions(string searchGuid)
        {
            var admissions = _db.AdmissonDetails.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchGuid))
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

            var result = admissions
                .OrderByDescending(a => a.Id)
                .ToList();

            return View(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int id, string status)
        {
            var admission = _db.AdmissonDetails.FirstOrDefault(a => a.Id == id);

            if (admission == null)
                return NotFound();

            if (status != "Approved" && status != "Rejected")
                return BadRequest();

            admission.Status = status;
            _db.SaveChanges();

            return RedirectToAction(nameof(ReviewAdmissions));
        }

        // ------------------- PDF Download -------------------
        [HttpPost]
        public IActionResult DownloadPdf(Admission data)
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("/Identity/Account/Login");

            string logoPath = Path.Combine(
        Directory.GetCurrentDirectory(),
        "wwwroot",
        "User",
        "img",
        "FavIcon Green.png"
            );

            // HTML template for PDF
            string html = $@"
                    <html>
                    <head>
                        <style>
                            body {{ font-family: Arial, sans-serif; margin: 20px; }}
                            .header {{ text-align: center; margin-bottom: 20px; }}
                            .header img {{ max-width: 120px; }}
                            .section {{ margin-bottom: 15px; }}
                            table {{ width: 100%; border-collapse: collapse; }}
                            table, th, td {{ border: 1px solid #333; padding: 6px; }}
                            th {{ background-color: #333; color: white; }}

                            .filled {{
                            display: inline-block;
                            min-width: 260px;
                            border-bottom: 1px solid #000;
                            padding-left: 5px;
                        }}
                        </style>
                    </head>
                    <body>

                        <div class='header'>
                            <img src='{logoPath}' />
                            <h2>College Admission Form</h2>
                        </div>

                        <div class='section'>
                            <h4>Personal Details</h4>
                            <p><strong>Name:</strong> <span class='filled'>{data.Name}</span></p>
                            <p><strong>Father Name:</strong> <span class='filled'>{data.FatherName}</span></p>
                            <p><strong>Mother Name:</strong> <span class='filled'>{data.MotherName}</span></p>
                            <p><strong>Date of Birth:</strong> <span class='filled'>{data.DateOfBirth:dd-MM-yyyy}</span></p>
                            <p><strong>Gender:</strong> <span class='filled'>{data.Gender}</span></p>

                        </div>

                       <div class='section'>
                            <h4>Address</h4>
                            <p><strong>Residential:</strong> <span class='filled'>{data.ResidentialAddress}</span></p>
                            <p><strong>Permanent:</strong> <span class='filled'>{data.PermanentAddress}</span></p>
                        </div>

                        <div class='section'>
                            <h4>Admission Details</h4>
                            <p><strong>Stream:</strong> <span class='filled'>{data.Stream}</span></p>
                        </div>


                        <div class='section'>
                            <h4>Previous Exam Details</h4>
                            <table>
                                <tr>
                                    <th>School</th>
                                    <th>Enrollment No</th>
                                    <th>Stream</th>
                                    <th>Marks</th>
                                    <th>Out Of</th>
                                    <th>Class</th>
                                </tr>
                                <tr>
                                    <td>{data.School}</td>
                                    <td>{data.EnrollmentNumber}</td>
                                    <td>{data.PreviousStream}</td>
                                    <td>{data.MarksSecured}</td>
                                    <td>{data.OutOf}</td>
                                    <td>{data.ClassObtained}</td>
                                </tr>
                            </table>
                        </div>

                        <div class='section'>
                            <h4>Sports Details</h4>
                            <p>{(string.IsNullOrEmpty(data.SportsDetails) ? "N/A" : data.SportsDetails)}</p>
                        </div>

                    </body>
                    </html>";


            var pdfDoc = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings
                {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait,
                    Margins = new MarginSettings
                    {
                        Top = 15,      // top margin in mm
                        Bottom = 15,   // bottom margin in mm
                        Left = 10,     // optional
                        Right = 10     // optional
                    }
                },
                Objects =
                            {
                                new ObjectSettings
                                {
                                    HtmlContent = html,
                                    WebSettings = new WebSettings
                                    {
                                        DefaultEncoding = "utf-8",
                                        LoadImages = true
                                    }
                                }
                            }

            };


            byte[] pdf = _pdfConverter.Convert(pdfDoc);
            return File(pdf, "application/pdf", "AdmissionForm.pdf");
        }
    }
}
