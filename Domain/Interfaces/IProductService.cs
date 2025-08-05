using Data.Models;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductDto>> GetAllProductsAsync();

        public Task<ProductDto> GetProductByIdAsync(Guid id);

        Task<bool> SeedProductToDatabase(string categoryName, string colorName, string subcategoryName, List<Size> sizes);

        Task<IEnumerable<ColorDto>> GetColors();

        Task<IEnumerable<CategoryDto>> GetCategories();

        Task<IEnumerable<SizeDto>> GetSizes();

        Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(string categoryName);

        Task<IEnumerable<ProductDto>> GetProductsByColorAsync(string colorName);

        


    }
}
