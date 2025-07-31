using AutoMapper;
using Data.Models;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Drawing;

namespace Domain.Implementation
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;

        public ProductService(AppDbContext context, IMapper mapper, ILogger<ProductService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            try
            {
                var dataProducts = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Color)
                .Include(p => p.Images).ToListAsync();
                if (dataProducts == null || !dataProducts.Any())
                {
                    _logger.LogError("No products found in the database.");
                    return Enumerable.Empty<ProductDto>();
                }
                return _mapper.Map<IEnumerable<ProductDto>>(dataProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving products.");
                throw;
            }
        }

        public async Task<ProductDto> GetProductByIdAsync(Guid id)
        {
            try
            {
                var dataProduct = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Color)
                    .Include(p => p.Images)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (dataProduct == null)
                {
                    _logger.LogWarning("Product with ID {Id} not found.", id);
                    return null;
                }
                return _mapper.Map<ProductDto>(dataProduct);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error occurred while retrieving product with ID {Id}.", id);
                throw;
            }
        }

        public async Task<bool> SeedProductToDatabase()
        {
            try
            {
                var images = new List<Image>();
                var guid = Guid.NewGuid();
                images.Add(new Image
                {
                    Id = Guid.NewGuid(),
                    ProductId = guid,
                    Url = ""
                });

                var product = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Панталон Елизабет Франчи",
                    Description = "Продукта е много стилен",
                    Price = 110,
                    Quantity = 10,
                    CategoryId = Guid.Parse("82C46B11-C584-4896-BCE4-DCF3413D3AC6"),
                    ColorId = Guid.Parse("5F45E5B5-97A7-45B2-8400-4CFDA5639C0A"),
                    Size = "M",
                    Images = images
                };

                _context.Products.Add(product);
                _context.SaveChanges();

                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while seeding products.");
                throw;
            }
        }
    }
}
