using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using SFA.DAS.FAA.Domain.Exceptions;

namespace SFA.DAS.FAA.Web.Infrastructure
{
    public class ResourceNotFoundExceptionHandler : IExceptionHandler
    {
        public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not ResourceNotFoundException)
            {
                return ValueTask.FromResult(false);
            }

            httpContext.Response.StatusCode = (int) HttpStatusCode.NotFound;
            return ValueTask.FromResult(true);
        }
    }
}
