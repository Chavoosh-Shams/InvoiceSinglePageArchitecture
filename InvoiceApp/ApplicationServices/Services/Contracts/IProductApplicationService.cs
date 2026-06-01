using InvoiceApp.ApplicationServices.Dtos.ProductDtos;

namespace InvoiceApp.ApplicationServices.Services.Contracts
{
    public interface IProductApplicationService
         : IApplicationService<PostProductDto, PutProductDto, DeleteProductDto, GetProductByIdDto, GetAllProductDto>
    {

    }
}
