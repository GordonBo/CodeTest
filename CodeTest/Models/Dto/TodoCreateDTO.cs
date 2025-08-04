using System.ComponentModel.DataAnnotations;

namespace CodeTest.Models.Dto
{
    public class TodoCreateDTO
    {

        [Required]
        [MinLength(1, ErrorMessage = "Title cannot be empty.")]
        [MaxLength(500, ErrorMessage = "Title cannot exceed 500 characters.")]
        public string Title { get; set; } = string.Empty;


        public DateOnly DueDate { get; set; }
    }
}
