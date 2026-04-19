using laba11.Models;
using Microsoft.EntityFrameworkCore;

namespace laba11.data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Student> Students => Set<Student>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Enroll> Enrolls => Set<Enroll>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // настройка связей и ограничений через Fluent API

            modelBuilder.Entity<Enroll>(entity =>
            {
                // связь Enroll -> Student: много записей у одного студента
                entity.HasOne(e => e.Student)
                      .WithMany(s => s.Enrolls)
                      .HasForeignKey(e => e.StudentId)
                      .OnDelete(DeleteBehavior.Cascade); // при удалении студента удаляются его записи

                // связь Enroll -> Course: много записей у одного курса
                entity.HasOne(e => e.Course)
                      .WithMany(c => c.Enrolls)
                      .HasForeignKey(e => e.CourseId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // начальные данные (seed data) / заполняются при первой миграции
            modelBuilder.Entity<Student>().HasData(
                new Student { Id = 1, Name = "Алиханов Богдан", Group = "241-333", CreatedAt = DateTime.UtcNow },
                new Student { Id = 2, Name = "Бабкин Ярослав", Group = "241-333", CreatedAt = DateTime.UtcNow },
                new Student { Id = 3, Name = "Богомолова Дарья", Group = "241-334", CreatedAt = DateTime.UtcNow },
                new Student { Id = 4, Name = "Болотная Дарья", Group = "241-334", CreatedAt = DateTime.UtcNow },
                new Student { Id = 5, Name = "Бычков Василий", Group = "241-334", CreatedAt = DateTime.UtcNow },
                new Student { Id = 6, Name = "Василенко Наталия", Group = "241-334", CreatedAt = DateTime.UtcNow },
                new Student { Id = 7, Name = "Воробьева Вероника", Group = "241-334", CreatedAt = DateTime.UtcNow },
                new Student { Id = 8, Name = "Газаматов Ислам", Group = "241-334", CreatedAt = DateTime.UtcNow },
                new Student { Id = 9, Name = "Долбышев Даниил", Group = "241-334", CreatedAt = DateTime.UtcNow },
                new Student { Id = 10, Name = "Дунаева Ева", Group = "241-334", CreatedAt = DateTime.UtcNow },
                new Student { Id = 11, Name = "Езерская Маргарита", Group = "241-334", CreatedAt = DateTime.UtcNow },
                new Student { Id = 12, Name = "Зайцев Владислав", Group = "241-334", CreatedAt = DateTime.UtcNow },
                new Student { Id = 13, Name = "Идрисов Тамерлан", Group = "241-334", CreatedAt = DateTime.UtcNow },
                new Student { Id = 14, Name = "Катышев Илья", Group = "241-334", CreatedAt = DateTime.UtcNow },
                new Student { Id = 15, Name = "Кондратенко Дмитрий", Group = "241-334", CreatedAt = DateTime.UtcNow },
                new Student { Id = 16, Name = "Погонцев Даниил", Group = "241-334", CreatedAt = DateTime.UtcNow },
                new Student { Id = 17, Name = "Романенко Николай", Group = "241-334", CreatedAt = DateTime.UtcNow },
                new Student { Id = 18, Name = "Синицина Диана", Group = "241-334", CreatedAt = DateTime.UtcNow },
                new Student { Id = 19, Name = "Смирнов Даниил", Group = "241-334", CreatedAt = DateTime.UtcNow },
                new Student { Id = 20, Name = "Топин Прохор", Group = "241-334", CreatedAt = DateTime.UtcNow },
                new Student { Id = 21, Name = "Федукина Мария", Group = "241-334", CreatedAt = DateTime.UtcNow },
                new Student { Id = 22, Name = "Хачатурян Тигран", Group = "241-334", CreatedAt = DateTime.UtcNow },
                new Student { Id = 23, Name = "Шувалов Дмитрий", Group = "241-334", CreatedAt = DateTime.UtcNow }

            );

            modelBuilder.Entity<Course>().HasData(
                new Course { Id = 1, Name = "Основы технологического предпринимательства", Description = "Практика", Credits = 4 },
                new Course { Id = 2, Name = "Проектная деятельность", Description = "Практика", Credits = 4 },
                new Course { Id = 3, Name = "BackEnd-разработка", Description = "Лекция + Лабораторная", Credits = 6 },
                new Course { Id = 4, Name = "Базы данных", Description = "Лекция + Практика", Credits = 4 },
                new Course { Id = 5, Name = "Системный анализ", Description = "Лекция + Лабораторная", Credits = 4 },
                new Course { Id = 6, Name = "Веб-программирование и дизайн", Description = "Лекция + Практика", Credits = 4 },
                new Course { Id = 7, Name = "Математическая логика и дискретная математика", Description = "Лекция + Пратика", Credits = 4 },
                new Course { Id = 8, Name = "Шаблоны проектирования", Description = "Лабораторная", Credits = 4 },
                new Course { Id = 9, Name = "Иностранный язык", Description = "Английский, Практика", Credits = 4 },
                new Course { Id = 10, Name = "Общая физическая подготовка", Description = "Практика", Credits = 4 }
            );

            modelBuilder.Entity<Enroll>().HasData(
                new Enroll { Id = 1, StudentId = 1, CourseId = 1, EnrolledAt = DateTime.UtcNow, Grade = "5" },
                new Enroll { Id = 2, StudentId = 1, CourseId = 3, EnrolledAt = DateTime.UtcNow, Grade = "4" },
                new Enroll { Id = 3, StudentId = 1, CourseId = 5, EnrolledAt = DateTime.UtcNow, Grade = null },
                // 2
                new Enroll { Id = 4, StudentId = 2, CourseId = 2, EnrolledAt = DateTime.UtcNow, Grade = "4" },
                new Enroll { Id = 5, StudentId = 2, CourseId = 4, EnrolledAt = DateTime.UtcNow, Grade = "5" },
                // 3
                new Enroll { Id = 6, StudentId = 3, CourseId = 1, EnrolledAt = DateTime.UtcNow, Grade = "3" },
                new Enroll { Id = 7, StudentId = 3, CourseId = 6, EnrolledAt = DateTime.UtcNow, Grade = null },
                // 4
                new Enroll { Id = 8, StudentId = 4, CourseId = 2, EnrolledAt = DateTime.UtcNow, Grade = "5" },
                new Enroll { Id = 9, StudentId = 4, CourseId = 7, EnrolledAt = DateTime.UtcNow, Grade = "4" },
                new Enroll { Id = 10, StudentId = 4, CourseId = 9, EnrolledAt = DateTime.UtcNow, Grade = null },
                // 5
                new Enroll { Id = 11, StudentId = 5, CourseId = 3, EnrolledAt = DateTime.UtcNow, Grade = "4" },
                new Enroll { Id = 12, StudentId = 5, CourseId = 8, EnrolledAt = DateTime.UtcNow, Grade = "3" },
                // 6
                new Enroll { Id = 13, StudentId = 6, CourseId = 1, EnrolledAt = DateTime.UtcNow, Grade = "5" },
                new Enroll { Id = 14, StudentId = 6, CourseId = 4, EnrolledAt = DateTime.UtcNow, Grade = null },
                new Enroll { Id = 15, StudentId = 6, CourseId = 10, EnrolledAt = DateTime.UtcNow, Grade = "4" },
                // 7
                new Enroll { Id = 16, StudentId = 7, CourseId = 2, EnrolledAt = DateTime.UtcNow, Grade = "3" },
                new Enroll { Id = 17, StudentId = 7, CourseId = 5, EnrolledAt = DateTime.UtcNow, Grade = "5" },
                // 8
                new Enroll { Id = 18, StudentId = 8, CourseId = 6, EnrolledAt = DateTime.UtcNow, Grade = null },
                new Enroll { Id = 19, StudentId = 8, CourseId = 9, EnrolledAt = DateTime.UtcNow, Grade = "4" },
                // 9
                new Enroll { Id = 20, StudentId = 9, CourseId = 3, EnrolledAt = DateTime.UtcNow, Grade = "5" },
                new Enroll { Id = 21, StudentId = 9, CourseId = 7, EnrolledAt = DateTime.UtcNow, Grade = "4" },
                new Enroll { Id = 22, StudentId = 9, CourseId = 10, EnrolledAt = DateTime.UtcNow, Grade = null },
                // 10
                new Enroll { Id = 23, StudentId = 10, CourseId = 1, EnrolledAt = DateTime.UtcNow, Grade = "3" },
                new Enroll { Id = 24, StudentId = 10, CourseId = 8, EnrolledAt = DateTime.UtcNow, Grade = "5" },
                // 11
                new Enroll { Id = 25, StudentId = 11, CourseId = 2, EnrolledAt = DateTime.UtcNow, Grade = "4" },
                new Enroll { Id = 26, StudentId = 11, CourseId = 6, EnrolledAt = DateTime.UtcNow, Grade = null },
                new Enroll { Id = 27, StudentId = 11, CourseId = 9, EnrolledAt = DateTime.UtcNow, Grade = "5" },
                // 12
                new Enroll { Id = 28, StudentId = 12, CourseId = 4, EnrolledAt = DateTime.UtcNow, Grade = "3" },
                new Enroll { Id = 29, StudentId = 12, CourseId = 7, EnrolledAt = DateTime.UtcNow, Grade = "4" },
                // 13
                new Enroll { Id = 30, StudentId = 13, CourseId = 1, EnrolledAt = DateTime.UtcNow, Grade = "5" },
                new Enroll { Id = 31, StudentId = 13, CourseId = 5, EnrolledAt = DateTime.UtcNow, Grade = null },
                new Enroll { Id = 32, StudentId = 13, CourseId = 10, EnrolledAt = DateTime.UtcNow, Grade = "4" },
                // 14
                new Enroll { Id = 33, StudentId = 14, CourseId = 2, EnrolledAt = DateTime.UtcNow, Grade = "3" },
                new Enroll { Id = 34, StudentId = 14, CourseId = 8, EnrolledAt = DateTime.UtcNow, Grade = "5" },
                // 15
                new Enroll { Id = 35, StudentId = 15, CourseId = 3, EnrolledAt = DateTime.UtcNow, Grade = null },
                new Enroll { Id = 36, StudentId = 15, CourseId = 6, EnrolledAt = DateTime.UtcNow, Grade = "4" },
                new Enroll { Id = 37, StudentId = 15, CourseId = 9, EnrolledAt = DateTime.UtcNow, Grade = "3" },
                // 16
                new Enroll { Id = 38, StudentId = 16, CourseId = 1, EnrolledAt = DateTime.UtcNow, Grade = "4" },
                new Enroll { Id = 39, StudentId = 16, CourseId = 7, EnrolledAt = DateTime.UtcNow, Grade = "5" },
                // 17
                new Enroll { Id = 40, StudentId = 17, CourseId = 4, EnrolledAt = DateTime.UtcNow, Grade = "3" },
                new Enroll { Id = 41, StudentId = 17, CourseId = 10, EnrolledAt = DateTime.UtcNow, Grade = null },
                new Enroll { Id = 42, StudentId = 17, CourseId = 2, EnrolledAt = DateTime.UtcNow, Grade = "4" },
                // 18
                new Enroll { Id = 43, StudentId = 18, CourseId = 5, EnrolledAt = DateTime.UtcNow, Grade = "5" },
                new Enroll { Id = 44, StudentId = 18, CourseId = 8, EnrolledAt = DateTime.UtcNow, Grade = "4" },
                // 19
                new Enroll { Id = 45, StudentId = 19, CourseId = 3, EnrolledAt = DateTime.UtcNow, Grade = null },
                new Enroll { Id = 46, StudentId = 19, CourseId = 6, EnrolledAt = DateTime.UtcNow, Grade = "3" },
                new Enroll { Id = 47, StudentId = 19, CourseId = 9, EnrolledAt = DateTime.UtcNow, Grade = "5" },
                // 20
                new Enroll { Id = 48, StudentId = 20, CourseId = 1, EnrolledAt = DateTime.UtcNow, Grade = "4" },
                new Enroll { Id = 49, StudentId = 20, CourseId = 7, EnrolledAt = DateTime.UtcNow, Grade = null },
                // 21
                new Enroll { Id = 50, StudentId = 21, CourseId = 2, EnrolledAt = DateTime.UtcNow, Grade = "5" },
                new Enroll { Id = 51, StudentId = 21, CourseId = 5, EnrolledAt = DateTime.UtcNow, Grade = "5" },
                new Enroll { Id = 52, StudentId = 21, CourseId = 10, EnrolledAt = DateTime.UtcNow, Grade = "5" },
                // 22
                new Enroll { Id = 53, StudentId = 22, CourseId = 4, EnrolledAt = DateTime.UtcNow, Grade = null },
                new Enroll { Id = 54, StudentId = 22, CourseId = 8, EnrolledAt = DateTime.UtcNow, Grade = "5" },
                // 23
                new Enroll { Id = 55, StudentId = 23, CourseId = 3, EnrolledAt = DateTime.UtcNow, Grade = "4" },
                new Enroll { Id = 56, StudentId = 23, CourseId = 6, EnrolledAt = DateTime.UtcNow, Grade = "3" },
                new Enroll { Id = 57, StudentId = 23, CourseId = 9, EnrolledAt = DateTime.UtcNow, Grade = null }
            );
        }
    }
}