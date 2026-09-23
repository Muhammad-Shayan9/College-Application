using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.VisualBasic.FileIO;
using System.ComponentModel.DataAnnotations;

namespace College_Application.Models
{
    public class Admission
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Key]
        public int Id { get; set; }

        [Required]
        public Guid UniqueId { get; set; }   // Tracking ID

        // Personal Details
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Father Name is required")]
        [StringLength(100)]
        public string FatherName { get; set; }

        [Required(ErrorMessage = "Mother Name is required")]
        [StringLength(100)]
        public string MotherName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Residential Address is required")]
        [StringLength(250)]
        public string ResidentialAddress { get; set; }

        [Required(ErrorMessage = "Permanent Address is required")]
        [StringLength(250)]
        public string PermanentAddress { get; set; }

        // Admission For
        [Required(ErrorMessage = "Stream is required")]
        public string Stream { get; set; }


        public string? Field { get; set; }

        [BindNever]
        public string? UserId { get; set; }

        // Previous Education
        [Required(ErrorMessage = "School name is required")]
        [StringLength(150)]
        public string School { get; set; }

        [Required(ErrorMessage = "Enrollment number is required")]
        [StringLength(50)]
        public string EnrollmentNumber { get; set; }

        [Required(ErrorMessage = "Previous Stream is required")]
        public string PreviousStream { get; set; }

        [Required(ErrorMessage = "Marks secured is required")]
        [Range(0, 1100, ErrorMessage = "Invalid marks")]
        public int MarksSecured { get; set; }

        [Required(ErrorMessage = "Out Of is required")]
        [Range(1, 1100, ErrorMessage = "Invalid total marks")]
        public int OutOf { get; set; }

        [Required(ErrorMessage = "Class Obtained is required")]
        public string ClassObtained { get; set; }

        // Optional
        public string? SportsDetails { get; set; } 

        // Admission Status
        [Required]
        public string Status { get; set; } = "Waiting";

        // After Acceptance
        public string? SpecializedSubject { get; set; }
        public string? OptionalSubject { get; set; }

        public string? UploadedPdfPath { get; set; }   // offline filled form
        public bool IsOfflineSubmission { get; set; }  // true = PDF upload

    }

}
