using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Data;
using TodoApp.Dtos;
using TodoApp.Entities;
using TodoApp.Services;

namespace TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public IActionResult GetTasks()
        {
            var tasks = _taskService.GetTasks();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public IActionResult GetTask(int id)
        {
            var task = _taskService.GetTaskById(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        [HttpPost]
        public IActionResult CreateTask([FromBody] TaskDto taskDto)
        {
            var result = _taskService.CreateTask(taskDto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public IActionResult EditTask(int id, [FromBody] TaskDto taskDto)
        {
            var result = _taskService.EditTask(id, taskDto);
            if (result.Contains("not found"))
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            var result = _taskService.DeleteTask(id);
            if (result.Contains("not found"))
            {
                return NotFound(result);
            }
            return Ok(result);
        }
    }
}
