using Mango.Services.Helpers;
using Mango.Services.ProductAPI.Models.DTO;
using Mango.Services.ShoppingCartAPI.Models.DTO;
using Mango.Web.Models;
using Mango.Web.Services;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService productService;
        private readonly ICartService cartService;

        public HomeController(ILogger<HomeController> logger, IProductService productService, ICartService cartService)
        {
            _logger = logger;
            this.productService = productService;
            this.cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            List<Mango.Services.ProductAPI.Models.DTO.ProductDTO> products = new List<Mango.Services.ProductAPI.Models.DTO.ProductDTO>();
            var response = await productService.GetAllProductsAsync<APIResponse<List<Mango.Services.ProductAPI.Models.DTO.ProductDTO>>>("");
            if (response != null && response.Success)
            {
                products = response.Data;
            }
            return View(products);
        }
        [Authorize]
        public async Task<IActionResult> Details(int productId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            Mango.Services.ProductAPI.Models.DTO.ProductDTO product = new Mango.Services.ProductAPI.Models.DTO.ProductDTO();
            var response = await productService.GetProductByIdAsync<APIResponse<Mango.Services.ProductAPI.Models.DTO.ProductDTO>>(productId, accessToken);
            if (response != null && response.Success)
            {
                product = response.Data;
            }
            return View(product);
        }
        [HttpPost]
        [ActionName("Details")]
        [Authorize]
        public async Task<IActionResult> DetailsPost(Mango.Services.ShoppingCartAPI.Models.DTO.ProductDTO productDto)
        {
            CartDTO cartDto = new()
            {
                CartHeader = new CartHeaderDTO
                {
                    UserId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value
                }
            };

            CartDetailsDTO cartDetails = new CartDetailsDTO()
            {
                Count = productDto.Count,
                ProductId = productDto.ProductId
            };

            var resp = await productService.GetProductByIdAsync<APIResponse<Mango.Services.ShoppingCartAPI.Models.DTO.ProductDTO>>(productDto.ProductId, "");
            if (resp != null && resp.Success)
            {
                cartDetails.Product = JsonConvert.DeserializeObject<Mango.Services.ShoppingCartAPI.Models.DTO.ProductDTO>(Convert.ToString(resp.Data));
            }
            List<CartDetailsDTO> cartDetailsDtos = new();
            cartDetailsDtos.Add(cartDetails);
            cartDto.CartDetails = cartDetailsDtos;

            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var addToCartResp = await cartService.AddToCartAsync<APIResponse<Mango.Services.ShoppingCartAPI.Models.DTO.ProductDTO>>(cartDto, accessToken);
            if (addToCartResp != null && addToCartResp.Success)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(productDto);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public async Task<IActionResult> LoginAsync()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }
    }
}