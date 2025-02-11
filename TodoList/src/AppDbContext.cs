using Microsoft.EntityFrameworkCore;

using TodoList.Entities;

namespace TodoList;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<TaskEntity> Tasks { get; set; }
}