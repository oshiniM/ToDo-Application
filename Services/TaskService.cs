using TodoApp.Data;
using TodoApp.Dtos;
using TodoApp.Entities;

namespace TodoApp.Services
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;

        public TaskService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<TaskItem> GetTasks()
        {
            return _context.Tasks.OrderByDescending(t => t.Id).ToList();
        }

        public TaskItem GetTaskById(int id)
        {
            return _context.Tasks.Find(id);
        }

        public string CreateTask(TaskDto taskDto)
        {
            var task = new TaskItem
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                IsCompleted = taskDto.IsCompleted,
                CreatedAt = DateTime.Now
            };

            _context.Tasks.Add(task);
            _context.SaveChanges();

            return "Task Created Successfully";
        }

        public string EditTask(int id, TaskDto taskDto)
        {
            var task = _context.Tasks.Find(id);
            if (task == null)
            {
                return "Task not found";
            }

            task.Title = taskDto.Title;
            task.Description = taskDto.Description;
            task.IsCompleted = taskDto.IsCompleted;
            task.CreatedAt = DateTime.Now;

            _context.Tasks.Update(task);
            _context.SaveChanges();

            return "Task Updated Successfully";
        }

        public string DeleteTask(int id)
        {
            var task = _context.Tasks.Find(id);
            if (task == null)
            {
                return "Task not found";
            }

            _context.Tasks.Remove(task);
            _context.SaveChanges();

            return "Task Deleted Successfully";
        }
    }
}
