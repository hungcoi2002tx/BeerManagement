using Business.Implements;
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
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        [HttpPost("GetAll")]
        public async Task<ResponseCustom<OrderDetail>> GetAllBySearchAsync([FromBody] OrderDetailSearchDto search)
        {
            try
            {
                var result = await _orderDetailService.GetAllBySearchAsync(search);
                return result;
            }
            catch (Exception)
            {
                return ResponeExtentions<OrderDetail>.GetError500("Error when get all order detail");
            }
        }

        [HttpPost("GetPage")]
        public async Task<ResponseCustom<OrderDetail>> GetPageAsync([FromBody] OrderDetailSearchDto search)
        {
            try
            {
                var result = await _orderDetailService.GetPageBySearchAsync(search);
                return result;
            }
            catch (Exception)
            {
                return ResponeExtentions<OrderDetail>.GetError500("Error when get page order detail");
            }
        }

        [HttpPost("Add")]
        public async Task<ResponseCustom<OrderDetail>> AddAsync([FromBody] OrderDetailAddDto obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ResponeExtentions<OrderDetail>.GetError400("Validate AddAsync - orderDetailController");
                }

                var result = await _orderDetailService.AddAsync(obj);
                return result;
            }
            catch (Exception)
            {
                return ResponeExtentions<OrderDetail>.GetError500("Error when add order detail");
            }
        }

        [HttpPut("Update")]
        public async Task<ResponseCustom<OrderDetail>> UpdateAsync([FromBody] OrderDetailEditDto obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ResponeExtentions<OrderDetail>.GetError400("Validate UpdateAsync - orderDetailController");
                }

                var result = await _orderDetailService.UpdateAsync(obj);
                return result;
            }
            catch (Exception)
            {
                return ResponeExtentions<OrderDetail>.GetError500("Error when update order detail");
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ResponseCustom<OrderDetail>> DeleteAsync(int orderId, int productId)
        {
            try
            {
                var result = await _orderDetailService.DeleteAsync(orderId, productId);
                return result;
            }
            catch (Exception)
            {
                return ResponeExtentions<OrderDetail>.GetError500("Error when delete order detail");
            }
        }
    }
}
