using laba16.Models;
using Microsoft.EntityFrameworkCore;

namespace laba16.Data;

// Контекст базы данных с пользователями и учебными сущностями
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Enroll> Enrolls => Set<Enroll>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Начальные пользователи с разными ролями для демонстрации авторизации
        // В реальном приложении пользователи создаются через регистрацию, а не через seed
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Login = "admin", Password = "admin123", Role = "Admin" },
            new User { Id = 2, Login = "manager", Password = "manager123", Role = "Manager" },
            new User { Id = 3, Login = "user", Password = "user123", Role = "User" }
        );

        // Несколько студентов для проверки защищённых эндпоинтов
        // Фиксированная дата вместо DateTime.Now, чтобы EnsureCreated не ругался на нестабильные данные
        modelBuilder.Entity<Student>().HasData(
            new Student { Id = 1, Name = "Иванов Иван", Group = "241-333", CreatedAt = new DateTime(2026, 1, 1) },
            new Student { Id = 2, Name = "Петров Пётр", Group = "241-333", CreatedAt = new DateTime(2026, 1, 1) },
            new Student { Id = 3, Name = "Аннова Анна", Group = "242-444", CreatedAt = new DateTime(2026, 1, 1) }
        );

        // Курсы с новыми полями: Name, Description, Credits
        modelBuilder.Entity<Course>().HasData(
            new Course { Id = 1, Name = "ASP.NET Core", Description = "Backend-разработка", Credits = 5 },
            new Course { Id = 2, Name = "Базы данных", Description = "Реляционные БД и SQL", Credits = 4 }
        );
    }
}