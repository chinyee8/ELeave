using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;

public class ApiHelper
{
    private readonly HttpClient _http;
    private readonly string _base;

    public ApiHelper(IHttpClientFactory factory, IConfiguration config)
    {
        _http = factory.CreateClient();
        _base = config["ApiSettings:BaseUrl"]!;
    }

    public void SetToken(string token)
    {
        _http.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        var response = await _http.GetAsync(_base + endpoint);
        if (!response.IsSuccessStatusCode) return default;
        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(json);
    }

    public async Task<(bool Success, string Body)>
        PostAsync<T>(string endpoint, T data)
    {
        var json = JsonConvert.SerializeObject(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _http.PostAsync(_base + endpoint, content);
        var body = await response.Content.ReadAsStringAsync();
        return (response.IsSuccessStatusCode, body);
    }
}

