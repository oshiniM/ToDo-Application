using System.ComponentModel.DataAnnotations;

namespace TodoApp.Dtos
{
    public class TaskDto
    {
        
        public required string Title { get; set; }

        [MaxLength(500)]
        public required string Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}
