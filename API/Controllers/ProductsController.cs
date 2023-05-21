using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        // private readonly IProductRepository _repo;               implementing generic repo 34

        /*   public ProductsController(IProductRepository repo)
          {
              _repo = repo;

          } */
        private readonly IGenericRepository<Product> _productsRepo;                 //replacing 1 dependency with 3
        public readonly IGenericRepository<ProductBrand> _productBrandRepo;
        public readonly IGenericRepository<ProductType> _productTypeRepo;
        public readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productsRepo,
        IGenericRepository<ProductBrand> productBrandRepo, IGenericRepository<ProductType> productTypeRepo, IMapper mapper)
        {
            _mapper = mapper;
            _productTypeRepo = productTypeRepo;
            _productBrandRepo = productBrandRepo;
            _productsRepo = productsRepo;

        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification();

            var products = await _productsRepo.ListAsync(spec);

            return Ok(_mapper
                .Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
            
            /* products.Select(product => new ProductToReturnDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                PictureUrl = product.PictureUrl,
                ProductBrand = product.ProductBrand.Name,
                ProductType = product.ProductType.Name
            }).ToList();        //products are in memory so we select them in memory and turning them into a list.  42 */
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]                     //creates swagger responses 55
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]   //creates swagger responses
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);

            //        return await _productsRepo.GetEntityWithSpec(spec);           go to the products repo and get the entity with spec specification and we passing spec
            //                                                              along with expressions from ProductsWithTypesAndBrandsSpecification to our generic repository
            var product = await _productsRepo.GetEntityWithSpec(spec);

            if (product == null) return NotFound(new ApiResponse(404));  //check if we get a product id that is null drop not found error

            return _mapper.Map<Product, ProductToReturnDto>(product);

            /* return new ProductToReturnDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,           changed to automapper
                PictureUrl = product.PictureUrl,
                ProductBrand = product.ProductBrand.Name,
                ProductType = product.ProductType.Name
            }; */

        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()             //29  34
        {
            return Ok(await _productBrandRepo.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _productTypeRepo.ListAllAsync());
        }
    }
}