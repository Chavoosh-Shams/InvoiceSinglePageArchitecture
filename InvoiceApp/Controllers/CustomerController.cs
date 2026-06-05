using Microsoft.AspNetCore.Mvc;
using InvoiceApp.ApplicationServices.Dtos.CustomerDtos;
using InvoiceApp.ApplicationServices.Services.Contracts;

namespace InvoiceApp.Controllers
{
    public class CustomerController : Controller
    {
        #region [- PrivateField -]
        private readonly ICustomerApplicationService _customerApplicationService;
        #endregion

        #region [- Ctor -]
        public CustomerController(ICustomerApplicationService customerApplicationService)
        {
            _customerApplicationService = customerApplicationService;
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
        public async Task<IActionResult> Post([FromBody] PostCustomerDto postCustomerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _customerApplicationService.PostAsync(postCustomerDto);

            if (!result.IsSuccessful)
                return BadRequest(result.Message);

            var response = result.Value;

            return Ok(response);
        }
        #endregion

        #region [- Put() -]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] PutCustomerDto putCustomerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _customerApplicationService.PutAsync(putCustomerDto);

            if (!result.IsSuccessful)
                return BadRequest(result.Message);

            var response = result.Value;

            return Ok(response);
        }
        #endregion

        #region [- Delete() -]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteCustomerDto deleteCustomerDto)
        {
            if (deleteCustomerDto.CustomerID == Guid.Empty)
            {
                return BadRequest("Null CustomerID!");
            }

            var result = await _customerApplicationService.DeleteAsync(deleteCustomerDto);

            if (!result.IsSuccessful)
                return BadRequest(result.Message);

            var response = result.Value;

            return Ok(response);
        }
        #endregion

        #region [- GetById() -]
        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] GetCustomerByIdDto getCustomerByIdDto)
        {
            if (getCustomerByIdDto.CustomerID == Guid.Empty)
            {
                return BadRequest("Null CustomerID");
            }
            var result = await _customerApplicationService.GetByIdAsync(getCustomerByIdDto);

            if (!result.IsSuccessful)
                return BadRequest(result.Message);

            var response = result.Value;

            return Ok(response);
        }
        #endregion

        #region [- GetAll() -]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _customerApplicationService.GetAllAsync();

            var response = result.Value;

            return Ok(response);
        } 
        #endregion
    }
}
