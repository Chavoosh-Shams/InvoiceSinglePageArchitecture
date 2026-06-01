namespace InvoiceApp.ApplicationServices.Dtos.ProductDtos
{
    public class DeleteProductDto
    {
        public Guid ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
