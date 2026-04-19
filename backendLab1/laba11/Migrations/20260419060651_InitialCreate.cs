using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace laba11.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Credits = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Group = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Enrolls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false),
                    CourseId = table.Column<int>(type: "INTEGER", nullable: false),
                    EnrolledAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Grade = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrolls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enrolls_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enrolls_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "Credits", "Description", "Name" },
                values: new object[,]
                {
                    { 1, 4, "Практика", "Основы технологического предпринимательства" },
                    { 2, 4, "Практика", "Проектная деятельность" },
                    { 3, 6, "Лекция + Лабораторная", "BackEnd-разработка" },
                    { 4, 4, "Лекция + Практика", "Базы данных" },
                    { 5, 4, "Лекция + Лабораторная", "Системный анализ" },
                    { 6, 4, "Лекция + Практика", "Веб-программирование и дизайн" },
                    { 7, 4, "Лекция + Пратика", "Математическая логика и дискретная математика" },
                    { 8, 4, "Лабораторная", "Шаблоны проектирования" },
                    { 9, 4, "Английский, Практика", "Иностранный язык" },
                    { 10, 4, "Практика", "Общая физическая подготовка" }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "CreatedAt", "Group", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(3752), "241-333", "Алиханов Богдан" },
                    { 2, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(3758), "241-333", "Бабкин Ярослав" },
                    { 3, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(3760), "241-334", "Богомолова Дарья" },
                    { 4, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(3762), "241-334", "Болотная Дарья" },
                    { 5, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(3764), "241-334", "Бычков Василий" },
                    { 6, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(3765), "241-334", "Василенко Наталия" },
                    { 7, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(3767), "241-334", "Воробьева Вероника" },
                    { 8, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(3769), "241-334", "Газаматов Ислам" },
                    { 9, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(3771), "241-334", "Долбышев Даниил" },
                    { 10, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(3772), "241-334", "Дунаева Ева" },
                    { 11, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(3774), "241-334", "Езерская Маргарита" },
                    { 12, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(3776), "241-334", "Зайцев Владислав" },
                    { 13, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(3777), "241-334", "Идрисов Тамерлан" },
                    { 14, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(3779), "241-334", "Катышев Илья" },
                    { 15, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(3781), "241-334", "Кондратенко Дмитрий" },
                    { 16, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(3782), "241-334", "Погонцев Даниил" },
                    { 17, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(3784), "241-334", "Романенко Николай" },
                    { 18, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(3785), "241-334", "Синицина Диана" },
                    { 19, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(3787), "241-334", "Смирнов Даниил" },
                    { 20, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(3789), "241-334", "Топин Прохор" },
                    { 21, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(3790), "241-334", "Федукина Мария" },
                    { 22, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(3792), "241-334", "Хачатурян Тигран" },
                    { 23, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(3794), "241-334", "Шувалов Дмитрий" }
                });

            migrationBuilder.InsertData(
                table: "Enrolls",
                columns: new[] { "Id", "CourseId", "EnrolledAt", "Grade", "StudentId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4112), "5", 1 },
                    { 2, 3, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4115), "4", 1 },
                    { 3, 5, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4117), null, 1 },
                    { 4, 2, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4119), "4", 2 },
                    { 5, 4, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4121), "5", 2 },
                    { 6, 1, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4122), "3", 3 },
                    { 7, 6, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4124), null, 3 },
                    { 8, 2, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4126), "5", 4 },
                    { 9, 7, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4127), "4", 4 },
                    { 10, 9, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4130), null, 4 },
                    { 11, 3, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4131), "4", 5 },
                    { 12, 8, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4133), "3", 5 },
                    { 13, 1, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4135), "5", 6 },
                    { 14, 4, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4136), null, 6 },
                    { 15, 10, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4138), "4", 6 },
                    { 16, 2, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4140), "3", 7 },
                    { 17, 5, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4141), "5", 7 },
                    { 18, 6, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4143), null, 8 },
                    { 19, 9, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4144), "4", 8 },
                    { 20, 3, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4146), "5", 9 },
                    { 21, 7, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4147), "4", 9 },
                    { 22, 10, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4149), null, 9 },
                    { 23, 1, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4151), "3", 10 },
                    { 24, 8, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4153), "5", 10 },
                    { 25, 2, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4154), "4", 11 },
                    { 26, 6, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4156), null, 11 },
                    { 27, 9, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4157), "5", 11 },
                    { 28, 4, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4159), "3", 12 },
                    { 29, 7, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4160), "4", 12 },
                    { 30, 1, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4162), "5", 13 },
                    { 31, 5, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4163), null, 13 },
                    { 32, 10, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4164), "4", 13 },
                    { 33, 2, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4166), "3", 14 },
                    { 34, 8, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4168), "5", 14 },
                    { 35, 3, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4169), null, 15 },
                    { 36, 6, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4171), "4", 15 },
                    { 37, 9, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4172), "3", 15 },
                    { 38, 1, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4173), "4", 16 },
                    { 39, 7, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4175), "5", 16 },
                    { 40, 4, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4177), "3", 17 },
                    { 41, 10, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4178), null, 17 },
                    { 42, 2, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4179), "4", 17 },
                    { 43, 5, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4181), "5", 18 },
                    { 44, 8, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4182), "4", 18 },
                    { 45, 3, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4184), null, 19 },
                    { 46, 6, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4185), "3", 19 },
                    { 47, 9, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4187), "5", 19 },
                    { 48, 1, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4188), "4", 20 },
                    { 49, 7, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4189), null, 20 },
                    { 50, 2, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4190), "5", 21 },
                    { 51, 5, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4192), "5", 21 },
                    { 52, 10, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4193), "5", 21 },
                    { 53, 4, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4194), null, 22 },
                    { 54, 8, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4196), "5", 22 },
                    { 55, 3, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4197), "4", 23 },
                    { 56, 6, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4201), "3", 23 },
                    { 57, 9, new DateTime(2026, 4, 19, 6, 6, 51, 186, DateTimeKind.Utc).AddTicks(4202), null, 23 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Enrolls_CourseId",
                table: "Enrolls",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrolls_StudentId",
                table: "Enrolls",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enrolls");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}
