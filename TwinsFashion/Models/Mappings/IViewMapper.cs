using Data.Models;
using Domain.Models;

namespace TwinsFashion.Models.Mappings
{
    public interface IViewMapper
    {
        public IEnumerable<ProductViewModel> MapViewModelProducts(IEnumerable<ProductDto> products);

        public IEnumerable<SizeViewModel> MapViewModelSizes(IEnumerable<SizeDto> sizes);

        public IEnumerable<ColorViewModel> MapViewModelColors(IEnumerable<ColorDto> colors);

        public IEnumerable<CategoryViewModel> MapViewModelCategories(IEnumerable<CategoryDto> categories);

        public IEnumerable<SubCategoryViewModel> MapViewModelSubCategories(IEnumerable<SubCategoryDto> subcategories);

        public ProductViewModel MapViewModelProduct(ProductDto product);
    }
}
