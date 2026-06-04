using InvoiceApp.Models.DomainModels.OrderDetailAggregates;

namespace InvoiceApp.Models.DomainModels.ProductAggregates
{
    public class Product
    {
        public Guid ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }

        public bool IsDeleted { get; set; }
    }
}
