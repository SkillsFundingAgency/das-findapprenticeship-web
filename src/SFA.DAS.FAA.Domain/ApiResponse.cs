using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.FAA.Domain
{
    public class ApiResponse<TResponse>
    {
        public TResponse Body { get; }
        public HttpStatusCode StatusCode { get; }
        public string ErrorContent { get; }

        public ApiResponse(TResponse body, HttpStatusCode statusCode, string errorContent)
        {
            Body = body;
            StatusCode = statusCode;
            ErrorContent = errorContent;
        }
    }
}
