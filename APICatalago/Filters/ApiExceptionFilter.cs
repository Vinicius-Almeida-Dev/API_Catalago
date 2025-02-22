using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace APICatalago.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ApiExceptionFilter> _logger;


        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _logger = logger;
        }

        void IExceptionFilter.OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Ocorreu uma exceção não tratada: Status Code 500");
            context.Result = new ObjectResult("Ocorreu um problema ao tratar sua exceção: Status Code 500\n Exception : " + context.Exception)
            {
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }
}
