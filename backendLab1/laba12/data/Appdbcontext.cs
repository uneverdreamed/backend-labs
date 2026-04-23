using laba12.models;
using Microsoft.EntityFrameworkCore;

namespace laba12.data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Student> Students => Set<Student>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Enroll> Enrolls => Set<Enroll>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // настройка связей через Fluent API
            modelBuilder.Entity<Enroll>(entity =>
            {
                entity.HasOne(e => e.Student)
                      .WithMany(s => s.Enrolls)
                      .HasForeignKey(e => e.StudentId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Course)
                      .WithMany(c => c.Enrolls)
                      .HasForeignKey(e => e.CourseId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // начальные данные
            modelBuilder.Entity<Student>().HasData(
                new Student { Id = 1, Name = "Алиханов Богдан", Group = "241-333", CreatedAt = new DateTime(2026, 4, 19, 6, 0, 0, DateTimeKind.Utc) },
                new Student { Id = 2, Name = "Бабкин Ярослав", Group = "241-333", CreatedAt = new DateTime(2026, 4, 19, 6, 0, 0, DateTimeKind.Utc) },
                new Student { Id = 3, Name = "Богомолова Дарья", Group = "241-334", CreatedAt = new DateTime(2026, 4, 19, 6, 0, 0, DateTimeKind.Utc) }
            );

            modelBuilder.Entity<Course>().HasData(
                new Course { Id = 1, Name = "BackEnd-разработка", Description = "Лекция + Лабораторная", Credits = 6 },
                new Course { Id = 2, Name = "Базы данных", Description = "Лекция + Практика", Credits = 4 },
                new Course { Id = 3, Name = "Веб-программирование и дизайн", Description = "Лекция + Практика", Credits = 4 }
            );

            modelBuilder.Entity<Enroll>().HasData(
                new Enroll { Id = 1, StudentId = 1, CourseId = 1, EnrolledAt = new DateTime(2026, 4, 19, 6, 0, 0, DateTimeKind.Utc), Grade = "5" },
                new Enroll { Id = 2, StudentId = 1, CourseId = 2, EnrolledAt = new DateTime(2026, 4, 19, 6, 0, 0, DateTimeKind.Utc), Grade = "4" },
                new Enroll { Id = 3, StudentId = 2, CourseId = 1, EnrolledAt = new DateTime(2026, 4, 19, 6, 0, 0, DateTimeKind.Utc), Grade = null },
                new Enroll { Id = 4, StudentId = 3, CourseId = 3, EnrolledAt = new DateTime(2026, 4, 19, 6, 0, 0, DateTimeKind.Utc), Grade = "3" }
            );
        }
    }
}