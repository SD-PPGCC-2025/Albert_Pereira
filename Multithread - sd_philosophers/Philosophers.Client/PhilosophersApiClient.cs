using System.Net.Http.Json;

namespace Philosophers.Client
{
    public class PhilosophersApiClient
    {
        private readonly HttpClient _httpClient;

        public PhilosophersApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task StartPhilosopherAsync(int id)
        {
            await _httpClient.PostAsync($"api/philosophers/start/{id}", null);
        }

        public async Task<Dictionary<int, string>> GetStatesAsync()
        {
            return await _httpClient.GetFromJsonAsync<Dictionary<int, string>>("api/philosophers/states");
        }
    }
}
