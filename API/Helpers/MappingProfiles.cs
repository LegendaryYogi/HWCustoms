using API.Dtos;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()            //ProductToReturnDto destination        44
                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))             //o as an expression for option, s as source where we get the property from that we want to insert into d.ProductBrand field
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());
        }
    }
}