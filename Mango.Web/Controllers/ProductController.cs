using Mango.Services.ProductAPI.Models.DTO;
using Mango.Services.ProductAPI.Models.Helpers;
using Mango.Web.Services;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }
        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDTO> products = new();
            var response = await productService.GetAllProductsAsync<APIResponse<List<ProductDTO>>>();
            if (response != null && response.Success)
            {
                products = response.Data;
            }
            return View(products);
        }
        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductCreate(ProductDTO model)
        {
            if (ModelState.IsValid)
            {
                //var accessToken = await HttpContext.GetTokenAsync("access_token");
                var response = await productService.CreateProduct<APIResponse<ProductDTO>>(model);
                if (response != null && response.Success)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }
            return View(model);
        }
        public async Task<IActionResult> ProductEdit(int productId)
        {
            //var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await productService.GetProductByIdAsync<APIResponse<ProductDTO>>(productId);
            if (response != null && response.Success)
            {
                ProductDTO model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Data));
                return View(model);
            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductEdit(ProductDTO model)
        {
            if (ModelState.IsValid)
            {
                //  var accessToken = await HttpContext.GetTokenAsync("access_token");
                var response = await productService.UpdateProduct<APIResponse<ProductDTO>>(model);
                if (response != null && response.Success)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }
            return View(model);
        }

        //  [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ProductDelete(int productId)
        {
            //  var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await productService.GetProductByIdAsync<APIResponse<ProductDTO>>(productId);
            if (response != null && response.Success)
            {
                ProductDTO model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Data));
                return View(model);
            }
            return NotFound();
        }
        [HttpPost]
        // [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductDelete(ProductDTO model)
        {
            if (ModelState.IsValid)
            {
                //var accessToken = await HttpContext.GetTokenAsync("access_token");
                var response = await productService.DeleteProduct<APIResponse<bool>>(model.ProductId);
                if (response.Success)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }
            return View(model);
        }
    }
}
