using Domain.Models;

namespace Domain.Interfaces
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    }
}
