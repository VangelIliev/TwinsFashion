using Data.Models;
using Domain.Models;

namespace Domain.Mappers
{
    public class DomainMapper : IDomainMapper
    {
        public IEnumerable<CategoryDto> MapDomainCategories(IEnumerable<Category> categories)
        {
            if (categories == null || !categories.Any())
            {
                return Enumerable.Empty<CategoryDto>();
            }
            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            });
        }

        public IEnumerable<ColorDto> MapDomainColors(IEnumerable<Color> colors)
        {
            if (colors == null || !colors.Any())
            {
                return Enumerable.Empty<ColorDto>();
            }
            return colors.Select(c => new ColorDto
            {
                Id = c.Id,
                Name = c.Name
            });
        }

        public IEnumerable<ProductDto> MapDomainProducts(IEnumerable<Product> products)
        {
            if (products == null || !products.Any())
            {
                return Enumerable.Empty<ProductDto>();
            }
            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Quantity = p.Quantity,
                Description = p.Description,
                Category = p.Category,
                SubCategory = p.SubCategory,
                Color = p.Color,
                Images = p.Images?.ToList() ?? new List<Image>(),
                Sizes = p.Sizes?.ToList() ?? new List<Size>()
            });
        }

        public IEnumerable<SizeDto> MapDomainSizes(IEnumerable<Size> sizes)
        {
            if (sizes == null || !sizes.Any())
            {
                return Enumerable.Empty<SizeDto>();
            }
            return sizes.Select(s => new SizeDto
            {
                Id = s.Id,
                Name = s.Name
            });
        }

        public IEnumerable<SubCategoryDto> MapDomainSubCategories(IEnumerable<SubCategory> subcategories)
        {
            if (subcategories == null || !subcategories.Any())
            {
                return Enumerable.Empty<SubCategoryDto>();
            }
            return subcategories.Select(sc => new SubCategoryDto
            {
                Id = sc.Id,
                Name = sc.Name,
                CategoryId = sc.CategoryId,
                Category = sc.Category
            });
        }

        public ProductDto MapDomainProductDto(Product product)
        {
            if (product == null)
            {
                return null;
            }
            var dto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity,
                Description = product.Description,
                Category = product.Category,
                SubCategory = product.SubCategory,
                Color = product.Color,
                Images = product.Images?.ToList() ?? new List<Image>(),
                Sizes = product.Sizes?.ToList() ?? new List<Size>()
            };
            return dto;
        }
    }
}
