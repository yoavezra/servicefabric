// <copyright file="HttpClientUtils.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Common.ClientSdk
{
    using System;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using ServiceSample.Common.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Polly;

    public static class HttpClientUtils
    {
        public static async Task<HttpResponseMessage> GetResponseAsync(HttpClient httpClient, Uri uri)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                return await SendRequestAsync(httpClient, requestMessage);
            }
        }

        public static async Task<HttpResponseMessage> GetResponseAsync(HttpClient httpClient, string token, Uri uri)
        {
            using (var requestMessage = CreateHttpRequestMessage(token, uri.AbsoluteUri, HttpMethod.Get))
            {
                return await SendRequestAsync(httpClient, requestMessage);
            }
        }

        public static async Task<string> GetStringAsync(HttpClient httpClient, string token, Uri uri)
        {
            using (var requestMessage = CreateHttpRequestMessage(token, uri.AbsoluteUri, HttpMethod.Get))
            {
                var response = await SendRequestAsync(httpClient, requestMessage);
                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }

        public static async Task<T> GetAsync<T>(HttpClient httpClient, string token, Uri uri, bool markAsExpectingToFail = false)
        {
            using (var requestMessage = CreateHttpRequestMessage(token, uri.AbsoluteUri, HttpMethod.Get))
            {
                if (markAsExpectingToFail)
                {
                    requestMessage.Headers.Add(HttpHeader.ExpectingFail, "true");
                }

                var response = await SendRequestAsync(httpClient, requestMessage);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<T>(json);
            }
        }

        public static async Task<HttpResponseMessage> DeleteAsync(HttpClient httpClient, string token, Uri uri)
        {
            using (var requestMessage = CreateHttpRequestMessage(token, uri.ToString(), HttpMethod.Delete))
            {
                return await SendRequestAsync(httpClient, requestMessage);
            }
        }

        public static async Task<HttpResponseMessage> PostAsync(HttpClient httpClient, string token, Uri uri, JObject content, string correlationId = "")
        {
            using (var requestMessage = CreateHttpRequestMessage(token, uri, HttpMethod.Post, content, correlationId))
            {
                return await SendRequestAsync(httpClient, requestMessage);
            }
        }

        private static HttpRequestMessage CreateHttpRequestMessage(
            string aadToken,
            string requestUri,
            HttpMethod method)
        {
            var requestMessage = new HttpRequestMessage(method, requestUri);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue(HttpHeader.BearerToken, aadToken);
            return requestMessage;
        }

        private static HttpRequestMessage CreateHttpRequestMessage(
            string aadToken,
            Uri requestUri,
            HttpMethod method,
            JObject content,
            string correlationId = "")
        {
            var requestMessage = new HttpRequestMessage(method, requestUri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json"),
            };

            if (string.IsNullOrWhiteSpace(correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
            }

            // Will make correlation id available for our logging middleware (OperationLoggingFilter)
            requestMessage.Headers.Add(HttpHeader.CorrelationId, correlationId);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue(HttpHeader.BearerToken, aadToken);
            return requestMessage;
        }

        private static async Task<HttpResponseMessage> SendRequestAsync(HttpClient httpClient, HttpRequestMessage requestMessage)
        {
            Trace.WriteLine($"Request: {requestMessage.RequestUri}");

            var response = await httpClient.SendAsync(requestMessage).ConfigureAwait(false);

            Trace.WriteLine($"Response: {response.StatusCode}");

            await EnsureSuccessStatusCodeAsync(response).ConfigureAwait(false);

            return response;
        }

        public static async Task EnsureSuccessStatusCodeAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"ReasonPhrase: {response.ReasonPhrase}");
            int responseStatusCode = (int)response.StatusCode;
            stringBuilder.AppendLine($"StatusCode: {responseStatusCode}");
            stringBuilder.AppendLine($"Content: {content}");

            throw new HttpRequestException(stringBuilder.ToString());
        }

        public static async Task WaitForService(Func<Task> action, int retryCount, int waitBetweenFailuresInSec = 20)
        {
            var retryPolicy = Policy
                .Handle<HttpRequestException>()
                .WaitAndRetryAsync(
                    retryCount,
                    i => TimeSpan.FromSeconds(waitBetweenFailuresInSec),
                    (exception, timeSpan, count, context) =>
                    {
                        Trace.WriteLine($"Retry n°{count} out of {retryCount}: \nError: {exception.GetType().FullName}. \nDetails: {exception.ToString()}");
                    });

            await retryPolicy.ExecuteAsync(
                async () =>
                {
                    await action().ConfigureAwait(false);
                }).ConfigureAwait(false);
        }
    }
}