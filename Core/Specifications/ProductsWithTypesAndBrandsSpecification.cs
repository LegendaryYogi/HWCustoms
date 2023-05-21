using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>   //get a meaningful name 39-40
    {
        public ProductsWithTypesAndBrandsSpecification()
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
        }

        public ProductsWithTypesAndBrandsSpecification(int id) 
            : base(x => x.Id == id)         // replacing expression in base : public BaseSpecification(Expression<Func<T, bool>> criteria)
        {                                   //get me the product that matches the product id with the given id
            AddInclude(x => x.ProductType);     //also include the product type and brand
            AddInclude(x => x.ProductBrand);
        }
    }
}