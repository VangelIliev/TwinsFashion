using AutoMapper;
using Data.Models;
using Domain.Models;

namespace Domain.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Data -> Domain
            CreateMap<Product, ProductDto>();
            CreateMap<Data.Models.Category, Models.CategoryDto>();
            CreateMap<Data.Models.Color, Models.ColorDto>();
            CreateMap<Data.Models.Image, Models.ImageDto>();

            // Domain -> Web Data
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Size))
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color.Name))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(i => i.Url).ToList()));
        }
    }
}
