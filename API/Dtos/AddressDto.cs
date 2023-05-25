using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class AddressDto         //when calling get user address we are getting a loop from address.cs because of AppUser so here is the fix with mapping 177
    {
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Zipcode { get; set; }
    }
}