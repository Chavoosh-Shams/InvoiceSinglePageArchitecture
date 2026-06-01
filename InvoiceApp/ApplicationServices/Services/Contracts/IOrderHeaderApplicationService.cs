using InvoiceApp.ApplicationServices.Dtos.OrderHeaderDtos;

namespace InvoiceApp.ApplicationServices.Services.Contracts
{
    public interface IOrderHeaderApplicationService
        : IApplicationService<PostOrderHeaderDto, PutOrderHeaderDto, DeleteOrderHeaderDto, GetOrderHeaderByIdDto, GetAllOrderHeaderDto>
    {

    }
}
