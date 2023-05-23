namespace Core.Entities                         //136 basket
{
    public class CustomerBasket
    {
        public CustomerBasket()     //create a new instance without knowing the id
        {
        }

        public CustomerBasket(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
    }
}