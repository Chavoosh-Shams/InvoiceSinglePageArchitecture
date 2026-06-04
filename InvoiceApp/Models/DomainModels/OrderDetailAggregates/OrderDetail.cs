using InvoiceApp.Models.DomainModels.ProductAggregates;
using InvoiceApp.Models.DomainModels.OrderHeaderAggregates;

namespace InvoiceApp.Models.DomainModels.OrderDetailAggregates
{
    public class OrderDetail
    {
        public Guid OrderDetailID { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        public Guid OrderHeaderID { get; set; }
        public OrderHeader OrderHeader { get; set; }

        public Guid ProductID { get; set; }
        public Product Product { get; set; }

        public bool IsDeleted { get; set; }
    }
}
