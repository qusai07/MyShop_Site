
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Net;
using MyShop_Site.Models.ResponseModels;
using MyShop_Site.Models.RequestModels;

namespace MyShop_Site.Services
{
    public class MasterService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<MasterService> _logger;
        private string _authToken = string.Empty;
        private DateTime _tokenExpiry = DateTime.MinValue;

        public string MasterBaseUrl { get; }

        public MasterService(HttpClient httpClient, IConfiguration configuration, ILogger<MasterService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
            MasterBaseUrl = _configuration["MasterAPI:BaseUrl"] ?? "https://api.yourservice.com";
        }

        public async Task<bool> AuthenticateAsync(string username, string password)
        {
            try
            {
                var loginRequest = new LoginRequestModel
                {
                    Username = username,
                    Password = password
                };

                var response = await RequestMasterAsync<LoginResponseModel>("auth/login", loginRequest);
                
                if (response.IsSuccess && response is LoginResponseModel loginResponse)
                {
                    _authToken = loginResponse.Token;
                    _tokenExpiry = DateTime.UtcNow.AddHours(1); // Assume 1 hour expiry
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Authentication failed");
            }
            
            return false;
        }

        public async Task<T> RequestMasterAsync<T>(string operation, object requestModel = null, bool isUnauthorized = false) where T : IResponseModel, new()
        {
            try
            {
                // Check if token is expired and refresh if needed
                if (!isUnauthorized && IsTokenExpired())
                {
                    // Handle token refresh logic here if needed
                }

                _httpClient.DefaultRequestHeaders.Clear();
                
                if (!string.IsNullOrEmpty(_authToken) && !isUnauthorized)
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authToken);
                }

                _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                var requestContent = requestModel != null 
                    ? new StringContent(JsonSerializer.Serialize(requestModel), Encoding.UTF8, "application/json")
                    : null;

                var httpResponse = await _httpClient.PostAsync($"{MasterBaseUrl}/api/{operation}", requestContent);

                var responseContent = await httpResponse.Content.ReadAsStringAsync();

                switch (httpResponse.StatusCode)
                {
                    case HttpStatusCode.OK:
                        var successResponse = JsonSerializer.Deserialize<T>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                        return successResponse ?? new T();

                    case HttpStatusCode.BadRequest:
                        var badRequestResponse = new T();
                        badRequestResponse.IsSuccess = false;
                        badRequestResponse.Message = "Bad Request: " + responseContent;
                        return badRequestResponse;

                    case HttpStatusCode.Unauthorized:
                        if (isUnauthorized)
                        {
                            var unauthorizedResponse = new T();
                            unauthorizedResponse.IsSuccess = false;
                            unauthorizedResponse.Message = "Unauthorized";
                            return unauthorizedResponse;
                        }
                        else
                        {
                            // Try to re-authenticate and retry
                            return await RequestMasterAsync<T>(operation, requestModel, true);
                        }

                    case HttpStatusCode.InternalServerError:
                        var serverErrorResponse = new T();
                        serverErrorResponse.IsSuccess = false;
                        serverErrorResponse.Message = "Server Error: " + responseContent;
                        return serverErrorResponse;

                    default:
                        var unknownErrorResponse = new T();
                        unknownErrorResponse.IsSuccess = false;
                        unknownErrorResponse.Message = "Unknown Error: " + responseContent;
                        return unknownErrorResponse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Request to Master API failed for operation: {Operation}", operation);
                var errorResponse = new T();
                errorResponse.IsSuccess = false;
                errorResponse.Message = $"Request failed: {ex.Message}";
                return errorResponse;
            }
        }

        private bool IsTokenExpired()
        {
            return string.IsNullOrEmpty(_authToken) || DateTime.UtcNow >= _tokenExpiry;
        }

        public void ClearAuthentication()
        {
            _authToken = string.Empty;
            _tokenExpiry = DateTime.MinValue;
        }
    }
}
