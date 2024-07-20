using AutoMapper;
using Business.Implements;
using Business.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Share.Models.Domain;
using Share.Ultils;
using Share;
using Share.Models.Dtos.EditDtos;
using Share.Models.Dtos.SearchDtos;
using Share.Models.ResponseObject;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        private readonly IMapper _mapper;

        public SupplierController(ISupplierService supplierService, IMapper mapper)
        {
            _supplierService = supplierService;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<ResponseCustom<Supplier>> GetAllAsync()
        {
            try
            {
                var result = await _supplierService.GetAllAsync();
                return result;
            }
            catch (Exception ex)
            {
                return ResponeExtentions<Supplier>.GetError500(ex.Message);
            }
        }

        [HttpPost("GetPage")]
        public async Task<ResponseCustom<Supplier>> GetPageAsync([FromBody] SupplierSearchDto search)
        {
            try
            {
                var result = await _supplierService.GetPageBySearchAsync(search);
                return result;
            }
            catch (Exception ex)
            {
                return ResponeExtentions<Supplier>.GetError500(ex.Message);
            }
        }

        [HttpPost("Add")]
        public async Task<ResponseCustom<Supplier>> AddAsync([FromBody] SupplierEditDto editModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ResponeExtentions<Supplier>.GetError400("Validate AddAsync - SupperlierController");
                }
                var result = await _supplierService.AddAsync(_mapper.Map<Supplier>(editModel));
                return result;
            }
            catch (Exception ex)
            {
                return ResponeExtentions<Supplier>.GetError500(ex.Message);
            }
        }

        [HttpPut("Update")]
        public async Task<ResponseCustom<Supplier>> UpdateAsync([FromBody] SupplierEditDto editModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ResponeExtentions<Supplier>.GetError400("Validate UpdateAsync - SupperlierController");
                }
                var result = await _supplierService.UpdateAsync(_mapper.Map<Supplier>(editModel));
                return result;
            }
            catch (Exception ex)
            {
                return ResponeExtentions<Supplier>.GetError500(ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ResponseCustom<Supplier>> DeleteAsync(int id)
        {
            try
            {
                var result = await _supplierService.DeleteAsync(id);
                return result;
            }
            catch (Exception ex)
            {
                return ResponeExtentions<Supplier>.GetError500(ex.Message);
            }
        }
    }
}
