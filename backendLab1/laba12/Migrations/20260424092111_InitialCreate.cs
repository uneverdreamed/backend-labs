using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace laba12.Migrations
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
                    { 1, 6, "Лекция + Лабораторная", "BackEnd-разработка" },
                    { 2, 4, "Лекция + Практика", "Базы данных" },
                    { 3, 4, "Лекция + Практика", "Веб-программирование и дизайн" }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "CreatedAt", "Group", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 4, 19, 6, 0, 0, 0, DateTimeKind.Utc), "241-333", "Алиханов Богдан" },
                    { 2, new DateTime(2026, 4, 19, 6, 0, 0, 0, DateTimeKind.Utc), "241-333", "Бабкин Ярослав" },
                    { 3, new DateTime(2026, 4, 19, 6, 0, 0, 0, DateTimeKind.Utc), "241-334", "Богомолова Дарья" }
                });

            migrationBuilder.InsertData(
                table: "Enrolls",
                columns: new[] { "Id", "CourseId", "EnrolledAt", "Grade", "StudentId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 4, 19, 6, 0, 0, 0, DateTimeKind.Utc), "5", 1 },
                    { 2, 2, new DateTime(2026, 4, 19, 6, 0, 0, 0, DateTimeKind.Utc), "4", 1 },
                    { 3, 1, new DateTime(2026, 4, 19, 6, 0, 0, 0, DateTimeKind.Utc), null, 2 },
                    { 4, 3, new DateTime(2026, 4, 19, 6, 0, 0, 0, DateTimeKind.Utc), "3", 3 }
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
