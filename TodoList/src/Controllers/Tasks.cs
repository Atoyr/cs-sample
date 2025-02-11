using Microsoft.AspNetCore.Mvc;

using TodoList.Repositories;
using TodoList.Models;
using TodoList.Entities;

namespace TaskList.Controllers;

public class TasksController : ControllerBase
{

    private readonly ITaskRepository _repository;

    public TasksController(ITaskRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// GET
    /// </summary>
    /// <returns>実行結果</returns>
    [HttpGet()]
    [Route("[controller]")]
    [ProducesResponseType<IEnumerable<TaskModel>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Index()
    {
        IEnumerable<TaskModel> tasks = Enumerable.Empty<TaskModel>();
        try
        {
            var taskEntities = await _repository.GetAllTasksAsync();
            tasks = taskEntities.Select(t => ConvertToTaskModel(t));
        }
        catch (Exception ex)
        {
            return StatusCode(
                    StatusCodes.Status500InternalServerError, 
                    new { Message = "内部サーバーエラーが発生しました。", Details = ex.Message }
                    );
        }
        return Ok(tasks);
    }

    /// <summary>
    /// GET
    /// </summary>
    /// <returns>実行結果</returns>
    [HttpGet()]
    [Route("[controller]/{id}")]
    [ProducesResponseType<IEnumerable<TaskModel>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Index(string id)
    {
        try
        {
            var taskEntitiy = await _repository.GetTaskAsync(id);
            if (taskEntitiy is null)
            {
                return NotFound();
            }
            return Ok(ConvertToTaskModel(taskEntitiy));
        }
        catch (Exception ex)
        {
            return StatusCode(
                    StatusCodes.Status500InternalServerError, 
                    new { Message = "内部サーバーエラーが発生しました。", Details = ex.Message }
                    );
        }
    }

    /// <summary>
    /// POST
    /// </summary>
    /// <returns>実行結果</returns>
    [HttpPost()]
    [Route("[controller]")]
    [ProducesResponseType<CreateTaskModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateTask(CreateTaskModel newTask)
    {
        try
        {
            var task = new TaskEntity(newTask.Title)
            {
                Description = newTask.Description, 
                DueDate = newTask.DueDate ?? DateTime.Now, 
                Status = StringToStatsuCode(newTask.Status), 
            };

            await _repository.AddTaskAsync(task);
            await _repository.SaveChangesAsync();
            return Ok(new { status = "succsss", id = task.Id});
        }
        catch (Exception ex)
        {
            return StatusCode(
                    StatusCodes.Status500InternalServerError, 
                    new { Message = "内部サーバーエラーが発生しました。", Details = ex.Message }
                    );
        }
    }

    /// <summary>
    /// PUT
    /// </summary>
    /// <returns>実行結果</returns>
    [HttpPut()]
    [Route("[controller]/{id}")]
    [ProducesResponseType<CreateTaskModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Consumes("application/json")]
    public async Task<IActionResult> UpdateTask(
            [FromRoute]  string id, 
            [FromBody] TaskModel updateTask)
    {
        if (id != updateTask.Id)
        {
            return BadRequest();
        }
        try
        {
            var task = ConvertToTaskEntity(updateTask);

            await _repository.UpdateTaskAsync(id, task);
            await _repository.SaveChangesAsync();
            return Ok(new { status = "succsss", id = task.Id});
        }
        catch (InvalidDataException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return StatusCode(
                    StatusCodes.Status500InternalServerError, 
                    new { Message = "内部サーバーエラーが発生しました。", Details = ex.Message }
                    );
        }
    }

    /// <summary>
    /// DELETE
    /// </summary>
    /// <returns>実行結果</returns>
    [HttpDelete()]
    [Route("[controller]/{id}")]
    [ProducesResponseType<CreateTaskModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await _repository.DeleteTaskAsync(id);
            await _repository.SaveChangesAsync();
            return Ok(new { status = "succsss"});
        }
        catch (InvalidDataException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return StatusCode(
                    StatusCodes.Status500InternalServerError, 
                    new { Message = "内部サーバーエラーが発生しました。", Details = ex.Message }
                    );
        }
    }

    private TaskModel ConvertToTaskModel(TaskEntity entity)
    {
        return new TaskModel(
                entity.Id, 
                entity.Title,
                entity.Description, 
                entity.DueDate, 
                entity.ModifiedAt, 
                StatsuCodeToString(entity.Status)
                );

    }

    private TaskEntity ConvertToTaskEntity(TaskModel entity)
    {
        return new TaskEntity( entity.Title)
        {
                Id = entity.Id, 
                Description = entity.Description, 
                DueDate = entity.DueDate, 
                ModifiedAt = entity.ModifiedAt, 
                Status = StringToStatsuCode(entity.Status)
        };
    }

    private string StatsuCodeToString(TodoList.Entities.TaskStatus status)
    {
        return status switch {
            TodoList.Entities.TaskStatus.Todo => "Todo", 
                TodoList.Entities.TaskStatus.Doing => "Doing", 
                TodoList.Entities.TaskStatus.Done => "Done", 
                _ => "Todo"
        };
    }

    private TodoList.Entities.TaskStatus StringToStatsuCode(string status)
    {
        return status?.ToLower() switch {
            "todo"=> TodoList.Entities.TaskStatus.Todo, 
            "doing"=> TodoList.Entities.TaskStatus.Doing, 
            "done"=> TodoList.Entities.TaskStatus.Done, 
            _ => TodoList.Entities.TaskStatus.Todo, 
        };
    }
}