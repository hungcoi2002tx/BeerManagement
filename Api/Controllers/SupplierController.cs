using AutoMapper;
using Business.Implements;
using Business.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Share.Models.Domain;
using Share.Models.EditModels;
using Share.Ultils;
using Share;
using Share.Models.SearchModels;

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
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var list = await _supplierService.GetAllAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("GetPage")]
        public async Task<IActionResult> GetPageAsync([FromBody] SupplierSearchModel search)
        {
            try
            {
                var list = await _supplierService.GetPageBySearchAsync(search);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddAsync([FromBody] SupplierEditModel editModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Loi validate hoac binding");
                }
                var result = await _supplierService.AddAsync(_mapper.Map<Supplier>(editModel));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.GetMesssageError("Supplier - AddAsync"));
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync([FromBody] SupplierEditModel editModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Loi validate hoac binding");
                }
                var result = await _supplierService.UpdateAsync(_mapper.Map<Supplier>(editModel));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.GetMesssageError("Supplier - UpdateAsync"));
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var model = await _supplierService.GetPageBySearchAsync(new SupplierSearchModel()
                {
                    Id = id
                });
                if(model == null)
                {
                    return NotFound("Not Exist");
                }
                var result = await _supplierService.DeleteAsync(model.Objects.First());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message.GetMesssageError("Supplier - UpdateAsync"));
            }
        }
    }
}
