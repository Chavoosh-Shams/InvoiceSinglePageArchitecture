
using InvoiceApp.ApplicationServices.Dtos.ProductDtos;

using InvoiceApp.ApplicationServices.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceApp.Controllers
{
    public class ProductController : Controller
    {
        #region [- PrivateField -]
        private readonly IProductApplicationService _productApplicationService;
        #endregion

        #region [- Ctor -]
        public ProductController(IProductApplicationService productApplicationService)
        {
            _productApplicationService = productApplicationService;
        }
        #endregion

        #region [- Index() -]
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region [- Post() -]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostProductDto postProductDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _productApplicationService.PostAsync(postProductDto);

            if (!result.IsSuccessful)
                return BadRequest(result.Message);

            return Ok(result.Value);
        }
        #endregion

        #region [- Put() -]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] PutProductDto putProductDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _productApplicationService.PutAsync(putProductDto);

            if (!result.IsSuccessful)
                return BadRequest(result.Message);

            return Ok(result.Value);
        }
        #endregion

        #region [- Delete() -]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteProductDto deleteProductDto)
        {
            if (deleteProductDto.ProductID == Guid.Empty)
            {
                return BadRequest("Null ProductID!");
            }

            var result = await _productApplicationService.DeleteAsync(deleteProductDto);

            if (!result.IsSuccessful)
                return BadRequest(result.Message);

            return Ok(result.Value);
        }
        #endregion

        #region [- GetById() -]
        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] GetProductByIdDto getProductByIdDto)
        {
            if (getProductByIdDto.ProductID == Guid.Empty)
            {
                return BadRequest("Null ProductID!");
            }
            var customer = await _productApplicationService.GetByIdAsync(getProductByIdDto);

            if (!customer.IsSuccessful)
                return BadRequest(customer.Message);

            var response = customer.Value;

            return Ok(response);
        }
        #endregion

        #region [- GetAll() -]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productApplicationService.GetAllAsync();
            return Ok(products.Value);
        } 
        #endregion
    }
}
