using Microsoft.AspNetCore.Mvc;
using ProductWebView.Clients.Interfaces;
using ProductWebView.Models;

namespace ProductWebView.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductApiClient _productApiClient;

        public ProductsController(IProductApiClient productApiClient)
        {
            _productApiClient = productApiClient;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productApiClient.GetAllProductsAsync();
            return View("~/Views/Home/Index.cshtml", products);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var product = await _productApiClient.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View("~/Views/Actions/Details.cshtml", product);
        }

        public IActionResult Create()
        {
            return View("~/Views/Actions/Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                await _productApiClient.AddProductAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/Actions/Create.cshtml", product);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var product = await _productApiClient.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View("~/Views/Actions/Edit.cshtml", product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _productApiClient.UpdateProductAsync(id, product);
                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/Actions/Edit.cshtml", product);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await _productApiClient.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View("~/Views/Actions/Delete.cshtml", product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _productApiClient.DeleteProductAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
