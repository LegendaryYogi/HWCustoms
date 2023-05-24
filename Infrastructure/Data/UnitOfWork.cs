using System.Collections;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork               //218
    {
        public readonly StoreContext _context;      //if we create a single additional repo or single additional entity we don't need unit of work cos it's already set up for this
        private Hashtable _repositories;
        public UnitOfWork(StoreContext context)
        {
            _context = context;
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity     //when we use this method give it type of TEntity
        {
            if (_repositories == null) _repositories = new Hashtable();     //check if there is already a Hashtable created

            var type = typeof(TEntity).Name;        //check the TEntity name forexample product

            if (!_repositories.ContainsKey(type))       //check if repo already contains a key with this type
            {
                var repositoryType = typeof(GenericRepository<>);   //create a rpository type of generic repo
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);   //create an instance of this repo of for example product and passing to context

                _repositories.Add(type, repositoryInstance); //add this repo to hashtable
            }

            return (IGenericRepository<TEntity>) _repositories[type];       //and we return it
        }
    }
}