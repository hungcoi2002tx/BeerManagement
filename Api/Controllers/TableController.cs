using AutoMapper;
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
    public class TableController : ControllerBase
    {
        private readonly ITableService _tableService;

        public TableController(ITableService tableService)
        {
            _tableService = tableService;
        }

        [HttpPost("GetAll")]
        public async Task<ResponseCustom<Table>> GetAllBySearchAsync([FromBody] TableSearchDto search)
        {
            try
            {
                var result = await _tableService.GetAllBySearchAsync(search);
                return result;
            }
            catch (Exception)
            {
                return ResponeExtentions<Table>.GetError500("Error when get all table");
            }
        }

        [HttpPost("GetPage")]
        public async Task<ResponseCustom<Table>> GetPageAsync([FromBody] TableSearchDto search)
        {
            try
            {
                var result = await _tableService.GetPageBySearchAsync(search);
                return result;
            }
            catch (Exception)
            {
                return ResponeExtentions<Table>.GetError500("Error when get page");
            }
        }

        [HttpPost("Add")]
        public async Task<ResponseCustom<Table>> AddAsync([FromBody] TableAddDto obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ResponeExtentions<Table>.GetError400("Validate AddAsync - TableController");
                }

                var result = await _tableService.AddAsync(obj);
                return result;
            }
            catch (Exception)
            {
                return ResponeExtentions<Table>.GetError500("Error when add table");
            }
        }

        [HttpPut("Update")]
        public async Task<ResponseCustom<Table>> UpdateAsync([FromBody] TableEditDto obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ResponeExtentions<Table>.GetError400("Validate UpdateAsync - TableController");
                }

                var result = await _tableService.UpdateAsync(obj);
                return result;
            }
            catch (Exception)
            {
                return ResponeExtentions<Table>.GetError500("Error when update table");
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ResponseCustom<Table>> DeleteAsync(int id)
        {
            try
            {
                var result = await _tableService.DeleteAsync(id);
                return result;
            }
            catch (Exception)
            {
                return ResponeExtentions<Table>.GetError500("Error when delete table");
            }
        }
    }
}
