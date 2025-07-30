using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TwinsFashion.Models;
using TwinsFashion.Services;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Domain.Interfaces;

namespace TwinsFashion.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IProductService _productService;
        public HomeController(ILogger<HomeController> logger, IEmailSender emailSender, IProductService productService)
        {
            _logger = logger;
            _emailSender = emailSender;
            _productService = productService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async IActionResult Products()
        {
            var model = await _productService.GetAllProductsAsync();
            if (model == null || !model.Any())
            {
                return View("NoProducts");
            }
            var productViewModels = model.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Category = p.Category.Name,
                Color = p.Color.Name,
                Size = p.Size,
                Price = p.Price,
                Description = p.Description
            }).ToList();
            return View(model);
        }

        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Product(string name)
        {
            var product = new ProductViewModel
            {
                Id = 1,
                Name = name,
                Category = "Category1",
                Color = "Red",
                Size = "M",
                Price = 49.99m,
                Description = "This is a sample product description."
            };
            return View("ProductDetails", product);
        }
        public IActionResult Contacts()
        {
            return View();
        }

        public IActionResult ShoppingCart()
        {
            var shoppingBasket = HttpContext.Session.GetString("Basket");
            if (shoppingBasket == null) 
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        

        [HttpPost]
        public async Task<IActionResult> Contacts(EmailContactUsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await  _emailSender.SendContactUsEmail(model);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult AddToCart(ProductViewModel model)
        {
            // Try to get the basket from session
            var basketJson = HttpContext.Session.GetString("Basket");
            ShoppingBasketViewModel shoppingBasketModel;

            if (string.IsNullOrEmpty(basketJson))
            {
                // No basket in session, create new
                shoppingBasketModel = new ShoppingBasketViewModel();
            }
            else
            {
                // Basket exists, deserialize
                shoppingBasketModel = JsonSerializer.Deserialize<ShoppingBasketViewModel>(basketJson);
            }

            // Add product to basket
            shoppingBasketModel.Products.Add(model);

            // Save updated basket to session
            HttpContext.Session.SetString("Basket", JsonSerializer.Serialize(shoppingBasketModel));

            return RedirectToAction("ShoppingCart");
        }
    }
}
