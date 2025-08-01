using AutoMapper;
using Data.Models;
using Domain.Models;

namespace TwinsFashion.Models.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Data.Models.Category, Domain.Models.CategoryDto>();
            CreateMap<Data.Models.Color, Domain.Models.ColorDto>();
            CreateMap<Data.Models.Image, Domain.Models.ImageDto>();

            CreateMap<Domain.Models.CategoryDto, CategoryViewModel>();
            CreateMap<Domain.Models.ColorDto, ColorViewModel>();

            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color.Name))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(i => i.Url).ToList()));

            // Domain -> Web Data
            CreateMap<ProductDto, ProductViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color.Name))
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.Images.Select(i => i.Url).ToList()));
        }
    }
}
