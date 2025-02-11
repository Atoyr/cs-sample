namespace TodoList.Models;

public record CreateTaskModel(
        string Title, 
        string Description, 
        DateTime? DueDate, 
        string Status
        );

public record TaskModel(
        string Id, 
        string Title, 
        string Description, 
        DateTime DueDate, 
        DateTime ModifiedAt, 
        string Status
        );