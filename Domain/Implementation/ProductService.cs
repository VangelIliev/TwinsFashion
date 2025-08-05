using Data.Models;
using Domain.Interfaces;
using Domain.Mappers;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Domain.Implementation
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IDomainMapper _mapper;
        private readonly ILogger<ProductService> _logger;

        public ProductService(AppDbContext context, IDomainMapper mapper, ILogger<ProductService> logger)
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
                .Include(p => p.Images)
                .Include(p => p.Sizes)
                .ToListAsync();
                if (dataProducts == null || !dataProducts.Any())
                {
                    _logger.LogError("No products found in the database.");
                    return Enumerable.Empty<ProductDto>();
                }
                return _mapper.MapDomainProducts(dataProducts);
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
                    .Include(p => p.Sizes)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (dataProduct == null)
                {
                    _logger.LogWarning("Product with ID {Id} not found.", id);
                    return null;
                }
                return _mapper.MapDomainProductDto(dataProduct);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error occurred while retrieving product with ID {Id}.", id);
                throw;
            }
        }

        public async Task<IEnumerable<SubCategoryDto>> GetSubCategories()
        {
            try
            {
                var subCategories = await _context.SubCategories.ToListAsync();
                if (subCategories == null || !subCategories.Any())
                {
                    _logger.LogWarning("No subcategories found in the database.");
                    return [];
                }
                return _mapper.MapDomainSubCategories(subCategories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving subcategories.");
                throw;
            }
        }

        public async Task<IEnumerable<ColorDto>> GetColors()
        {
            try
            {
                var colors = await _context.Colors.ToListAsync();
                if (colors == null || !colors.Any())
                {
                    _logger.LogWarning("No colors found in the database.");
                    return [];
                }
                return _mapper.MapDomainColors(colors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving colors.");
                throw;
            }
        }
        public async Task<IEnumerable<CategoryDto>> GetCategories()
        {
            try
            {
                var categories = await _context.Categories.ToListAsync();
                if (categories == null || !categories.Any())
                {
                    _logger.LogWarning("No categories found in the database.");
                    return [];
                }
                return _mapper.MapDomainCategories(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving categories.");
                throw;
            }
        }

        public async Task<IEnumerable<SizeDto>> GetSizes()
        {
            try
            {
                var sizes = await _context.Sizes.ToListAsync();
                if (sizes == null || !sizes.Any())
                {
                    _logger.LogWarning("No sizes found in the database.");
                    return [];
                }
                return _mapper.MapDomainSizes(sizes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving sizes.");
                throw;
            }
        }
        public async Task<bool> SeedProductToDatabase(string categoryName, string colorName, string subcategoryName, List<Size> sizes)
        {
            try
            {
                // 1. Get related entities
                var category = await _context.Categories.FirstAsync(x => x.Name == categoryName);
                var color = await _context.Colors.FirstAsync(x => x.Name == colorName);
                var subCategory = await _context.SubCategories.FirstAsync(x => x.Name == subcategoryName);

                // 2. Create product and images
                var productId = Guid.NewGuid();
                var images = new List<Image>
                {
                    new Image
                    {
                        Id = Guid.NewGuid(),
                        ProductId = productId,
                        Url = "/images/pants/Elizabeth_Franchie_Pants.jpg"
                    }
                };

                var product = new Product
                {
                    Id = productId,
                    Name = "Панталон Елизабетa Франчи",
                    Description = "Летен панталон от памук и еластан",
                    Price = 110,
                    Quantity = 10,
                    CategoryId = category.Id,
                    Category = category,
                    ColorId = color.Id,
                    Color = color,
                    SubCategoryId = subCategory.Id,
                    SubCategory = subCategory,
                    Images = images,
                    Sizes = sizes
                };

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while seeding products.");
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(string categoryName)
        {
            try
            {
                var products = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Color)
                    .Include(p => p.Images)
                    .Where(p => p.Category.Name == categoryName)
                    .ToListAsync();
                if (products == null || !products.Any())
                {
                    _logger.LogWarning("No products found for category {CategoryName}.", categoryName);
                    return Enumerable.Empty<ProductDto>();
                }
                return _mapper.MapDomainProducts(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving products by category {CategoryName}.", categoryName);
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByColorAsync(string colorName)
        {
            try
            {
                var products = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Color)
                    .Include(p => p.Images)
                    .Where(p => p.Color.Name == colorName)
                    .ToListAsync();
                if (products == null || products.Count == 0)
                {
                    _logger.LogWarning("No products found for color {ColorName}.", colorName);
                    return Enumerable.Empty<ProductDto>();
                }
                return _mapper.MapDomainProducts(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving products by color {ColorName}.", colorName);
                throw;
            }
        }
    }
}
