using Microsoft.IdentityModel.Logging;
using Minerets.Shop.Models;
using MyShop_Site.Models.Authentication;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MyShop_Site.Services
{
    public class MasterService
    {
        private static readonly MasterService instance = new MasterService();
        public static  MasterService Instance => Instance;

        public string MasterBaseUrl = "https://dev.minerets.com/ShopMaster";
        public async Task<bool> AuthMasterUser(string userName, string password)
        {
            try
            {
                string token = null;
                using HttpClient httpClient = new HttpClient();
                using HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(
                    $"{MasterBaseUrl}/api/Authentication/Authenticate",
                    JsonContent.Create(new { userName, password }));
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    token = await httpResponseMessage.Content.ReadAsStringAsync();
                }

                if (token != null)
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    using HttpResponseMessage userResponseMessage = await httpClient.PostAsync($"{MasterBaseUrl}/api/User/GetUser", null);

                    if (userResponseMessage.IsSuccessStatusCode)
                    {
                        UserInfoResponseModel userInfoResponseModel = await userResponseMessage.Content.ReadFromJsonAsync<UserInfoResponseModel>();
                        //Save User ID, userName, password 
                        //Save User Token 
                    }
                    else
                    {
                        string error = await userResponseMessage.Content.ReadAsStringAsync();
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
            }
            return false;
        }


        public async Task<IResponseModel> RequestMaster<T>(string Operation, object requestModel = null, bool isUnauthorized = false) where T : IResponseModel
        {
            IResponseModel responseModel;

            try
            {
                string masterToken = ""; // Get Token 

                using HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", masterToken);

                httpClient.DefaultRequestHeaders.Add("MasterToken", masterToken);

                using HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(
                    $"{MasterBaseUrl}/api/{Operation}",
                    requestModel == null ? null : new StringContent(JsonSerializer.Serialize(requestModel, requestModel.GetType()), Encoding.UTF8, "application/json"));

                switch (httpResponseMessage.StatusCode)
                {
                    case HttpStatusCode.OK:
                        switch (typeof(T).Name)
                        {
                            case nameof(EmptyResponseModel):
                                responseModel = new EmptyResponseModel();
                                break;
                            default:
                                responseModel = JsonSerializer.Deserialize<T>(await httpResponseMessage.Content.ReadAsStringAsync());
                                break;
                        }
                        break;
                    case HttpStatusCode.BadRequest:
                        responseModel = new FailedResponseModel()
                        {
                            ErrorCode = await httpResponseMessage.Content.ReadAsStringAsync(),
                            ErrorDetail = await httpResponseMessage.Content.ReadAsStringAsync()

                        };
                        break;
                    case HttpStatusCode.Unauthorized:
                        if (isUnauthorized)
                        {
                            responseModel = responseModel = new FailedResponseModel()
                            {
                                ErrorCode = "Unauthorized"
                            };
                        }
                        else
                        {
                            //bool isAuth = await AuthAccountUser();
                            responseModel = await RequestMaster<T>(Operation, requestModel, true);
                        }
                        break;
                    case HttpStatusCode.InternalServerError:
                        responseModel = new FailedResponseModel()
                        {
                            ErrorCode = "ServerError",
                            ErrorDetail = await httpResponseMessage.Content.ReadAsStringAsync()
                        };
                        break;
                    default:
                        responseModel = new FailedResponseModel()
                        {
                            ErrorCode = "UnknownError",
                            ErrorDetail = await httpResponseMessage.Content.ReadAsStringAsync()
                        };
                        break;
                }
            }
            catch (Exception ex)
            {
                responseModel = new FailedResponseModel()
                {
                    ErrorCode = "UnknownError",
                    ErrorDetail = ex.Message
                };

            }
            return responseModel;
        }


    }
}
