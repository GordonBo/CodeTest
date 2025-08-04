using System.ComponentModel.DataAnnotations;

namespace CodeTest.Models.Dto
{
    public class TodoUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = "Title cannot be empty.")]
        [MaxLength(500, ErrorMessage = "Title cannot exceed 500 characters.")]
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }

        public DateOnly DueDate { get; set; }
    }
}
