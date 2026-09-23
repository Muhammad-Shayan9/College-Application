using System.ComponentModel.DataAnnotations;

namespace College_Application.Models
{
    public class Faculty
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Image path is required")]
        [StringLength(200, ErrorMessage = "Image path cannot be longer than 200 characters")]
        public string ImagePath { get; set; }

    }
}
