using System.Net;
using InvoiceApp.Models.Services.Contracts;
using InvoiceApp.Frameworks.ResponseFrameworks;
using InvoiceApp.ApplicationServices.Dtos.ProductDtos;
using InvoiceApp.Models.DomainModels.ProductAggregates;
using InvoiceApp.ApplicationServices.Services.Contracts;
using InvoiceApp.Frameworks.ResponseFrameworks.Contracts;

namespace InvoiceApp.ApplicationServices.Services
{
    public class ProductApplicationService : IProductApplicationService
    {

        #region [- PrivateFields -]
        private readonly IProductRepository _productRepository;
        #endregion

        #region [- Ctor -]
        public ProductApplicationService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        #endregion

        #region [- PostAsync() -]
        public async Task<IResponse<PostProductDto>> PostAsync(PostProductDto postProductDto)
        {
            try
            {
                if (postProductDto == null)
                {
                    return new Response<PostProductDto>(
                        false,
                        HttpStatusCode.BadRequest,
                        ResponseMessages.Error,
                        null
                        );
                }
                else
                {
                    var product = new Product()
                    {
                        ProductID = postProductDto.ProductID,
                        ProductName = postProductDto.ProductName,
                        UnitPrice = postProductDto.UnitPrice,
                    };
                    var result = await _productRepository.InsertAsync(product);
                    if (!result.IsSuccessful)
                    {
                        return new Response<PostProductDto>(
                            false,
                            HttpStatusCode.InternalServerError,
                            ResponseMessages.Error,
                            null);
                    }
                    else
                    {
                        return new Response<PostProductDto>(
                           true,
                           HttpStatusCode.Created,
                           ResponseMessages.SuccessfullOperation,
                           postProductDto);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new Response<PostProductDto>(
                    false,
                    HttpStatusCode.InternalServerError,
                    ResponseMessages.Error,
                    null);
            }
        }
        #endregion

        #region [- PutAsync() -]
        public async Task<IResponse<PutProductDto>> PutAsync(PutProductDto putProductDto)
        {
            try
            {
                if (putProductDto == null)
                {
                    return new Response<PutProductDto>(
                        false,
                        HttpStatusCode.BadRequest,
                        ResponseMessages.Error,
                        null
                        );
                }
                else
                {
                    var product = new Product()
                    {
                        ProductID = putProductDto.ProductID,
                        ProductName = putProductDto.ProductName,
                        UnitPrice = putProductDto.UnitPrice,
                    };
                    var result = await _productRepository.UpdateAsync(product);
                    if (!result.IsSuccessful)
                    {
                        return new Response<PutProductDto>(
                            false,
                            HttpStatusCode.InternalServerError,
                            ResponseMessages.Error,
                            null);
                    }
                    else
                    {
                        return new Response<PutProductDto>(
                           true,
                           HttpStatusCode.OK,
                           ResponseMessages.SuccessfullOperation,
                           putProductDto);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new Response<PutProductDto>(
                    false,
                    HttpStatusCode.InternalServerError,
                    ResponseMessages.Error,
                    null);
            }
        }
        #endregion

        #region [- DeleteAsync() -]
        public async Task<IResponse<DeleteProductDto>> DeleteAsync(DeleteProductDto deleteProductDto)
        {
            try
            {
                if (deleteProductDto == null)
                {
                    return new Response<DeleteProductDto>(
                        false,
                        HttpStatusCode.BadRequest,
                        ResponseMessages.Error,
                        null
                        );
                }
                else
                {
                    var product = new Product()
                    {
                        ProductID = deleteProductDto.ProductID,
                        ProductName = deleteProductDto.ProductName,
                        UnitPrice = deleteProductDto.UnitPrice,
                    };
                    var result = await _productRepository.DeleteAsync(product);
                    if (!result.IsSuccessful)
                    {
                        return new Response<DeleteProductDto>(
                            false,
                            HttpStatusCode.InternalServerError,
                            ResponseMessages.Error,
                            null);
                    }
                    else
                    {
                        return new Response<DeleteProductDto>(
                           true,
                           HttpStatusCode.OK,
                           ResponseMessages.SuccessfullOperation,
                           deleteProductDto);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new Response<DeleteProductDto>(
                    false,
                    HttpStatusCode.InternalServerError,
                    ResponseMessages.Error,
                    null);
            }
        }
        #endregion

        #region [- GetByIdAsync() -]
        public async Task<IResponse<GetProductByIdDto>> GetByIdAsync(GetProductByIdDto getProductByIdDto)
        {
            try
            {
                if (getProductByIdDto == null)
                {
                    return new Response<GetProductByIdDto>(
                        false,
                        HttpStatusCode.BadRequest,
                        ResponseMessages.Error,
                        null
                        );
                }
                else
                {
                    var product = new Product()
                    {
                        ProductID = getProductByIdDto.ProductID,
                        ProductName = getProductByIdDto.ProductName,
                        UnitPrice = getProductByIdDto.UnitPrice,
                    };
                    var productDto = await _productRepository.SelectByIdAsync(product);
                    if (!productDto.IsSuccessful || productDto.Value == null)
                    {
                        return new Response<GetProductByIdDto>(
                            false,
                            HttpStatusCode.NotFound,
                            ResponseMessages.NullInput,
                            null);
                    }
                    else
                    {
                        var responseDto = new GetProductByIdDto()
                        {
                            ProductID = productDto.Value.ProductID,
                            ProductName = productDto.Value.ProductName,
                            UnitPrice = productDto.Value.UnitPrice,
                        };
                        return new Response<GetProductByIdDto>(
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
                return new Response<GetProductByIdDto>(
                    false,
                    HttpStatusCode.InternalServerError,
                    ResponseMessages.Error,
                    null);
            }
        }
        #endregion

        #region [- GetAllAsync() -]
        public async Task<IResponse<List<GetAllProductDto>>> GetAllAsync()
        {
            try
            {
                var products = await _productRepository.SelectAllAsync();
                if (!products.IsSuccessful || products.Value == null)
                {
                    return new Response<List<GetAllProductDto>>(
                        false,
                        HttpStatusCode.NotFound,
                        ResponseMessages.NullInput,
                        null);
                }
                else
                {
                    var result = products.Value.Select(p => new GetAllProductDto()
                    {
                        ProductID = p.ProductID,
                        ProductName = p.ProductName,
                        UnitPrice = p.UnitPrice,
                    }).ToList();
                    return new Response<List<GetAllProductDto>>(
                        true,
                        HttpStatusCode.OK,
                        ResponseMessages.SuccessfullOperation,
                        result);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new Response<List<GetAllProductDto>> (
                    false,
                    HttpStatusCode.InternalServerError,
                    ResponseMessages.Error,
                    null);
            }
        } 
        #endregion

    }
}