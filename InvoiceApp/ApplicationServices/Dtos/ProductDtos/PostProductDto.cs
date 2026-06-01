namespace InvoiceApp.ApplicationServices.Dtos.ProductDtos
{
    public class PostProductDto
    {
        public Guid ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
