namespace InvoiceApp.ApplicationServices.Dtos.CustomerDtos
{
    public class PostCustomerDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
    }
    public class GetAllCustomerDto
    {
        public Guid CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
    }
}
