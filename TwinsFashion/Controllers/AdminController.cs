using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TwinsFashion.Services;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authentication;

namespace TwinsFashion.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IProductService _productService;
        public AdminController(IAdminService adminService, IProductService productService)
        {
            _adminService = adminService;
            _productService = productService;
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                // If already authenticated, redirect to AdminProducts
                return RedirectToAction("AllProducts", "Admin");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(string username, string password)
        {
            if (await _adminService.AuthoriseUser(username, password))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "Admin")
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMonths(1)
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                return RedirectToAction("AllProducts", "Admin");
            }
            ModelState.AddModelError("", "Invalid username or password");
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return View("AdminProducts",products);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveProduct(Guid id)
        {
            await _adminService.RemoveProduct(id);
            return RedirectToAction("AllProducts");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveProductSize(Guid id, string size)
        {
            await _adminService.RemoveProductSize(id, size);
            return RedirectToAction("AllProducts");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("LogIn");
        }
    }
}
