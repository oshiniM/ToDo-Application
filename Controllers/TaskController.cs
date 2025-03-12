using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public TaskController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult GetTask()
        {
            var tasks = context.Tasks.OrderByDescending(t => t.Id).ToList();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public IActionResult GetTask(int id)
        {
            var task = context.Tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        [HttpPost]
        public IActionResult CreateTask(TaskDto taskDto)
        {
            var task = new TaskItem
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                IsCompleted = taskDto.IsCompleted,
                CreatedAt = DateTime.Now
            };

            context.Tasks.Add(task);
            context.SaveChanges();

            return Ok("Task Created Successfully");
        }

        [HttpPut("{id}")]
        public IActionResult EditTask(int id, TaskDto taskDto)
        {
            var task = context.Tasks.Find(id);
            if (task == null)  
            {
                return NotFound();
            }

            task.Title = taskDto.Title;
            task.Description = taskDto.Description;
            task.IsCompleted = taskDto.IsCompleted;
            task.CreatedAt = DateTime.Now;

            context.Tasks.Update(task);
            context.SaveChanges();

            return Ok("Task Updated Successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            var task = context.Tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }

            context.Tasks.Remove(task);
            context.SaveChanges();

            return Ok("Task Deleted Successfully");
        }


    }
}
