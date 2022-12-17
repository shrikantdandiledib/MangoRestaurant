using AutoMapper;
using Mango.Services.ProductAPI.DbContexts;
using Mango.Services.ProductAPI.Models.DTO;
using Mango.Services.ProductAPI.Models.Models;
using Mango.Services.ProductAPI.Repository;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.Models.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContexts _db;
        private IMapper _mapper;
        public ProductRepository(ApplicationDbContexts db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<ProductDTO> CreateUpdateProduct(ProductDTO product)
        {
            Product product1= _mapper.Map<ProductDTO,Product>(product);
            if (product1?.ProductId>0)
            {
                _db.Products.Update(product1);
            }
            else
            {
                _db.Products.Add(product1);
            }
            _db.SaveChangesAsync();
            return _mapper.Map<Product, ProductDTO>(product1);
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            try
            {
                Product product = await _db.Products.FirstOrDefaultAsync(x=>x.ProductId==productId);
                if (product==null)
                {
                    return false;
                }
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<ProductDTO> GetProduct(int productId)
        {
            Product products = await _db.Products.FirstOrDefaultAsync(x => x.ProductId == productId);
            return _mapper.Map<ProductDTO>(products);
        }

        public async Task<IEnumerable<ProductDTO>> GetProducts()
        {
            IEnumerable<Product> products = await _db.Products.ToListAsync();
            return _mapper.Map<List<ProductDTO>>(products);
        }
    }
}
