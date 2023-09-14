namespace MinDatabaseAPI.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public int AddressId { get; set; }

     //  public Address CustomerAddress { get; set; }    

       
    }

    
}
