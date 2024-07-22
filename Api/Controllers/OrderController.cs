using Business.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Share.Models.Domain;
using Share.Models.Dtos.AddDtos;
using Share.Models.Dtos.EditDtos;
using Share.Models.Dtos.SearchDtos;
using Share.Models.ResponseObject;
using Share.Ultils;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("GetAll")]
        public async Task<ResponseCustom<Order>> GetAllBySearchAsync([FromBody] OrderSearchDto search)
        {
            try
            {
                var result = await _orderService.GetAllBySearchAsync(search);
                return result;
            }
            catch (Exception)
            {
                return ResponeExtentions<Order>.GetError500("Error when get all order");
            }
        }

        [HttpPost("GetPage")]
        public async Task<ResponseCustom<Order>> GetPageAsync([FromBody] OrderSearchDto search)
        {
            try
            {
                var result = await _orderService.GetPageBySearchAsync(search);
                return result;
            }
            catch (Exception)
            {
                return ResponeExtentions<Order>.GetError500("Error when get page order");
            }
        }

        [HttpPost("Add")]
        public async Task<ResponseCustom<Order>> AddAsync([FromBody] OrderAddDto obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ResponeExtentions<Order>.GetError400("Validate AddAsync - orderController");
                }

                var result = await _orderService.AddAsync(obj);
                return result;
            }
            catch (Exception)
            {
                return ResponeExtentions<Order>.GetError500("Error when add order");
            }
        }

        [HttpPut("Update")]
        public async Task<ResponseCustom<Order>> UpdateAsync([FromBody] OrderEditDto obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ResponeExtentions<Order>.GetError400("Validate UpdateAsync - OrderController");
                }

                var result = await _orderService.UpdateAsync(obj);
                return result;
            }
            catch (Exception)
            {
                return ResponeExtentions<Order>.GetError500("Error when update order");
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ResponseCustom<Order>> DeleteAsync(int id)
        {
            try
            {
                var result = await _orderService.DeleteAsync(id);
                return result;
            }
            catch (Exception)
            {
                return ResponeExtentions<Order>.GetError500("Error when delete order");
            }
        }
    }
}
