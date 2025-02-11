namespace TodoList.Entities;

public enum TaskStatus
{
    Todo, Doing, Done
}

public class TaskEntity
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get ; set; }
    public DateTime DueDate { get; set; }
    public DateTime ModifiedAt { get; set; }
    public TaskStatus Status { get; set; }

    private TaskEntity() {}

    public TaskEntity(string title)
    {
        Id = Guid.NewGuid().ToString();
        Title = title;
        Description = string.Empty;
        DueDate = DateTime.Now;
        ModifiedAt = DateTime.Now;
    }
}