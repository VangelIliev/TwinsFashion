using Data.Models;
using Domain.Models;
using Newtonsoft.Json;

namespace TwinsFashion.Models.Mappings
{
    public class ViewMapper : IViewMapper
    {
        public IEnumerable<CategoryViewModel> MapViewModelCategories(IEnumerable<CategoryDto> categories)
        {
            if (categories == null)
                return Enumerable.Empty<CategoryViewModel>();
            return categories.Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name
            });
        }

        public IEnumerable<ColorViewModel> MapViewModelColors(IEnumerable<ColorDto> colors)
        {
            if (colors == null)
                return Enumerable.Empty<ColorViewModel>();
            return colors.Select(c => new ColorViewModel
            {
                Id = c.Id,
                Name = c.Name
            });
        }

        public ProductViewModel MapViewModelProduct(ProductDto product)
        {
            if (product == null)
                return null;
            return new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Category = product.Category?.Name,
                Color = product.Color?.Name,
                ImageUrls = product.Images?.Select(i => i.Url).ToList() ?? new List<string>(),
                Sizes = MapViewModelSizes(product.Sizes)
            };
        }

        public IEnumerable<ProductViewModel> MapViewModelProducts(IEnumerable<ProductDto> products)
        {
            if (products == null) return Enumerable.Empty<ProductViewModel>();

            return products.Select(product => new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Category = product.Category?.Name,
                ImageUrls = product.Images?.Select(i => i.Url).ToList() ?? new List<string>(),
                Sizes = MapViewModelSizes(product.Sizes)
            });
        }

        // Updated to extract 'size' from JSON string in Size.Name
        public ICollection<string> MapViewModelSizes(ICollection<Size> sizes)
        {
            if (sizes == null)
                return Enumerable.Empty<string>().ToList();

            var result = new List<string>();
            foreach (var s in sizes)
            {
                try
                {
                    var sizeObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(s.Name);
                    if (sizeObj != null && sizeObj.ContainsKey("size"))
                    {
                        result.Add(sizeObj["size"]);
                    }
                }
                catch
                {
                    // Optionally log or handle malformed JSON
                }
            }
            return result;
        }

        public ICollection<string> MapViewModelSizes(IEnumerable<SizeDto> sizes)
        {

            if (sizes == null)
                return Enumerable.Empty<string>().ToList();

            var result = new List<string>();
            foreach (var s in sizes)
            {
                try
                {
                    var sizeObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(s.Name);
                    if (sizeObj != null && sizeObj.ContainsKey("size"))
                    {
                        result.Add(sizeObj["size"]);
                    }
                }
                catch
                {
                    // Optionally log or handle malformed JSON
                }
            }
            return result;
        }


        public IEnumerable<SubCategoryViewModel> MapViewModelSubCategories(IEnumerable<SubCategoryDto> subcategories)
        {
            if (subcategories == null)
                return Enumerable.Empty<SubCategoryViewModel>();
            return subcategories.Select(sc => new SubCategoryViewModel
            {
                Id = sc.Id,
                Name = sc.Name
            });
        }
    }
}
