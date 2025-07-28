using entain2.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace entain2
{
    public class LoggingHandler : DelegatingHandler
    {
        private readonly Logger logger;
        public LoggingHandler(HttpMessageHandler innerHandler, Logger logger) : base(innerHandler)
        {
            this.logger = logger;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            logger.Log($"Request: {request.Method} {request.RequestUri}");
            if (request.Content != null)
            {
                var requestBody = await request.Content.ReadAsStringAsync(cancellationToken);
                logger.Log($"Request Body:\n{FormatJson(requestBody)}");
            }
            var response = await base.SendAsync(request, cancellationToken);
            logger.Log($"Response: {(int)response.StatusCode} {response.ReasonPhrase}");
            if (response.Content != null)
            {
                var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
                logger.Log($"Response Body:\n{FormatJson(responseBody)}");
                response.Content = new StringContent(responseBody, Encoding.UTF8, response.Content.Headers?.ContentType?.MediaType ?? "application/json");
            }
            return response;
        }
        public static string FormatJson(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return "(empty)";
            try
            {
                var parsed = JToken.Parse(content);
                return parsed.ToString(Formatting.Indented);
            }
            catch (JsonReaderException)
            {
                return content;
            }
        }
    }
}