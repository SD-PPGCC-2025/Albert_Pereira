using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace GEDAI.Api.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly TokenResponse _tokenResponse;
        public BaseController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _tokenResponse = new TokenResponse();
            _tokenResponse.Token = GetTokenAsync("admin", "admin").Result;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenResponse.Token);
        }

        private async Task<string> GetTokenAsync(string username, string password)
        {
            var requestContent = new StringContent(JsonConvert.SerializeObject(new
            {
                Username = username,
                Password = password
            }), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7136/v1/account/token", requestContent);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);
                return tokenResponse.Token;
            }

            throw new Exception("Failed to retrieve token");
        }

        public class TokenResponse
        {
            public string Token { get; set; }
        }
    }
}
