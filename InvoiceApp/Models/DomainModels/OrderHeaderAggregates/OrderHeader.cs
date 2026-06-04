using InvoiceApp.Models.DomainModels.CustomerAggregates;
using InvoiceApp.Models.DomainModels.OrderDetailAggregates;

namespace InvoiceApp.Models.DomainModels.OrderHeaderAggregates
{
    public class OrderHeader
    {
        public Guid OrderHeaderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShipCity { get; set; }
        public string ShipAddress { get; set; }

        public Guid CustomerID { get; set; }
        public Customer Customer { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }

        public bool IsDeleted { get; set; }
    }
}
