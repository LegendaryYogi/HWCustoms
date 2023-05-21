namespace Core.Specifications
{
    public class ProductSpecParams                  //64
    {
        private const int MaxPageSize = 50;
        public int PageIndex {get; set;} = 1;  //by default we are getting the 1st page of our sorting

        private int _pageSize = 6;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string Sort { get; set; }
        private string _search;
        public string Search 
        {
            get => _search;
            set => _search = value.ToLower();       //when a search term comes into our API we convert it into lowercase 66
        }  
    }
}