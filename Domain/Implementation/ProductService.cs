using Domain.Interfaces;
using Domain.Models;
using Data.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
                    _logger.LogWarning("No products found in the database.");
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
    }
}
