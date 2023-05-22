using Core.Entities;

namespace Core.Interfaces               //update, create, delete the basket
{
    public interface IBasketRepository
    {
     Task<CustomerBasket> GetBasketAsync(string basketId);   
     Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);   // take an instance of CustBasket and call basket
     Task<bool> DeleteBasketAsync(string basketId);
    }
}