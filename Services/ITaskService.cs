using TodoApp.Dtos;
using TodoApp.Entities;

namespace TodoApp.Services
{
    public interface ITaskService
    {
        List<TaskItem> GetTasks();
        TaskItem GetTaskById(int id);
        string CreateTask(TaskDto taskDto);
        string EditTask(int id, TaskDto taskDto);
        string DeleteTask(int id);
    }
}
