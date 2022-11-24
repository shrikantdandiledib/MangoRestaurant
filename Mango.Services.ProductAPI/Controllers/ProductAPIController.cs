using Mango.Services.ProductAPI.DTO;
using Mango.Services.ProductAPI.Repository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<ServiceResponse<IEnumerable<ProductDTO>>> Get()
        {
            ServiceResponse<IEnumerable<ProductDTO>> serviceResponse = new ServiceResponse<IEnumerable<ProductDTO>>();
            try
            {
                IEnumerable<ProductDTO> products = await productRepository.GetProducts();
                serviceResponse.Data = products;
                serviceResponse.Message = products.Count().ToString() + " Records found.";
            }
            catch (Exception ex)
            {
                serviceResponse.Exception = ex.ToString();
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }

        // GET api/<ProductAPIController>/5
        [HttpGet("{id}")]
        public async Task<ServiceResponse<ProductDTO>> Get(int id)
        {
            ServiceResponse<ProductDTO> serviceResponse = new ServiceResponse<ProductDTO>();
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
                serviceResponse.Exception = ex.ToString();
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }

        // POST api/<ProductAPIController>
        [HttpPost]
        public async Task<ServiceResponse<ProductDTO>> Post([FromBody] ProductDTO productDTO)
        {
            ServiceResponse<ProductDTO> serviceResponse = new ServiceResponse<ProductDTO>();
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
                serviceResponse.Exception = ex.ToString();
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }

       

        // DELETE api/<ProductAPIController>/5
        [HttpDelete("{id}")]
        public async Task<ServiceResponse<bool>> Delete(int id)
        {
            ServiceResponse<bool> serviceResponse = new ServiceResponse<bool>();
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
                serviceResponse.Exception = ex.ToString();
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }
    }
}
