using AutoMapper;
using Mango.Services.ShoppingCart.DbContexts;
using Mango.Services.ShoppingCart.IServices;
using Mango.Services.ShoppingCartAPI.Models.DTO;
using Mango.Services.ShoppingCartAPI.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCart.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContexts dbContext;
        private IMapper _mapper;

        public CartService(ApplicationDbContexts db, IMapper mapper)
        {
            this.dbContext = db;
            _mapper = mapper;
        }

        public async Task<bool> ClearCart(string userId)
        {
            var cartHeaderFromDb = await dbContext.CartHeaders.FirstOrDefaultAsync(u => u.UserId == userId);
            if (cartHeaderFromDb != null)
            {
                dbContext.CartDetails
                    .RemoveRange(dbContext.CartDetails.Where(u => u.CartHeaderId == cartHeaderFromDb.CartHeaderId));
                dbContext.CartHeaders.Remove(cartHeaderFromDb);
                await dbContext.SaveChangesAsync();
                return true;

            }
            return false;
        }

        public async Task<CartDTO> CreateUpdateCart(CartDTO cartDto)
        {
            Cart cart = _mapper.Map<Cart>(cartDto);

            //check if product exists in database, if not create it!
            var prodInDb = await dbContext.Products
                .FirstOrDefaultAsync(u => u.ProductId == cartDto.CartDetails.FirstOrDefault()
                .ProductId);
            if (prodInDb == null)
            {
                dbContext.Products.Add(cart.CartDetails.FirstOrDefault().Product);
                await dbContext.SaveChangesAsync();
            }


            //check if header is null
            var cartHeaderFromDb = await dbContext.CartHeaders.AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == cart.CartHeader.UserId);

            if (cartHeaderFromDb == null)
            {
                //create header and details
                dbContext.CartHeaders.Add(cart.CartHeader);
                await dbContext.SaveChangesAsync();
                cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.CartHeaderId;
                cart.CartDetails.FirstOrDefault().Product = null;
                dbContext.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                await dbContext.SaveChangesAsync();
            }
            else
            {
                //if header is not null
                //check if details has same product
                var cartDetailsFromDb = await dbContext.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                    u => u.ProductId == cart.CartDetails.FirstOrDefault().ProductId &&
                    u.CartHeaderId == cartHeaderFromDb.CartHeaderId);

                if (cartDetailsFromDb == null)
                {
                    //create details
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                    cart.CartDetails.FirstOrDefault().Product = null;
                    dbContext.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    //update the count / cart details
                    cart.CartDetails.FirstOrDefault().Product = null;
                    cart.CartDetails.FirstOrDefault().Count += cartDetailsFromDb.Count;
                    cart.CartDetails.FirstOrDefault().CartDetailsId = cartDetailsFromDb.CartDetailsId;
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                    dbContext.CartDetails.Update(cart.CartDetails.FirstOrDefault());
                    await dbContext.SaveChangesAsync();
                }
            }

            return _mapper.Map<CartDTO>(cart);
        }

        public async Task<CartDTO> GetCartByUserId(string userId)
        {
            Cart cart = new()
            {
                CartHeader = await dbContext.CartHeaders.FirstOrDefaultAsync(u => u.UserId == userId)
            };

            cart.CartDetails = dbContext.CartDetails
                .Where(u => u.CartHeaderId == cart.CartHeader.CartHeaderId).Include(u => u.Product);

            return _mapper.Map<CartDTO>(cart);
        }

        public async Task<bool> RemoveFromCart(int cartDetailsId)
        {
            try
            {
                CartDetails cartDetails = await dbContext.CartDetails
                    .FirstOrDefaultAsync(u => u.CartDetailsId == cartDetailsId);

                int totalCountOfCartItems = dbContext.CartDetails
                    .Where(u => u.CartHeaderId == cartDetails.CartHeaderId).Count();

                dbContext.CartDetails.Remove(cartDetails);
                if (totalCountOfCartItems == 1)
                {
                    var cartHeaderToRemove = await dbContext.CartHeaders
                        .FirstOrDefaultAsync(u => u.CartHeaderId == cartDetails.CartHeaderId);

                    dbContext.CartHeaders.Remove(cartHeaderToRemove);
                }
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
