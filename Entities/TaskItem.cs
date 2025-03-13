using System.Diagnostics.Eventing.Reader;
using System.Reflection;

namespace TodoApp.Entities
{
    public class TaskItem
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; } 

    }
}
