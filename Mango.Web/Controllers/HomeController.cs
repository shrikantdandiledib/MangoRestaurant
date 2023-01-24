using Mango.Services.ProductAPI.Models.DTO;
using Mango.Services.ProductAPI.Models.Helpers;
using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService productService;

        public HomeController(ILogger<HomeController> logger, IProductService productService )
        {
            _logger = logger;
            this.productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDTO> products = new List<ProductDTO>();
            var response = await productService.GetAllProductsAsync<APIResponse<List<ProductDTO>>>("");
            if (response!=null&&response.Success)
            {
                products = response.Data;
            }
            return View(products);
        }
        [Authorize]
        public async Task<IActionResult> Details(int productId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            ProductDTO product = new ProductDTO();
            var response = await productService.GetProductByIdAsync<APIResponse<ProductDTO>>(productId, accessToken);
            if (response != null && response.Success)
            {
                product = response.Data;
            }
            return View(product);
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