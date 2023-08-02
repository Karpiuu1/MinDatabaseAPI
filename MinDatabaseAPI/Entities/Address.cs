namespace MinDatabaseAPI.Entities
{
    public class Address
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public string AddressText { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}
