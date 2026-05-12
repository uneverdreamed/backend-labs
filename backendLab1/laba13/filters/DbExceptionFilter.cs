using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace laba12.filters
{
    // фильтр исключений перехватывает ошибки, возникающие при работе с базой данных
    // применяется к контроллерам через атрибут [ServiceFilter] или глобально
    public class DbExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<DbExceptionFilter> _logger;

        public DbExceptionFilter(ILogger<DbExceptionFilter> logger)
        {
            _logger = logger;
        }

        // метод вызывается ASP.NET Core при возникновении необработанного исключения в контроллере
        public void OnException(ExceptionContext context)
        {
            // проверка, является ли исключение ошибкой EF Core (DbUpdateException)
            if (context.Exception is DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Ошибка при обновлении базы данных");

                // определеие типа ошибки по внутреннему исключению
                var innerMessage = dbEx.InnerException?.Message ?? dbEx.Message;
                var statusCode = 409; // Conflict — по умолчанию для ошибок БД
                var userMessage = "Ошибка при сохранении данных";

                // нарушение уникальности (UNIQUE constraint)
                if (innerMessage.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase))
                {
                    userMessage = "Запись с такими данными уже существует";
                }
                // нарушение внешнего ключа (FOREIGN KEY constraint)
                else if (innerMessage.Contains("FOREIGN KEY", StringComparison.OrdinalIgnoreCase))
                {
                    userMessage = "Указанная связанная запись не найдена в базе данных";
                    statusCode = 400; // Bad Request — клиент передал неверный FK
                }

                // формирование структурированного ответа об ошибке
                context.Result = new ObjectResult(new
                {
                    error = userMessage,
                    detail = innerMessage,
                    statusCode = statusCode
                })
                {
                    StatusCode = statusCode
                };

                // пометка исключения как обработанное — оно не пойдёт дальше по конвейеру
                context.ExceptionHandled = true;
            }
        }
    }
}