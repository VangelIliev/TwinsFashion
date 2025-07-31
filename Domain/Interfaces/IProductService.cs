using Domain.Models;

namespace Domain.Interfaces
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductDto>> GetAllProductsAsync();

        public Task<ProductDto> GetProductByIdAsync(Guid id);
    }
}
