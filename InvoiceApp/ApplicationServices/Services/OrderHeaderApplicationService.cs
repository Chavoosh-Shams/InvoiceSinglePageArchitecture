using System.Net;
using InvoiceApp.Models.Services.Contracts;
using InvoiceApp.Frameworks.ResponseFrameworks;
using InvoiceApp.ApplicationServices.Services.Contracts;
using InvoiceApp.Frameworks.ResponseFrameworks.Contracts;
using InvoiceApp.ApplicationServices.Dtos.OrderDetailDtos;
using InvoiceApp.ApplicationServices.Dtos.OrderHeaderDtos;
using InvoiceApp.Models.DomainModels.OrderDetailAggregates;
using InvoiceApp.Models.DomainModels.OrderHeaderAggregates;

namespace InvoiceApp.ApplicationServices.Services
{
    public class OrderHeaderApplicationService : IOrderHeaderApplicationService
    {
        #region [- Private Field -]
        private readonly IOrderHeaderRepository _orderHeaderRepository;
        #endregion

        #region [- Ctor -]
        public OrderHeaderApplicationService(IOrderHeaderRepository orderHeaderRepository)
        {
            _orderHeaderRepository = orderHeaderRepository;
        } 
        #endregion

        #region [- PostAsync() -]
        public async Task<IResponse<PostOrderHeaderDto>> PostAsync(PostOrderHeaderDto postOrderHeaderDto)
        {
            try
            {
                if (postOrderHeaderDto == null)
                {
                    return new Response<PostOrderHeaderDto>(
                        false,
                        HttpStatusCode.BadRequest,
                        ResponseMessages.Error,
                        null
                        );
                }
                else
                {
                    var orderHeader = new OrderHeader()
                    {
                        OrderHeaderID = postOrderHeaderDto.OrderHeaderID,
                        CustomerID = postOrderHeaderDto.CustomerID,
                        OrderDate = postOrderHeaderDto.OrderDate,
                        ShipCity = postOrderHeaderDto.ShipCity,
                        ShipAddress = postOrderHeaderDto.ShipAddress,
                        OrderDetails = postOrderHeaderDto.PostOrderDetailDtos.Select(orderDetail => new OrderDetail()
                        {
                            OrderDetailID = orderDetail.OrderDetailID,
                            OrderHeaderID = orderDetail.OrderHeaderID,
                            ProductID = orderDetail.ProductID,
                            UnitPrice = orderDetail.UnitPrice,
                            Quantity = orderDetail.Quantity
                        }).ToList()
                    };
                    var result = await _orderHeaderRepository.InsertAsync(orderHeader);
                    if (!result.IsSuccessful)
                    {
                        return new Response<PostOrderHeaderDto>(
                            false,
                            HttpStatusCode.InternalServerError,
                            ResponseMessages.Error,
                            null);
                    }
                    else
                    {

                        return new Response<PostOrderHeaderDto>(
                           true,
                           HttpStatusCode.Created,
                           ResponseMessages.SuccessfullOperation,
                           postOrderHeaderDto);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new Response<PostOrderHeaderDto>(
                    false,
                    HttpStatusCode.InternalServerError,
                    ResponseMessages.Error,
                    null);
            }
        }
        #endregion

        #region [- PutAsync() -]
        public async Task<IResponse<PutOrderHeaderDto>> PutAsync(PutOrderHeaderDto putOrderHeaderDto)
        {
            try
            {
                if (putOrderHeaderDto == null)
                {
                    return new Response<PutOrderHeaderDto>(
                        false,
                        HttpStatusCode.BadRequest,
                        ResponseMessages.Error,
                        null
                        );
                }
                else
                {
                    var orderHeader = new OrderHeader()
                    {
                        OrderHeaderID = putOrderHeaderDto.OrderHeaderID,
                        CustomerID = putOrderHeaderDto.CustomerID,
                        OrderDate = putOrderHeaderDto.OrderDate,
                        ShipCity = putOrderHeaderDto.ShipCity,
                        ShipAddress = putOrderHeaderDto.ShipAddress,
                        OrderDetails = putOrderHeaderDto.PutOrderDetailDtos.Select(orderDetail => new OrderDetail()
                        {
                            OrderDetailID = orderDetail.OrderDetailID,
                            OrderHeaderID = orderDetail.OrderHeaderID,
                            ProductID = orderDetail.ProductID,
                            UnitPrice = orderDetail.UnitPrice,
                            Quantity = orderDetail.Quantity
                        }).ToList()
                    };
                    var result = await _orderHeaderRepository.UpdateAsync(orderHeader);
                    if (!result.IsSuccessful)
                    {
                        return new Response<PutOrderHeaderDto>(
                            false,
                            HttpStatusCode.InternalServerError,
                            ResponseMessages.Error,
                            null);
                    }
                    else
                    {

                        return new Response<PutOrderHeaderDto>(
                           true,
                           HttpStatusCode.OK,
                           ResponseMessages.SuccessfullOperation,
                           putOrderHeaderDto);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new Response<PutOrderHeaderDto>(
                    false,
                    HttpStatusCode.InternalServerError,
                    ResponseMessages.Error,
                    null);
            }
        }
        #endregion

        #region [- DeleteAsync() -]
        public async Task<IResponse<DeleteOrderHeaderDto>> DeleteAsync(DeleteOrderHeaderDto deleteOrderHeaderDto)
        {
            try
            {
                if (deleteOrderHeaderDto == null)
                {
                    return new Response<DeleteOrderHeaderDto>(
                        false,
                        HttpStatusCode.BadRequest,
                        ResponseMessages.Error,
                        null
                        );
                }
                else
                {
                    var orderHeader = new OrderHeader()
                    {
                        OrderHeaderID = deleteOrderHeaderDto.OrderHeaderID,
                        CustomerID = deleteOrderHeaderDto.CustomerID,
                        OrderDate = deleteOrderHeaderDto.OrderDate,
                        ShipCity = deleteOrderHeaderDto.ShipCity,
                        ShipAddress = deleteOrderHeaderDto.ShipAddress
                    };
                    var result = await _orderHeaderRepository.DeleteAsync(orderHeader);
                    if (!result.IsSuccessful)
                    {
                        return new Response<DeleteOrderHeaderDto>(
                            false,
                            HttpStatusCode.InternalServerError,
                            ResponseMessages.Error,
                            null);
                    }
                    else
                    {

                        return new Response<DeleteOrderHeaderDto>(
                           true,
                           HttpStatusCode.OK,
                           ResponseMessages.SuccessfullOperation,
                           deleteOrderHeaderDto);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new Response<DeleteOrderHeaderDto>(
                    false,
                    HttpStatusCode.InternalServerError,
                    ResponseMessages.Error,
                    null);
            }
        }
        #endregion

        #region [- GetByIdAsync() -]
        public async Task<IResponse<GetOrderHeaderByIdDto>> GetByIdAsync(GetOrderHeaderByIdDto getOrderHeaderByIdDto)
        {
          
            try
            {
                if (getOrderHeaderByIdDto == null)
                {
                    return new Response<GetOrderHeaderByIdDto>(
                        false,
                        HttpStatusCode.BadRequest,
                        ResponseMessages.Error,
                        null
                        );
                }
                else
                {
                    var orderHeader = new OrderHeader()
                    {
                        OrderHeaderID = getOrderHeaderByIdDto.OrderHeaderID,
                        CustomerID = getOrderHeaderByIdDto.CustomerID,
                        OrderDate = getOrderHeaderByIdDto.OrderDate,
                        ShipCity = getOrderHeaderByIdDto.ShipCity,
                        ShipAddress = getOrderHeaderByIdDto.ShipAddress
                    };
                    var customerDto = await _orderHeaderRepository.SelectByIdAsync(orderHeader);
                    if (!customerDto.IsSuccessful || customerDto.Value == null)
                    {
                        return new Response<GetOrderHeaderByIdDto>(
                            false,
                            HttpStatusCode.NotFound,
                            ResponseMessages.NullInput,
                            null);
                    }
                    else
                    {
                        var responseDto = new GetOrderHeaderByIdDto()
                        {   
                            OrderHeaderID = customerDto.Value.OrderHeaderID,
                            CustomerID = customerDto.Value.CustomerID,
                            CustomerFirstName = customerDto.Value.Customer.FirstName,
                            CustomerLastName = customerDto.Value.Customer.LastName,
                            CustomerPhone = customerDto.Value.Customer.Phone,
                            OrderDate = customerDto.Value.OrderDate,
                            ShipCity = customerDto.Value.ShipCity,
                            ShipAddress = customerDto.Value.ShipAddress,
                            GetOrderDetails = customerDto.Value.OrderDetails.Select(orderDetail=> new GetOrderDetail()
                            {
                                OrderDetailID = orderDetail.OrderDetailID,
                                OrderHeaderID = orderDetail.OrderHeaderID,
                                ProductID = orderDetail.ProductID,
                                ProductName = orderDetail.Product.ProductName,
                                UnitPrice = orderDetail.Product.UnitPrice,
                                Quantity = orderDetail.Quantity
                            }).ToList()
                        };
                        return new Response<GetOrderHeaderByIdDto>(
                           true,
                           HttpStatusCode.OK,
                           ResponseMessages.SuccessfullOperation,
                           responseDto);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new Response<GetOrderHeaderByIdDto>(
                    false,
                    HttpStatusCode.InternalServerError,
                    ResponseMessages.Error,
                    null);
            }
        }
        #endregion

        #region [- GetAllAsync() -]
        public async Task<IResponse<List<GetAllOrderHeaderDto>>> GetAllAsync()
        {
            try
            {
                var orderHeaders = await _orderHeaderRepository.SelectAllAsync();
                if (!orderHeaders.IsSuccessful || orderHeaders.Value == null)
                {
                    return new Response<List<GetAllOrderHeaderDto>>(
                        false,
                        HttpStatusCode.NotFound,
                        ResponseMessages.NullInput,
                        null);
                }
                else
                {
                    var result = orderHeaders.Value.Select(orderHeader => new GetAllOrderHeaderDto()
                    {
                        OrderHeaderID = orderHeader.OrderHeaderID,
                        CustomerID = orderHeader.CustomerID,
                        CustomerFirstName = orderHeader.Customer.FirstName,
                        CustomerLastName = orderHeader.Customer.LastName,
                        CustomerPhone = orderHeader.Customer.Phone,
                        OrderDate = orderHeader.OrderDate,
                        ShipCity = orderHeader.ShipCity,
                        ShipAddress = orderHeader.ShipAddress,
                    }).ToList();

                    return new Response<List<GetAllOrderHeaderDto>>(
                        true,
                        HttpStatusCode.OK,
                        ResponseMessages.SuccessfullOperation,
                        result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new Response<List<GetAllOrderHeaderDto>> (
                    false,
                    HttpStatusCode.InternalServerError,
                    ResponseMessages.Error,
                    null);
            }
        }
        #endregion
    }
}