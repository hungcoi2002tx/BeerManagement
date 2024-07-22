using AutoMapper;
using Business.Implements;
using Business.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Share.Models.Domain;
using Share.Models.Dtos.EditDtos;
using Share.Models.Dtos.SearchDtos;
using Share.Models.ResponseObject;
using Share.Ultils;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryImportController : ControllerBase
    {
        private readonly IHistoryImportService _servive;
        private readonly IMapper _mapper;

        public HistoryImportController(IHistoryImportService service, IMapper mapper)
        {
            _servive = service;
            _mapper = mapper;
        }


		[HttpPost("GetAll")]
		public async Task<ResponseCustom<ImportHistory>> GetAllBySearchAsync([FromBody] ImportHistorySearchDto search)
		{
			try
			{
				var result = await _servive.GetAllBySearchAsync(search);
				return result;
			}
			catch (Exception ex)
			{
				return ResponeExtentions<ImportHistory>.GetError500(ex.Message);
			}
		}

		[HttpPost("Add")]
        public async Task<ResponseCustom<Share.Models.Domain.ImportHistory>> AddAsync([FromBody] ImportHistoryEditDto editModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ResponeExtentions<Share.Models.Domain.ImportHistory>.GetError400("Validate AddAsync - HistoryImportController");
                }

                var result = await _servive.AddAsync(editModel);
                return result;
            }
            catch (Exception ex)
            {
                return ResponeExtentions<Share.Models.Domain.ImportHistory>.GetError500(ex.Message);
            }
        }

        [HttpPost("GetPage")]
        public async Task<ResponseCustom<ImportHistory>> GetPageAsync([FromBody] ImportHistorySearchDto search)
        {
            try
            {
                var result = await _servive.GetPageBySearchAsync(search);
                return result;
            }
            catch (Exception ex)
            {
                return ResponeExtentions<ImportHistory>.GetError500(ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ResponseCustom<ImportHistory>> DeleteAsync(int id)
        {
            try
            {
                var result = await _servive.DeleteAsync(id);
                return result;
            }
            catch (Exception ex)
            {
                return ResponeExtentions<ImportHistory>.GetError500(ex.Message);
            }
        }
    }
}
