using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mixlr.ApiClient.Interfaces
{
    public abstract class BaseApiClient
    {
        protected static HttpClient _client;

        protected BaseApiClient(string apiUrl)
        {
            _client = new HttpClient { BaseAddress = new Uri(apiUrl) };
        }

        #region HttpMethods

        public static async Task<T> Get<T>(string url)
        {
            var response = await _client.GetAsync(url);

            return await ParseResponse<T>(response, url);
        }

        public static async Task<T> Post<T>(string url, HttpContent content)
        {
            var response = await _client.PostAsync(url, content);

            return await ParseResponse<T>(response, url);
        }

        public static async Task<T> Put<T>(string url, HttpContent content)
        {
            var response = await _client.PutAsync(url, content);

            return await ParseResponse<T>(response, url);
        }

        public static async Task<T> Patch<T>(string url, HttpContent content)
        {
            var response = await _client.PatchAsync(url, content);

            return await ParseResponse<T>(response, url);
        }

        public static async Task<bool> Delete<T>(string url)
        {
            var response = await _client.DeleteAsync(url);

            return response.IsSuccessStatusCode;
        }

        #endregion

        private static async Task<T> ParseResponse<T>(HttpResponseMessage response, string url)
        {
            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    var parsedError = JObject.Parse(await response.Content.ReadAsStringAsync());
                    var errorString = string.Empty;

                    if (parsedError["error"] != null) { errorString = parsedError["error"].ToString(); }
                    else if (parsedError["errors"] != null) { errorString = parsedError["errors"][0].ToString(); }

                    throw new Models.Exceptions.MixlrException
                    {
                        Endpoint = url,
                        Error = errorString
                    };
                }
                catch
                {
                    throw new Exception($"Bad response from Mixlr: {response.StatusCode}"
                        + $"{Environment.NewLine}Details: {await response.Content.ReadAsStringAsync()}");
                }
            }

            var json = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(json)) { return default; }

            var parsedJson = JObject.Parse(json);

            return JsonConvert.DeserializeObject<T>(parsedJson.ToString());
        }
    }
}
