using TodoList;
using TodoList.Entities;
using TodoList.Repositories;
using Microsoft.EntityFrameworkCore;
namespace TodoList.Test;

public class TaskRepositoryTests
{
    // テストごとにユニークな InMemory データベースを利用するためのヘルパー
    private DbContextOptions<AppDbContext> CreateNewContextOptions()
    {
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task AddTask_Should_Add_Task_Successfully()
    {
        // Arrange: ユニークなデータベース名でオプションを作成
        var options = CreateNewContextOptions();

        // 初期化とリポジトリ生成（AddTaskAsync の呼び出し前）
        using (var context = new AppDbContext(options))
        {
            var repository = new TaskRepository(context);
            var task = new TaskEntity("Test Task")
            {
                Description = "This is a test task.",
                DueDate = DateTime.Now.AddDays(1),
                Status = TodoList.Entities.TaskStatus.Todo
            };

            await repository.AddTaskAsync(task);
            await repository.SaveChangesAsync();
        }

        // Assert: 別のコンテキストインスタンスでデータが反映されているか確認
        using (var context = new AppDbContext(options))
        {
            var tasks = await context.Tasks.ToListAsync();
            Assert.Single(tasks);

            var addedTask = tasks.First();
            Assert.Equal("Test Task", addedTask.Title);
            Assert.Equal(TodoList.Entities.TaskStatus.Todo, addedTask.Status);
        }
    }
}