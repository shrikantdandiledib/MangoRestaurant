using Mango.Services.Helpers;

namespace Mango.Web.Services.IServices
{
    public interface IBaseService : IDisposable
    {
        APIResponse<object> response { get; set; }
        Task<T> SendAsync<T>(ApiRequest apiRequest);
    }
}
