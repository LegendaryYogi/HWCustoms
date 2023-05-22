using System.Text.Json;
using Core.Entities;
using Core.Interfaces;
using StackExchange.Redis;

namespace Infrastructure.Data
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer redis)      //to inject what we need
        {
            
            _database = redis.GetDatabase();                    //we got connection to our database we use
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _database.KeyDeleteAsync(basketId);    //returns true if it was deleted
        }

        public async Task<CustomerBasket> GetBasketAsync(string basketId)
        {
            var data = await _database.StringGetAsync(basketId);        //basket stored as strings in redis database

            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(data);  //if have data then deserialize into cust basket otherwise return null
        
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)        //update or create
        {
            var created = await _database.StringSetAsync(basket.Id, 
                JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));      //if update we replace the existing basket in Redis database with whatever is coming up from the client as the new basket, basket lifetime 30 days     

            if (!created) return null; //handle this in controller

            return await GetBasketAsync(basket.Id);
        }
    }
}