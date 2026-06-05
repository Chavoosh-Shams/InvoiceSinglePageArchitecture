using System.Net;
using Microsoft.EntityFrameworkCore;
using InvoiceApp.Models.Services.Contracts;
using InvoiceApp.Frameworks.ResponseFrameworks;
using InvoiceApp.Models.DomainModels.ProductAggregates;
using InvoiceApp.Frameworks.ResponseFrameworks.Contracts;

namespace InvoiceApp.Models.Services.Repositories
{
    public class ProductRepository : IProductRepository
    {
        #region [- PrivateField -]
        private readonly ProjectDbContext _context;
        #endregion

        #region [- Ctor -]
        public ProductRepository(ProjectDbContext context)
        {
            _context = context;
        }
        #endregion

        #region [- InsertAsync() -]
        public async Task<IResponse<Product>> InsertAsync(Product product)
        {
            try
            {
                if (product == null)
                {
                    return new Response<Product>(
                        false,
                        HttpStatusCode.BadRequest,
                        ResponseMessages.NullInput,
                        null
                        );
                }
                else
                {
                    await _context.AddAsync(product);
                    await _context.SaveChangesAsync();
                    return new Response<Product>(
                        true,
                        HttpStatusCode.Created,
                        ResponseMessages.SuccessfullOperation,
                        product);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new Response<Product>(
                    false,
                    HttpStatusCode.InternalServerError,
                    ResponseMessages.Error,
                    null);
            }
        }
        #endregion

        #region [- UpdateAsync() -]
        public async Task<IResponse<Product>> UpdateAsync(Product product)
        {
            try
            {
                if (product == null)
                {
                    return new Response<Product>(
                        false,
                        HttpStatusCode.BadRequest,
                        ResponseMessages.NullInput,
                        null
                        );
                }
                else
                {
                    var existingProduct = await _context.Products.SingleOrDefaultAsync(p => p.ProductID == product.ProductID);
                    if (existingProduct == null)
                    {
                        return new Response<Product>(
                            false,
                            HttpStatusCode.NotFound,
                            ResponseMessages.NotFound,
                            null
                            );
                    }
                    else
                    {
                        _context.Entry(existingProduct).CurrentValues.SetValues(product);
                        await _context.SaveChangesAsync();
                        return new Response<Product>(
                            true,
                            HttpStatusCode.OK,
                            ResponseMessages.SuccessfullOperation,
                            existingProduct
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new Response<Product>(
                    false,
                    HttpStatusCode.InternalServerError,
                    ResponseMessages.Error,
                    null);
            }
        }
        #endregion

        #region [- DeleteAsync() -]
        public async Task<IResponse<Product>> DeleteAsync(Product product)
        {
            try
            {
                if (product == null)
                {
                    return new Response<Product>(
                        false,
                        HttpStatusCode.BadRequest,
                        ResponseMessages.NullInput,
                        null
                        );
                }
                else
                {
                    var existingProduct = await _context.Products.SingleOrDefaultAsync(p => p.ProductID == product.ProductID);
                    if (existingProduct == null)
                    {
                        return new Response<Product>(
                            false,
                            HttpStatusCode.NotFound,
                            ResponseMessages.NotFound,
                            null
                            );
                    }
                    else
                    {
                        existingProduct.IsDeleted = true;
                        _context.Products.Update(existingProduct);
                        await _context.SaveChangesAsync();
                        return new Response<Product>(
                            true,
                            HttpStatusCode.OK,
                            ResponseMessages.SuccessfullOperation,
                            existingProduct
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new Response<Product>(
                    false,
                    HttpStatusCode.InternalServerError,
                    ResponseMessages.Error,
                    null);
            }
        }
        #endregion

        #region [- SelectByIdAsync() -]
        public async Task<IResponse<Product>> SelectByIdAsync(Product product)
        {
            try
            {
                if (product == null)
                {
                    return new Response<Product>(
                        false,
                        HttpStatusCode.BadRequest,
                        ResponseMessages.NullInput,
                        null
                        );
                }
                else
                {
                    var existingProduct = await _context.Products.SingleOrDefaultAsync(p => p.ProductID == product.ProductID);
                    if (existingProduct == null)
                    {
                        return new Response<Product>(
                            false,
                            HttpStatusCode.NotFound,
                            ResponseMessages.NotFound,
                            null
                            );
                    }
                    else
                    {
                        return new Response<Product>(
                            true,
                            HttpStatusCode.OK,
                            ResponseMessages.SuccessfullOperation,
                            existingProduct
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new Response<Product>(
                    false,
                    HttpStatusCode.InternalServerError,
                    ResponseMessages.Error,
                    null);
            }
        }
        #endregion

        #region [- SelectAllAsync() -]
        public async Task<IResponse<IEnumerable<Product>>> SelectAllAsync()
        {
            try
            {
                var products = await _context.Products.AsNoTracking().ToListAsync();
                return new Response<IEnumerable<Product>>(
                    true,
                   HttpStatusCode.OK,
                   ResponseMessages.SuccessfullOperation,
                   products);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new Response<IEnumerable<Product>>(
                    false,
                    HttpStatusCode.InternalServerError,
                    ResponseMessages.Error,
                    null);
            }
        } 
        #endregion
    }
}