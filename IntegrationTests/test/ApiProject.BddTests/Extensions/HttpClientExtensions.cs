using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ApiProject.Tests.Extensions
{
    public class TestHttpResponseMessage<T>
    {
        public HttpResponseMessage WebResponse { get; set; }
        public T TestObject { get; set; }
    }

    public static class HttpClientExtensions
    {
        public static async Task<T> GetAsync<T>(this HttpClient client, string requestUrl, string token = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Accept.ParseAdd("application/json");

            if (!string.IsNullOrWhiteSpace(token))
                request.Headers.Add("Authorization", $"Bearer {token}");

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static async Task<HttpResponseMessage> GetAsync(this HttpClient client, string requestUrl, string token = null, string language = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Accept.ParseAdd("application/json");

            if (!string.IsNullOrWhiteSpace(token))
                request.Headers.Add("Authorization", $"Bearer {token}");

            if (!string.IsNullOrWhiteSpace(language))
                request.Headers.Add("Accept-Language", language);

            var response = await client.SendAsync(request);
            return response;
        }

        public static async Task<TestHttpResponseMessage<TResponse>> PostAsyncForTestResponse<TRequest, TResponse>(this HttpClient client, string requestUrl, TRequest model, string token = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
            request.Headers.Accept.ParseAdd("application/json");

            if (!string.IsNullOrWhiteSpace(token))
                request.Headers.Add("Authorization", $"Bearer {token}");

            var payload = JsonConvert.SerializeObject(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            request.Content = content;

            var response = await client.SendAsync(request);
            //response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return new TestHttpResponseMessage<TResponse>()
            {
                WebResponse = response,
                TestObject = JsonConvert.DeserializeObject<TResponse>(json)
            };
        }

        public static async Task<TResponse> PostAsync<TRequest, TResponse>(this HttpClient client, string requestUrl, TRequest model, string token = null)
        {
            return (await PostAsyncForTestResponse<TRequest, TResponse>(client, requestUrl, model, token)).TestObject;
        }

        public static async Task<HttpResponseMessage> PostAsync(this HttpClient client, string requestUrl, object model, string token = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
            request.Headers.Accept.ParseAdd("application/json");

            if (!string.IsNullOrWhiteSpace(token))
                request.Headers.Add("Authorization", $"Bearer {token}");

            var payload = JsonConvert.SerializeObject(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            request.Content = content;

            var response = await client.SendAsync(request);
            return response;
        }

        public static async Task<HttpResponseMessage> PutAsync(this HttpClient client, string requestUrl, object model, string token = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, requestUrl);
            request.Headers.Accept.ParseAdd("application/json");

            if (!string.IsNullOrWhiteSpace(token))
                request.Headers.Add("Authorization", $"Bearer {token}");

            var payload = JsonConvert.SerializeObject(model);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            request.Content = content;

            var response = await client.SendAsync(request);
            return response;
        }

        public static async Task<HttpResponseMessage> OptionsAsync(this HttpClient client, string requestUrl, string token = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Options, requestUrl);
            request.Headers.Accept.ParseAdd("application/json");

            if (!string.IsNullOrWhiteSpace(token))
                request.Headers.Add("Authorization", $"Bearer {token}");

            var response = await client.SendAsync(request);
            return response;
        }

        public static async Task<HttpResponseMessage> DeleteAsync(this HttpClient client, string requestUrl, string token = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, requestUrl);
            request.Headers.Accept.ParseAdd("application/json");

            if (!string.IsNullOrWhiteSpace(token))
                request.Headers.Add("Authorization", $"Bearer {token}");

            var response = await client.SendAsync(request);
            return response;
        }
    }
}