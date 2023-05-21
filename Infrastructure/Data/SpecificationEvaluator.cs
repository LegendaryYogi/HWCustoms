using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,          //38
        ISpecification<TEntity> spec)
        {
            var query = inputQuery;

            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria); //where id = ProductId                 (((spec.Criteria is replaced like p => p.ProductTypeId == id)))
            }

            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);        //59
            }

            if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);        
            }

            if (spec.IsPagingEnabled)           //paging operator needs to come after any filterig or sorting operators
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));
            
            /* This takes the 2 include statements for product repository and aggregate them and pass it to query which is and IQueryable 
            that we pass to our method that query the database and return with the result based on this IQueryable
            await _context.Products
                .Include(p => p.ProductType)
                .Include(p => p.ProductBrand)
                .FirstOrDefaultAsync(p => p.Id == id); */
        
            return query;
        }
    }
}