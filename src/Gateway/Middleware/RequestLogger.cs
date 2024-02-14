using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Gateway.Middleware
{
    
    /// <summary>
    /// Middleware to log the duration of each request.
    /// </summary>
    public class RequestLogger
    {
       
        /// <summary>
        /// The delegate for the next middleware in the pipeline.
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// The logger used to log information about the requests.
        /// </summary>
        /// <remarks>
        /// This logger is specific to the <see cref="RequestLogger"/> middleware, 
        /// and it is used to log details about each incoming HTTP request.
        /// </remarks>
        private readonly ILogger<RequestLogger> _logger;

        /// <summary>
        /// Constructor that initializes a new instance of the <see cref="RequestLogger"/> class.
        /// </summary>
        /// <param name="next">
        /// The delegate representing the next middleware in the pipeline.
        /// </param>
        /// <param name="logger">
        /// The logger used to log information about the requests.
        /// </param>
        public RequestLogger(RequestDelegate next, ILogger<RequestLogger> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Method that processes each individual HTTP request by measuring its execution time and logging the details.
        /// </summary>
        /// <param name="context">
        /// The context for the current HTTP request.
        /// </param>
        /// <remarks>
        /// This method records the time taken to process the request by invoking the next middleware in the pipeline.
        /// Once the middleware completes execution, logs the request method, path, and processing time.
        /// </remarks>
        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            await _next(context); 

            stopwatch.Stop();
            _logger.LogInformation(
                $"Request {context.Request.Method} {context.Request.Path} processed in {stopwatch.ElapsedMilliseconds} ms");
        }
    }

    /// <summary>
    /// Extension method to add the <see cref="RequestLogger"/> middleware to an application's request pipeline.
    /// </summary>
    public static class RequestLoggerExtensions
    {
        /// <summary>
        /// Method to add the <see cref="RequestLogger"/> middleware to an application's request pipeline.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="IApplicationBuilder"/> instance to which the middleware is added.
        /// </param>
        /// <returns>
        /// The <see cref="IApplicationBuilder"/> instance with the <see cref="RequestLogger"/> middleware added.
        /// </returns>
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLogger>();
        }
    }
}