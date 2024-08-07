﻿using Api.CustomAttribute;
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
        [CustomAuthorize("Admin", "Manager", "Staff")]

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
        [CustomAuthorize("Admin", "Manager", "Staff")]

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
        [CustomAuthorize("Admin", "Manager", "Staff")]

        public async Task<ResponseCustom<OrderDetail>> AddAsync([FromBody] List<OrderDetailAddDto> list)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ResponeExtentions<OrderDetail>.GetError400("Validate AddAsync - orderDetailController");
                }

                var result = await _orderDetailService.AddAsync(list);
                return result;
            }
            catch (Exception)
            {
                return ResponeExtentions<OrderDetail>.GetError500("Error when add order detail");
            }
        }

        [HttpPut("Update")]
        [CustomAuthorize("Admin", "Manager", "Staff")]

        public async Task<ResponseCustom<OrderDetail>> UpdateAsync([FromBody] List<OrderDetailEditDto> obj)
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
        [CustomAuthorize("Admin")]

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
