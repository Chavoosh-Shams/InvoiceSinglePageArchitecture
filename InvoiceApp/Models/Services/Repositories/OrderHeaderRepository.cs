using System.Net;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using InvoiceApp.Models.Services.Contracts;
using InvoiceApp.Frameworks.ResponseFrameworks;
using InvoiceApp.Frameworks.ResponseFrameworks.Contracts;
using InvoiceApp.Models.DomainModels.OrderHeaderAggregates;

namespace InvoiceApp.Models.Services.Repositories
{
    public class OrderHeaderRepository : IOrderHeaderRepository
    {
        #region [- PrivateField -]
        private readonly ProjectDbContext _context;
        #endregion

        #region [- Ctor -]
        public OrderHeaderRepository(ProjectDbContext context)
        {
            _context = context;
        }
        #endregion

        #region [- InsertAsync() -]
        public async Task<IResponse<OrderHeader>> InsertAsync(OrderHeader orderHeader)
        {
            try
            {
                if (orderHeader == null)
                {
                    return new Response<OrderHeader>(
                        false,
                        HttpStatusCode.BadRequest,
                        ResponseMessages.NullInput,
                        null
                        );
                }
                else
                {
                    var jsonString = JsonSerializer.Serialize(orderHeader, new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        ReferenceHandler = ReferenceHandler.IgnoreCycles
                    });
                    var jsonParam = new SqlParameter("@JsonData", jsonString);
                    await _context.Database.ExecuteSqlRawAsync(
                        "EXEC dbo.Sp_InsertOrder @JsonData",
                        jsonParam
                    );
                    return new Response<OrderHeader>(
                        true,
                        HttpStatusCode.Created,
                        ResponseMessages.SuccessfullOperation,
                        orderHeader);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new Response<OrderHeader>(
                    false,
                    HttpStatusCode.InternalServerError,
                    ResponseMessages.Error,
                    null
                );
            }
        }
        #endregion

        #region [- UpdateAsync() -]
        public async Task<IResponse<OrderHeader>> UpdateAsync(OrderHeader orderHeader)
        {
            try
            {
                if (orderHeader == null)
                {
                    return new Response<OrderHeader>(
                        false,
                        HttpStatusCode.BadRequest,
                        ResponseMessages.NullInput,
                        null
                        );
                }
                else
                {
                    var jsonString = JsonSerializer.Serialize(orderHeader, new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        ReferenceHandler = ReferenceHandler.IgnoreCycles
                    });
                    var jsonParam = new SqlParameter("@JsonData", jsonString);
                    await _context.Database.ExecuteSqlRawAsync(
                        "EXEC dbo.Sp_UpdateOrder @JsonData",
                        jsonParam
                    );
                    return new Response<OrderHeader>(
                        true,
                        HttpStatusCode.OK,
                        ResponseMessages.SuccessfullOperation,
                        orderHeader);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new Response<OrderHeader>(
                    false,
                    HttpStatusCode.InternalServerError,
                    ResponseMessages.Error,
                    null
                );
            }
        }
        #endregion

        #region [- DeleteAsync() -]
        public async Task<IResponse<OrderHeader>> DeleteAsync(OrderHeader orderHeader)
        {
            try
            {
                if (orderHeader == null)
                {
                    return new Response<OrderHeader>(
                        false,
                        HttpStatusCode.BadRequest,
                        ResponseMessages.NullInput,
                        null
                        );
                }
                else
                {
                    var param = new SqlParameter("@OrderHeaderID", orderHeader.OrderHeaderID);
                    await _context.Database.ExecuteSqlRawAsync(
                        "EXEC dbo.Sp_DeleteOrder @OrderHeaderID",
                        param
                    );
                    return new Response<OrderHeader>(
                        true,
                        HttpStatusCode.OK,
                        ResponseMessages.SuccessfullOperation,
                        orderHeader);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new Response<OrderHeader>(
                    false,
                    HttpStatusCode.InternalServerError,
                    ResponseMessages.Error,
                    null
                );
            }
        }
        #endregion

        #region [- SelectByIdAsync() -]
        public async Task<IResponse<OrderHeader>> SelectByIdAsync(OrderHeader orderHeader)
        {
            try
            {
                if (orderHeader == null)
                {
                    return new Response<OrderHeader>(
                        false,
                        HttpStatusCode.BadRequest,
                        ResponseMessages.NullInput,
                        null
                        );
                }
                else
                {
                    var existingOrderHeader = await _context.OrderHeaders
                        .AsNoTracking()
                        .Include(oh => oh.OrderDetails)
                        .ThenInclude(od => od.Product)
                        .Include(oh => oh.Customer)
                        .IgnoreQueryFilters()
                        .SingleOrDefaultAsync(oh => oh.OrderHeaderID == orderHeader.OrderHeaderID);

                    if (existingOrderHeader == null)
                    {
                        return new Response<OrderHeader>(
                            false,
                            HttpStatusCode.NotFound,
                            ResponseMessages.NotFound,
                            null
                            );
                    }
                    else
                    {
                        return new Response<OrderHeader>(
                            true,
                            HttpStatusCode.OK,
                            ResponseMessages.SuccessfullOperation,
                            existingOrderHeader);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new Response<OrderHeader>(
                    false,
                    HttpStatusCode.InternalServerError,
                    ResponseMessages.Error,
                    null
                );
            }
        }
        #endregion

        #region [- SelectAllAsync() -]
        public async Task<IResponse<IEnumerable<OrderHeader>>> SelectAllAsync()
        {
            try
            {
                var orderHeaders = await _context.OrderHeaders
                    .AsNoTracking()
                    .Include(oh => oh.Customer)
                    .IgnoreQueryFilters()
                    .ToListAsync();
                return new Response<IEnumerable<OrderHeader>>(
                    true,
                    HttpStatusCode.OK,
                    ResponseMessages.SuccessfullOperation,
                    orderHeaders);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new Response<IEnumerable<OrderHeader>>(
                    false,
                    HttpStatusCode.InternalServerError,
                    ResponseMessages.Error,
                    null
                );
            }
        } 
        #endregion
    }
}
