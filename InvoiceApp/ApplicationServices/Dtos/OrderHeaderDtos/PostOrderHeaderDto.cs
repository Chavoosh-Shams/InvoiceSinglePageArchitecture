using InvoiceApp.ApplicationServices.Dtos.OrderDetailDtos;

namespace InvoiceApp.ApplicationServices.Dtos.OrderHeaderDtos
{
    public class PostOrderHeaderDto
    {
        public Guid OrderHeaderID { get; set; }
        public Guid CustomerID { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShipCity { get; set; }
        public string ShipAddress { get; set; }
        public List<PostOrderDetailDto> PostOrderDetailDtos { get; set; }
    }
}
