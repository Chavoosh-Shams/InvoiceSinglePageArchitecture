namespace InvoiceApp.ApplicationServices.Dtos.OrderDetailDtos
{
    public class GetOrderDetail
    {
        public Guid OrderDetailID { get; set; }
        public Guid OrderHeaderID { get; set; }
        public Guid ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
