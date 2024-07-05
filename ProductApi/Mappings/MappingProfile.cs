using AutoMapper;
using ProductApi.DTOs;
using ProductApi.Models;

namespace ProductApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, ProductCreateUpdateDto>().ReverseMap();
        }
    }
}
