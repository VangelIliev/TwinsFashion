using Data.Models;
using Domain.Models;

namespace Domain.Mappers
{
    public interface IDomainMapper
    {
        public IEnumerable<ProductDto> MapDomainProducts(IEnumerable<Product> products);

        public IEnumerable<SizeDto> MapDomainSizes(IEnumerable<Size> sizes);

        public IEnumerable<ColorDto> MapDomainColors(IEnumerable<Color> colors);

        public IEnumerable<CategoryDto> MapDomainCategories(IEnumerable<Category> categories);

        public IEnumerable<SubCategoryDto> MapDomainSubCategories(IEnumerable<SubCategory> subcategories);

        public ProductDto MapDomainProductDto(Product product);
    }
}
