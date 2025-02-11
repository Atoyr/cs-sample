using Microsoft.EntityFrameworkCore;

using TodoList.Entities;

namespace TodoList.Repositories;

public interface ITaskRepository
{
    Task<IEnumerable<TaskEntity>> GetAllTasksAsync();
    Task<TaskEntity?> GetTaskAsync(string id);

    Task AddTaskAsync(TaskEntity task);
    Task UpdateTaskAsync(string id, TaskEntity task);

    Task DeleteTaskAsync(string id);

    Task SaveChangesAsync();
}

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TaskEntity>> GetAllTasksAsync()
    {
        return await _context.Tasks.ToListAsync();
    }

    public async Task<TaskEntity?> GetTaskAsync(string id)
    {
        return await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task AddTaskAsync(TaskEntity task)
    {
        await _context.Tasks.AddAsync(task);
    }

    public async Task UpdateTaskAsync(string id, TaskEntity task)
    {
        var beforeTask = await GetTaskAsync(id);
        if (beforeTask is null)
        {
            throw new InvalidOperationException($"Task Id {task.Id} が存在しません");
        }
        if (beforeTask.ModifiedAt != task.ModifiedAt)
        {
            throw new Exception($"Task はすでに更新済みです");
        }
        beforeTask.Title = task.Title;
        beforeTask.Description = task.Description;
        beforeTask.DueDate = task.DueDate;
        beforeTask.Status = task.Status;
    }

    public async Task DeleteTaskAsync(string id)
    {
        var beforeTask = await GetTaskAsync(id);
        if (beforeTask is null)
        {
            throw new InvalidOperationException($"Task Id {id} が存在しません");
        }

        _context.Remove(beforeTask);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}