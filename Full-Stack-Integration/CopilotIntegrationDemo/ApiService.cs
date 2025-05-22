using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CopilotIntegrationDemo
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Sends a GET request to the specified URL and returns the response content as a string.
        /// </summary>
        /// <param name="url">The URL of the web API endpoint.</param>
        /// <returns>The response content as a string.</returns>
        public async Task<string> GetAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("URL cannot be null or empty.", nameof(url));

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}