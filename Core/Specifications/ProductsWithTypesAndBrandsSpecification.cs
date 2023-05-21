using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>   //get a meaningful name 39-40
    {
        public ProductsWithTypesAndBrandsSpecification(ProductSpecParams productParams)
            : base(x => 
                (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
                (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) &&
                (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId)
            )
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
            AddOrderBy(x => x.Name);
            ApplyPaging(productParams.PageSize * (productParams.PageIndex -1), 
            productParams.PageSize);  //we are on the page 1 and multiplying pagesize as 5 so 5*1 and this is skip operator so we skip the fist 5 items so we need -1

            if (!string.IsNullOrEmpty(productParams.Sort))  //checked value of sort so we will apply ordering properly 60
            {
                switch (productParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(n => n.Name);
                        break;
                }
            }
        }

        public ProductsWithTypesAndBrandsSpecification(int id) 
            : base(x => x.Id == id)         // replacing expression in base : public BaseSpecification(Expression<Func<T, bool>> criteria)
        {                                   //get me the product that matches the product id with the given id
            AddInclude(x => x.ProductType);     //also include the product type and brand
            AddInclude(x => x.ProductBrand);
        }
    }
}