using System.Net;
using InvoiceApp.Models.Services.Contracts;
using InvoiceApp.Frameworks.ResponseFrameworks;
using InvoiceApp.ApplicationServices.Dtos.CustomerDtos;
using InvoiceApp.ApplicationServices.Services.Contracts;
using InvoiceApp.Models.DomainModels.CustomerAggregates;
using InvoiceApp.Frameworks.ResponseFrameworks.Contracts;

namespace InvoiceApp.ApplicationServices.Services
{
    public class CustomerApplicationService : ICustomerApplicationService
    {
        #region [- PrivateField -]
        private readonly ICustomerRepository _customerRepository;
        #endregion

        #region [- Ctor -]
        public CustomerApplicationService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        #endregion

        #region [- PostAsync() -]
        public async Task<IResponse<PostCustomerDto>> PostAsync(PostCustomerDto postCustomerDto)
        {
            try
            {
                if (postCustomerDto == null)
                {
                    return new Response<PostCustomerDto>(
                        false,
                        HttpStatusCode.BadRequest,
                        ResponseMessages.Error,
                        null
                        );
                }
                else
                {
                    var customer = new Customer()
                    {
                        FirstName = postCustomerDto.FirstName,
                        LastName = postCustomerDto.LastName,
                        Phone = postCustomerDto.Phone,
                        City = postCustomerDto.City,
                        Address = postCustomerDto.Address,
                    };
                    var result = await _customerRepository.InsertAsync(customer);
                    if (!result.IsSuccessful)
                    {
                        return new Response<PostCustomerDto>(
                            false,
                            HttpStatusCode.InternalServerError,
                            ResponseMessages.Error,
                            null);
                    }
                    else
                    {
                        return new Response<PostCustomerDto>(
                           true,
                           HttpStatusCode.Created,
                           ResponseMessages.SuccessfullOperation,
                           postCustomerDto);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new Response<PostCustomerDto>(
                    false,
                    HttpStatusCode.InternalServerError,
                    ResponseMessages.Error,
                    null);
            }
        }
        #endregion

        #region [- PutAsync() -]
        public async Task<IResponse<PutCustomerDto>> PutAsync(PutCustomerDto putCustomerDto)
        {
            try
            {
                if (putCustomerDto == null)
                {
                    return new Response<PutCustomerDto>(
                        false,
                        HttpStatusCode.BadRequest,
                        ResponseMessages.Error,
                        null
                        );
                }
                else
                {
                    var customer = new Customer()
                    {
                        CustomerID = putCustomerDto.CustomerID,
                        FirstName = putCustomerDto.FirstName,
                        LastName = putCustomerDto.LastName,
                        Phone = putCustomerDto.Phone,
                        City = putCustomerDto.City,
                        Address = putCustomerDto.Address,
                    };
                    var result = await _customerRepository.UpdateAsync(customer);
                    if (!result.IsSuccessful)
                    {
                        return new Response<PutCustomerDto>(
                            false,
                            HttpStatusCode.InternalServerError,
                            ResponseMessages.Error,
                            null);
                    }
                    else
                    {
                        return new Response<PutCustomerDto>(
                           true,
                           HttpStatusCode.OK,
                           ResponseMessages.SuccessfullOperation,
                           putCustomerDto);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new Response<PutCustomerDto>(
                    false,
                    HttpStatusCode.InternalServerError,
                    ResponseMessages.Error,
                    null);
            }
        }
        #endregion

        #region [- DeleteAsync() -]
        public async Task<IResponse<DeleteCustomerDto>> DeleteAsync(DeleteCustomerDto deleteCustomerDto)
        {
            try
            {
                if (deleteCustomerDto == null)
                {
                    return new Response<DeleteCustomerDto>(
                        false,
                        HttpStatusCode.BadRequest,
                        ResponseMessages.Error,
                        null
                        );
                }
                else
                {
                    var customer = new Customer()
                    {
                        CustomerID = deleteCustomerDto.CustomerID,
                        FirstName = deleteCustomerDto.FirstName,
                        LastName = deleteCustomerDto.LastName,
                        Phone = deleteCustomerDto.Phone,
                        City = deleteCustomerDto.City,
                        Address = deleteCustomerDto.Address,
                    };
                    var result = await _customerRepository.DeleteAsync(customer);
                    if (!result.IsSuccessful)
                    {
                        return new Response<DeleteCustomerDto>(
                            false,
                            HttpStatusCode.NotFound,
                            ResponseMessages.NullInput,
                            null);
                    }
                    else
                    {
                        return new Response<DeleteCustomerDto>(
                           true,
                           HttpStatusCode.OK,
                           ResponseMessages.SuccessfullOperation,
                           deleteCustomerDto
                           );
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new Response<DeleteCustomerDto>(
                    false,
                    HttpStatusCode.InternalServerError,
                    ResponseMessages.Error,
                    null);
            }
        }
        #endregion

        #region [- GetByIdAsync() -]
        public async Task<IResponse<GetCustomerByIdDto>> GetByIdAsync(GetCustomerByIdDto getCustomerByIdDto)
        {
            try
            {
                if (getCustomerByIdDto == null)
                {
                    return new Response<GetCustomerByIdDto>(
                        false,
                        HttpStatusCode.BadRequest,
                        ResponseMessages.Error,
                        null
                        );
                }
                else
                {
                    var customer = new Customer()
                    {
                        CustomerID = getCustomerByIdDto.CustomerID,
                        FirstName = getCustomerByIdDto.FirstName,
                        LastName = getCustomerByIdDto.LastName,
                        Phone = getCustomerByIdDto.Phone,
                        City = getCustomerByIdDto.City,
                        Address = getCustomerByIdDto.Address,
                    };
                    var customerDto = await _customerRepository.SelectByIdAsync(customer);
                    if (!customerDto.IsSuccessful || customerDto.Value == null)
                    {
                        return new Response<GetCustomerByIdDto>(
                            false,
                            HttpStatusCode.NotFound,
                            ResponseMessages.NullInput,
                            null);
                    }
                    else
                    {
                        var responseDto = new GetCustomerByIdDto()
                        {
                            CustomerID = customerDto.Value.CustomerID,
                            FirstName = customerDto.Value.FirstName,
                            LastName = customerDto.Value.LastName,
                            Phone = customerDto.Value.Phone,
                            City = customerDto.Value.City,
                            Address = customerDto.Value.Address,
                        };
                        return new Response<GetCustomerByIdDto>(
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
                return new Response<GetCustomerByIdDto>(
                    false,
                    HttpStatusCode.InternalServerError,
                    ResponseMessages.Error,
                    null);
            }
        }
        #endregion

        #region [- GetAllAsync() -]
        public async Task<IResponse<List<GetAllCustomerDto>>> GetAllAsync()
        {
            try
            {
                var customers = await _customerRepository.SelectAllAsync();
                if (!customers.IsSuccessful || customers.Value == null)
                {
                    return new Response<List<GetAllCustomerDto>>(
                        false,
                        HttpStatusCode.NotFound,
                        ResponseMessages.NullInput,
                        null);
                }
                else
                {
                    var result = customers.Value.Select(customer => new GetAllCustomerDto()
                    {
                        CustomerID = customer.CustomerID,
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        Phone = customer.Phone,
                        City = customer.City,
                        Address = customer.Address,
                    }).ToList();
                    return new Response<List<GetAllCustomerDto>>(
                    true,
                    HttpStatusCode.OK,
                    ResponseMessages.SuccessfullOperation,
                    result);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new Response<List<GetAllCustomerDto>>(
                    false,
                    HttpStatusCode.InternalServerError,
                    ResponseMessages.Error,
                    null);
            }
        } 
        #endregion
    }
}
