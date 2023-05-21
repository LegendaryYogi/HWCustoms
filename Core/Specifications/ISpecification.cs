using System.Linq.Expressions;

namespace Core.Specifications
{
    public interface ISpecification<T>      //creating generic methods
    {
        Expression<Func<T, bool>> Criteria {get; }
        List<Expression<Func<T, object>>> Includes {get; }
        Expression<Func<T, object>> OrderBy {get; }             //add support for to additional expressions so add an expression of generic variety and pass in a func that takes a T type and return an object we call it OrderBy 59
        Expression<Func<T, object>> OrderByDescending {get; }
        int Take {get; }        //adding pagination 63
        int Skip {get; }
        bool IsPagingEnabled {get; }
    }
}