using laba11.Models;
using Microsoft.EntityFrameworkCore;

namespace laba11.data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

\        public DbSet<Student> Students => Set<Student>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Enrollment> Enrollments => Set<Enrollment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка связей и ограничений через Fluent API

            modelBuilder.Entity<Enroll>(entity =>
            {
                // Связь Enrollment -> Student: много записей у одного студента
                entity.HasOne(e => e.Student)
                      .WithMany(s => s.Enrollments)
                      .HasForeignKey(e => e.StudentId)
                      .OnDelete(DeleteBehavior.Cascade); // при удалении студента удаляются его записи

                // Связь Enrollment -> Course: много записей у одного курса
                entity.HasOne(e => e.Course)
                      .WithMany(c => c.Enrollments)
                      .HasForeignKey(e => e.CourseId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Начальные данные (seed data) — заполняются при первой миграции
            modelBuilder.Entity<Student>().HasData(
                new Student { Id = 1, Name = "Иванов Иван", Group = "241-333", CreatedAt = DateTime.UtcNow },
                new Student { Id = 2, Name = "Петрова Мария", Group = "241-333", CreatedAt = DateTime.UtcNow },
                new Student { Id = 3, Name = "Сидоров Алексей", Group = "241-334", CreatedAt = DateTime.UtcNow }
            );

            modelBuilder.Entity<Course>().HasData(
                new Course { Id = 1, Name = "Математика", Description = "Высшая математика", Credits = 5 },
                new Course { Id = 2, Name = "Физика", Description = "Общая физика", Credits = 4 },
                new Course { Id = 3, Name = "Программирование", Description = "Backend-разработка", Credits = 6 }
            );

            modelBuilder.Entity<Enrollment>().HasData(
                new Enrollment { Id = 1, StudentId = 1, CourseId = 1, EnrolledAt = DateTime.UtcNow, Grade = "5" },
                new Enrollment { Id = 2, StudentId = 1, CourseId = 3, EnrolledAt = DateTime.UtcNow, Grade = null },
                new Enrollment { Id = 3, StudentId = 2, CourseId = 2, EnrolledAt = DateTime.UtcNow, Grade = "4" }
            );
        }
    }
}