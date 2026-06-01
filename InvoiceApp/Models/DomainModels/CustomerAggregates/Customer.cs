using InvoiceApp.Models.DomainModels.OrderHeaderAggregates;

namespace InvoiceApp.Models.DomainModels.CustomerAggregates
{
    public class Customer
    {
        public Guid CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Address { get; set; }

        public ICollection<OrderHeader> OrderHeaders { get; set; }
    }
}
