namespace InvoiceApp.ApplicationServices.Dtos.OrderHeaderDtos
{
    public class GetAllOrderHeaderDto
    {
        public Guid OrderHeaderID { get; set; }
        public Guid CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShipCity { get; set; }
        public string ShipAddress { get; set; }
    }
}
