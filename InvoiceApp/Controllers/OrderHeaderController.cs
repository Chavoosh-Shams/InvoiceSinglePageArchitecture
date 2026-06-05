using Microsoft.AspNetCore.Mvc;
using InvoiceApp.ApplicationServices.Services.Contracts;
using InvoiceApp.ApplicationServices.Dtos.OrderHeaderDtos;

namespace InvoiceApp.Controllers
{
    public class OrderHeaderController : Controller
    {
        #region [- PrivateField -]
        private readonly IOrderHeaderApplicationService _orderHeaderApplicationService; 
        #endregion

        #region [- Ctor -]
        public OrderHeaderController(IOrderHeaderApplicationService orderHeaderApplicationService)
        {
            _orderHeaderApplicationService = orderHeaderApplicationService;
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
        public async Task<IActionResult> Post([FromBody] PostOrderHeaderDto postOrderHeaderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _orderHeaderApplicationService.PostAsync(postOrderHeaderDto);

            if (!result.IsSuccessful)
                return BadRequest(result.Message);

            var response = result.Value;

            return Ok(response);
        }
        #endregion

        #region [- Put() -]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] PutOrderHeaderDto putOrderHeaderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _orderHeaderApplicationService.PutAsync(putOrderHeaderDto);

            if (!result.IsSuccessful)
                return BadRequest(result.Message);

            var response = result.Value;

            return Ok(response);
        }
        #endregion

        #region [- Delete() -]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteOrderHeaderDto deleteOrderHeaderDto)
        {
            if (deleteOrderHeaderDto.OrderHeaderID == Guid.Empty)
            {
                return BadRequest("Null CustomerID!");
            }

            var result = await _orderHeaderApplicationService.DeleteAsync(deleteOrderHeaderDto);

            if (!result.IsSuccessful)
                return BadRequest(result.Message);

            var response = result.Value;

            return Ok(response);
        }
        #endregion

        #region [- GetById() -]
        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] GetOrderHeaderByIdDto getOrderHeaderByIdDto)
        {
                
            if (getOrderHeaderByIdDto.OrderHeaderID == Guid.Empty)
            {
                return BadRequest("Null OrderHeaderID");
            }

            var result = await _orderHeaderApplicationService.GetByIdAsync(getOrderHeaderByIdDto);

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
            var result = await _orderHeaderApplicationService.GetAllAsync();

            var response = result.Value;

            return Ok(response);
        }
        #endregion
    }
}
