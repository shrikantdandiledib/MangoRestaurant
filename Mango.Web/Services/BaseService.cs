using Mango.Services.ProductAPI.Models.Helpers;
using Mango.Web.Services.IServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using static Mango.Web.Helper.Constants;

namespace Mango.Web.Services
{
    public class BaseService : IBaseService
    {
        public APIResponse<object> response { get; set; }
        public IHttpClientFactory httpClient { get; set; }
        public BaseService(IHttpClientFactory httpClient)
        {
            this.response = new APIResponse<object>();
            this.httpClient = httpClient;
        }

        public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var client = httpClient.CreateClient("MangoAPI");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);
                client.DefaultRequestHeaders.Clear();
                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");
                }
                if (!string.IsNullOrEmpty(apiRequest.AccessToken))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.AccessToken);
                }


                HttpResponseMessage responseMessage = null;
                switch (apiRequest.APIType)
                {
                    case APIType.GET:
                        message.Method = HttpMethod.Get;
                        break;
                    case APIType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case APIType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case APIType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }
                responseMessage = await client.SendAsync(message);
                var apiContent = await responseMessage.Content.ReadAsStringAsync();
                var isValidJson = IsValidJson(apiContent);
                if (isValidJson)
                {
                    var apiResponseDto = JsonConvert.DeserializeObject<T>(apiContent);
                    return apiResponseDto;
                }
                return CreateExceptionObject<T>(apiContent, "Exception Found", new List<string> { apiContent }, false);

            }
            catch (Exception e)
            {
                var dto = new APIResponse<object>
                {
                    Data = e.ToString(),
                    Message = e.Message,
                    Exception = new List<string> { Convert.ToString(e.Message) },
                    Success = false,
                };
                return CreateExceptionObject<T>(dto, e.Message, new List<string> { Convert.ToString(e.Message) }, false);
            }
        }

        private static T CreateExceptionObject<T>(object apiContent, string message, List<string> exception, bool status)
        {
            var dto = new APIResponse<object>
            {
                Data = apiContent,
                Message = "Exception Found",
                Exception = exception,
                Success = status,
            };
            var response = JsonConvert.SerializeObject(dto);
            var apiResponseDto = JsonConvert.DeserializeObject<T>(response);
            return apiResponseDto;
        }

        private static bool IsValidJson(string strInput)
        {
            if (string.IsNullOrWhiteSpace(strInput)) { return false; }
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

    }
}
