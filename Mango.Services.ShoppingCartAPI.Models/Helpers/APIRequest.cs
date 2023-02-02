namespace Mango.Services.ShoppingCartAPI.Models.Helpers
{
    public class ApiRequest
    {
        public Constants.APIType APIType { get; set; } = Constants.APIType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
        public string AccessToken { get; set; }
    }
}
