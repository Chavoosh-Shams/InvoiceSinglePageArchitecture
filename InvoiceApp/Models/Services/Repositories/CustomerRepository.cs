using System.Net;
using Microsoft.EntityFrameworkCore;
using InvoiceApp.Models.Services.Contracts;
using InvoiceApp.Frameworks.ResponseFrameworks;
using InvoiceApp.Models.DomainModels.CustomerAggregates;
using InvoiceApp.Frameworks.ResponseFrameworks.Contracts;

namespace InvoiceApp.Models.Services.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {

        #region [- PrivateField -]
        private readonly ProjectDbContext _context;
        #endregion

        #region [- Ctor -]
        public CustomerRepository(ProjectDbContext context)
        {
            _context = context;
        }
        #endregion

        #region [- InsertAsync() -]
        public async Task<IResponse<Customer>> InsertAsync(Customer customer)
        {
            try
            {
                if (customer == null)
                {
                    return new Response<Customer>(
                        false,
                        HttpStatusCode.BadRequest,
                        ResponseMessages.NullInput,
                        null
                        );
                }
                else
                {
                    await _context.AddAsync(customer);
                    await _context.SaveChangesAsync();
                    return new Response<Customer>(
                        true,
                        HttpStatusCode.Created,
                        ResponseMessages.SuccessfullOperation,
                        customer);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new Response<Customer>(
                    false,
                    HttpStatusCode.InternalServerError,
                    ResponseMessages.Error,
                    null
                );
            }
        }
        #endregion

        #region [- UpdateAsync() -]
        public async Task<IResponse<Customer>> UpdateAsync(Customer customer)
        {
            try
            {
                if (customer == null)
                {
                    return new Response<Customer>(
                        false,
                        HttpStatusCode.BadRequest,
                        ResponseMessages.NullInput,
                        null
                        );
                }
                else
                {
                    var existingCustomer = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerID == customer.CustomerID);
                    if (existingCustomer == null)
                    {
                        return new Response<Customer>(
                            false,
                            HttpStatusCode.NotFound,
                            ResponseMessages.NotFound,
                            null
                            );
                    }
                    else
                    {
                        _context.Entry(existingCustomer).CurrentValues.SetValues(customer);
                        await _context.SaveChangesAsync();
                        return new Response<Customer>(
                            true,
                            HttpStatusCode.OK,
                            ResponseMessages.SuccessfullOperation,
                            existingCustomer
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new Response<Customer>(
                    false,
                    HttpStatusCode.InternalServerError,
                    ResponseMessages.Error,
                    null
                );
            }
        }
        #endregion

        #region [- DeleteAsync() -]
        public async Task<IResponse<Customer>> DeleteAsync(Customer customer)
        {
            try
            {

                if (customer == null)
                {
                    return new Response<Customer>(
                        false,
                        HttpStatusCode.BadRequest,
                        ResponseMessages.NullInput,
                        null
                        );
                }
                else
                {
                    var existingCustomer = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerID == customer.CustomerID);
                    if (existingCustomer == null)
                    {
                        return new Response<Customer>(
                            false,
                            HttpStatusCode.NotFound,
                            ResponseMessages.NotFound,
                            null
                            );
                    }
                    else
                    {
                        existingCustomer.IsDeleted = true;
                        _context.Customers.Update(existingCustomer);
                        await _context.SaveChangesAsync();
                        return new Response<Customer>(
                            true,
                            HttpStatusCode.OK,
                            ResponseMessages.SuccessfullOperation,
                            existingCustomer
                            );
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new Response<Customer>(
                    false,
                    HttpStatusCode.InternalServerError,
                    ResponseMessages.Error,
                    null
                );
            }
        }
        #endregion

        #region [- SelectByIdAsync() -]
        public async Task<IResponse<Customer>> SelectByIdAsync(Customer customer)
        {
            try
            {
                if (customer == null)
                {
                    return new Response<Customer>(
                        false,
                        HttpStatusCode.BadRequest,
                        ResponseMessages.NullInput,
                        null
                        );
                }
                else
                {
                    var existingCustomer = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerID == customer.CustomerID);
                    if (existingCustomer == null)
                    {
                        return new Response<Customer>(
                            false,
                            HttpStatusCode.NotFound,
                            ResponseMessages.NotFound,
                            null
                            );
                    }
                    else
                    {
                        return new Response<Customer>(
                           true,
                           HttpStatusCode.OK,
                           ResponseMessages.SuccessfullOperation,
                           existingCustomer);
                    }

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new Response<Customer>(
                    false,
                    HttpStatusCode.InternalServerError,
                    ResponseMessages.Error,
                    null
                );
            }
        }
        #endregion

        #region [- SelectAllAsync() -]
        public async Task<IResponse<IEnumerable<Customer>>> SelectAllAsync()
        {
            try
            {
                var customers = await _context.Customers.AsNoTracking().ToListAsync();
                return new Response<IEnumerable<Customer>>(
                   true,
                   HttpStatusCode.OK,
                   ResponseMessages.SuccessfullOperation,
                   customers);
            }
            catch(Exception ex) { 
                Console.WriteLine(ex.ToString());
                return new Response<IEnumerable<Customer>>(
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