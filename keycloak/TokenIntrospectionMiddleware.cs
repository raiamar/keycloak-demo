using System.Net;
using System.Text.Json;

public class TokenIntrospectionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public TokenIntrospectionMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
        _httpClient = new HttpClient();
    }

    public async Task Invoke(HttpContext context)
    {
        string token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token != null)
        {
            var introspectionUrl = _configuration["Keycloak:IntrospectionUrl"];
            var response = await _httpClient.PostAsync(introspectionUrl, new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("token", token),
                new KeyValuePair<string, string>("client_id", _configuration["Keycloak:ClientId"]),
                new KeyValuePair<string, string>("client_secret", _configuration["Keycloak:ClientSecret"]),
            }));

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonDocument.Parse(content);

                // Check if the 'active' field exists and is a boolean
                if (tokenResponse.RootElement.TryGetProperty("active", out var activeElement) &&
                  activeElement.ValueKind == JsonValueKind.True || activeElement.ValueKind == JsonValueKind.False)
                {
                    bool isActive = activeElement.GetBoolean();
                    if (isActive)
                    {
                        await _next(context); // Token is valid, continue the pipeline
                        return;
                    }
                    //else { context.Request.Headers.Remove("Authorization"); }
                }
            }

            // If we reach here, token is not valid
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return;
        }

        // No token present in the request
        await _next(context);
    }
}
