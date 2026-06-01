namespace InvoiceApp.ApplicationServices.Dtos.CustomerDtos
{
    public class DeleteCustomerDto
    {
        public Guid CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
    }
}
