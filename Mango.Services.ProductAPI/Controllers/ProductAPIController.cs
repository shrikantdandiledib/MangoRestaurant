
using Mango.Services.ProductAPI.Helpers;
using Mango.Services.ProductAPI.Models.DTO;
using Mango.Services.ProductAPI.Models.Helpers;
using Mango.Services.ProductAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        private readonly IProductRepository productRepository;

        public ProductAPIController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        // GET: api/<ProductAPIController>
        [HttpGet]
        public async Task<APIResponse<IEnumerable<ProductDTO>>> Get()
        {
            APIResponse<IEnumerable<ProductDTO>> serviceResponse = new APIResponse<IEnumerable<ProductDTO>>();
            try
            {
                IEnumerable<ProductDTO> products = await productRepository.GetProducts();
                serviceResponse.Data = products;
                serviceResponse.Message = products.Count().ToString() + " Records found.";
            }
            catch (Exception ex)
            {
                serviceResponse.Exception = new List<string> { Convert.ToString(ex.Message) };
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }

        // GET api/<ProductAPIController>/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<APIResponse<ProductDTO>> Get(int id)
        {
            APIResponse<ProductDTO> serviceResponse = new APIResponse<ProductDTO>();
            try
            {
                ProductDTO products = await productRepository.GetProduct(id);
                if (products != null)
                {
                    serviceResponse.Data = products;
                    serviceResponse.Message = "Record found.";
                }
                else
                {
                    serviceResponse.Data = null;
                    serviceResponse.Message = "Record does not found.";
                }

            }
            catch (Exception ex)
            {
                serviceResponse.Exception = new List<string> { Convert.ToString(ex.Message) };
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }

        // POST api/<ProductAPIController>
        [Authorize]
        [HttpPost]
        public async Task<APIResponse<ProductDTO>> Post([FromBody] ProductDTO productDTO)
        {
            APIResponse<ProductDTO> serviceResponse = new APIResponse<ProductDTO>();
            try
            {
                ProductDTO products = await productRepository.CreateUpdateProduct(productDTO);
                if (products != null)
                {
                    serviceResponse.Data = products;
                    serviceResponse.Message = "Record Created.";
                }
                else
                {
                    serviceResponse.Data = null;
                    serviceResponse.Message = "Something went wrong!!!";
                }

            }
            catch (Exception ex)
            {
                serviceResponse.Exception = new List<string> { Convert.ToString(ex.Message) };
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }



        // DELETE api/<ProductAPIController>/5
        [Authorize(Roles =ClientRoles.Admin)]
        [HttpDelete("{id}")]
        public async Task<APIResponse<bool>> Delete(int id)
        {
            APIResponse<bool> serviceResponse = new APIResponse<bool>();
            try
            {
                var products = await productRepository.DeleteProduct(id);
                if (products)
                {
                    serviceResponse.Data = products;
                    serviceResponse.Message = "Record Deleted.";
                }
                else
                {
                    serviceResponse.Data = products;
                    serviceResponse.Message = "Something went wrong!!!";
                }

            }
            catch (Exception ex)
            {
                serviceResponse.Exception = new List<string> { Convert.ToString(ex.Message) };
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }
    }
}
