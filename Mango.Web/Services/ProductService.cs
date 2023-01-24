using Mango.Services.ProductAPI.Models.DTO;
using Mango.Services.ProductAPI.Models.Helpers;
using Mango.Web.Helper;
using Mango.Web.Services.IServices;
using System.Reflection.Metadata;

namespace Mango.Web.Services
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IHttpClientFactory clientFactory;

        public ProductService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            this.clientFactory = clientFactory;
        }
        public async Task<T> CreateProduct<T>(ProductDTO productDTO,string accessToken)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                APIType = Constants.APIType.POST,
                Data = productDTO,
                Url = Constants.ProductAPIBase + "api/products",
                AccessToken = accessToken
                
            });
        }

        public async Task<T> DeleteProduct<T>(int id, string accessToken)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                APIType = Constants.APIType.POST,
                Url = Constants.ProductAPIBase + "api/products/"+id,
                AccessToken = accessToken
            });
        }

        public async Task<T> GetAllProductsAsync<T>(string accessToken)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                APIType = Constants.APIType.GET,
                Url = Constants.ProductAPIBase + "api/products",
                  AccessToken = accessToken
            });
        }

        public async Task<T> GetProductByIdAsync<T>(int id, string accessToken)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                APIType = Constants.APIType.GET,
                Url = Constants.ProductAPIBase + "api/products/" + id,
                  AccessToken = accessToken
            });
        }

        public async Task<T> UpdateProduct<T>(ProductDTO productDTO, string accessToken)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                APIType = Constants.APIType.PUT,
                Data = productDTO,
                Url = Constants.ProductAPIBase + "api/products",
                   AccessToken = accessToken
            });
        }
    }
}
