using AutoMapper;
using Data.Models;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;
using TwinsFashion.Models;
using TwinsFashion.Services;

namespace TwinsFashion.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        public HomeController(
            ILogger<HomeController> logger, 
            IEmailSender emailSender, 
            IProductService productService, 
            IMapper mapper)
        {
            _logger = logger;
            _emailSender = emailSender;
            _productService = productService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            _productService.SeedProductToDatabase(
                "Clothes",
                "Black",
                "T-Shirts",
                new List<Size>
                {
                    new Size { Name = "S" },
                    new Size { Name = "M" },
                    new Size { Name = "L" },
                    new Size { Name = "XL" }
                }).GetAwaiter().GetResult();
            return View();
        }
        public async Task<IActionResult> Products()
        {
            var model = await _productService.GetAllProductsAsync();
            if (model == null || !model.Any())
            {
                return View("NoProducts");
            }
            var productsViewmodel = _mapper.Map<IEnumerable<ProductViewModel>>(model);
            return View(model);
        }

        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Product(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Product with ID {Id} not found.", id);
                return NotFound();
            }

            var productViewModel = _mapper.Map<ProductViewModel>(product);
            return View("ProductDetails", productViewModel);
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
