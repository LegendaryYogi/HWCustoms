using System.Linq.Expressions;

namespace Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification()
        {
        }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        public Expression<Func<T, bool>> Criteria {get; }

        public List<Expression<Func<T, object>>> Includes {get; } = 
            new List<Expression<Func<T, object>>>();

        public Expression<Func<T, object>> OrderBy {get; private set;}   //why private set? adding set what orderby is int this particular class 59

        public Expression<Func<T, object>> OrderByDescending {get; private set;}

        public int Take {get; private set;}  //adding pagination 63

        public int Skip {get; private set;} 

        public bool IsPagingEnabled {get; private set;} 

        protected void AddInclude(Expression<Func<T, object>> includeExpression)  //create a method that allow us to add include statements to our include list (list of expressions) 36
        {
            Includes.Add(includeExpression);
        }

        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)        //59
        {
            OrderBy = orderByExpression;
        }

        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)        
        {
            OrderByDescending = orderByDescExpression;
        }

        protected void ApplyPaging(int skip, int take)      //adding pagination 63
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }
    }
}