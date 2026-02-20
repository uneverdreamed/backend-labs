using lab2;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic);
    options.SerializerOptions.WriteIndented = true;
});

var app = builder.Build();

var users = new List<User>
{
    new User { Id = 1, Name = "Имя1", Age = 21, Email = "imya1@pochta.com"},
    new User { Id = 2, Name = "Имя2", Age = 31, Email = "imya2@pochta.com"},
    new User { Id = 3, Name = "Имя3", Age = 41, Email = "imya3@pochta.com"}

};

app.Run();
