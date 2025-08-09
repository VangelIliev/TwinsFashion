using Data.Models;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TwinsFashion.Models;
using TwinsFashion.Models.Mappings;
using TwinsFashion.Services;

namespace TwinsFashion.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IProductService _productService;
        private readonly IViewMapper _mapper;
        public HomeController(
            ILogger<HomeController> logger,
            IEmailSender emailSender,
            IProductService productService,
            IViewMapper mapper)
        {
            _logger = logger;
            _emailSender = emailSender;
            _productService = productService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            ViewData["basketQuantity"] = GetShoppingBasketCount();
            return View();
        }
        public async Task<IActionResult> Products()
        {
            ViewData["basketQuantity"] = GetShoppingBasketCount();
            var model = await _productService.GetAllProductsAsync();
            var categoriesDto = await _productService.GetCategories();
            var categories = categoriesDto.Select(x => x.Name);
            ViewData["Categories"] = categories;
            if (model == null || !model.Any())
            {
                return View("NoProducts");
            }
            var productsViewmodel = _mapper.MapViewModelProducts(model);
            return View(productsViewmodel);
        }

        public IActionResult About()
        {
            ViewData["basketQuantity"] = GetShoppingBasketCount();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Product(Guid id)
        {
            ViewData["basketQuantity"] = GetShoppingBasketCount();
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Product with ID {Id} not found.", id);
                return NotFound();
            }

            var productViewModel = _mapper.MapViewModelProduct(product);
            return View("ProductDetails", productViewModel);
        }
        public IActionResult Contacts()
        {
            ViewData["basketQuantity"] = GetShoppingBasketCount();
            return View();
        }

        public IActionResult ShoppingCart()
        {
            var shoppingBasket = HttpContext.Session.GetString("Basket");
            ViewData["basketQuantity"] = GetShoppingBasketCount();
            if (string.IsNullOrEmpty(shoppingBasket))
            {
                return View(new ShoppingBasketViewModel());
            }
            var shoppingBasketModel = JsonConvert.DeserializeObject<ShoppingBasketViewModel>(shoppingBasket);
            return View(shoppingBasketModel);
        }

        [HttpPost]
        public async Task<IActionResult> Contacts(EmailContactUsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _emailSender.SendContactUsEmail(model);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(Guid id, string size, int quantity)
        {
            if (id == Guid.Empty || string.IsNullOrEmpty(size) || quantity <= 0)
            {
                return BadRequest("Invalid product data.");
            }

            var productDto = await _productService.GetProductByIdAsync(id);
            if (productDto == null)
            {
                return NotFound("Product not found.");
            }

            var model = _mapper.MapViewModelProduct(productDto);
            model.Size = size;
            model.Quantity = quantity;

            var basketJson = HttpContext.Session.GetString("Basket");
            ShoppingBasketViewModel shoppingBasketModel;

            if (string.IsNullOrEmpty(basketJson))
            {
                shoppingBasketModel = new ShoppingBasketViewModel();
            }
            else
            {
                shoppingBasketModel = JsonConvert.DeserializeObject<ShoppingBasketViewModel>(basketJson);
            }

            if (!shoppingBasketModel.Products.TryGetValue(model.Id, out var productList))
            {
                // Product is not in the cart yet, add it as a new list
                shoppingBasketModel.Products[model.Id] = new List<ProductViewModel> { model };
            }
            else
            {
                // Product is in the cart, check if the same size exists
                var existingProduct = productList.FirstOrDefault(p => p.Size == model.Size);
                if (existingProduct != null)
                {
                    // Same size exists, just increase the quantity
                    existingProduct.Quantity += model.Quantity;
                }
                else
                {
                    // Different size, add it as a new item in the list
                    productList.Add(model);
                }
            }

            HttpContext.Session.SetString("Basket", JsonConvert.SerializeObject(shoppingBasketModel));
            
            var newBasketQuantity = GetShoppingBasketCount();
            return Ok(new { success = true, newQuantity = newBasketQuantity });
        }

        [HttpPost]
        public IActionResult RemoveFromCart(Guid id, string size)
        {
            var basketJson = HttpContext.Session.GetString("Basket");
            if (string.IsNullOrEmpty(basketJson))
                return RedirectToAction("ShoppingCart");

            var shoppingBasketModel = JsonConvert.DeserializeObject<ShoppingBasketViewModel>(basketJson);

            if (shoppingBasketModel.Products.TryGetValue(id, out var productList))
            {
                var productToRemove = productList.FirstOrDefault(p => p.Size == size);
                if (productToRemove != null)
                {
                    productList.Remove(productToRemove);
                    if (!productList.Any())
                        shoppingBasketModel.Products.Remove(id);
                }
            }

            HttpContext.Session.SetString("Basket", JsonConvert.SerializeObject(shoppingBasketModel));
            return RedirectToAction("Index", "Home");
        }

        private int GetShoppingBasketCount()
        {
            var shoppingBasket = HttpContext.Session.GetString("Basket");
            int basketQuantity = 0;
            if (!string.IsNullOrEmpty(shoppingBasket))
            {
                var shoppingBasketModel = JsonConvert.DeserializeObject<ShoppingBasketViewModel>(shoppingBasket);
                basketQuantity = shoppingBasketModel.Products?.Sum(pg => pg.Value.Sum(p => p.Quantity)) ?? 0;
            }        
            return basketQuantity;
        }

        [HttpGet]
        public async Task<IActionResult> ProductDetails(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();
            var result = _mapper.MapViewModelProduct(product);
            return Ok(result);
        }
    }
}
