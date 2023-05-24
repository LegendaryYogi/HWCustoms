using Core.Entities;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<T> GetEntityWithSpec(ISpecification<T> spec);
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);       
        Task<int> CountAsync(ISpecification<T> spec);
        void Add(T entity);             //219 plus infos
        void Update(T entity);      //tracking happening in memory so not async
        void Delete(T entity);
    }
}