namespace GerenciadorTarefas.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning(ex, "A resposta já foi iniciada quando ocorreu uma exceção.");
                    throw;
                }

                var (statusCode, title) = MapearExcecao(ex);

                _logger.LogError(ex, "Erro tratado pelo ExceptionMiddleware: {Title}", title);

                context.Response.Clear();
                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/problem+json";

                var problem = new ProblemDetails
                {
                    Status = statusCode,
                    Title = title,
                    Detail = _env.IsDevelopment() ? ex.Message : null,
                    Instance = context.Request.Path
                };

                if (_env.IsDevelopment())
                {
                    problem.Extensions["stackTrace"] = ex.StackTrace;
                    problem.Extensions["exceptionType"] = ex.GetType().FullName;
                }

                var json = System.Text.Json.JsonSerializer.Serialize(problem);
                await context.Response.WriteAsync(json);
            }
        }

        private static (int StatusCode, string Title) MapearExcecao(Exception ex)
        {
            return ex switch
            {
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Não autorizado"),
                KeyNotFoundException => (StatusCodes.Status404NotFound, "Recurso não encontrado"),
                ArgumentException or InvalidOperationException => (StatusCodes.Status400BadRequest, "Requisição inválida"),
                Microsoft.EntityFrameworkCore.DbUpdateException => (StatusCodes.Status409Conflict, "Conflito ao salvar dados"),
                _ => (StatusCodes.Status500InternalServerError, "Erro interno do servidor")
            };
        }
    }
}
