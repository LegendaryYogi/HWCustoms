using API.Dtos;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class ProductUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        public readonly IConfiguration _config;
        public ProductUrlResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if(!String.IsNullOrEmpty(source.PictureUrl))   //actually pictureurl cannot be null as we configured before
            {
                return _config["ApiUrl"] + source.PictureUrl;       //ApiUrl from appsettings.Dev.json    it gives the full path of the image
            }

            return null;
        }
    }
}