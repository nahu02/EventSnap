namespace ApiTest
{
    internal class LoggingHttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient(string name)
        {
            var handler = new HttpClientHandler();
            var client = new HttpClient(new LoggingHandler(handler));
            return client;
        }

        public class LoggingHandler : DelegatingHandler
        {
            public LoggingHandler(HttpMessageHandler innerHandler) : base(innerHandler)
            {
            }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                Console.WriteLine(request.ToString());
                Console.WriteLine(await request.Content.ReadAsStringAsync());
                var response = await base.SendAsync(request, cancellationToken);
                Console.WriteLine(response.ToString());
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                return response;
            }
        }
    }
}