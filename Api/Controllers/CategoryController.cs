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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _servive;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _servive = categoryService;
            _mapper = mapper;
        }

        [HttpPost("GetAll")]
        public async Task<ResponseCustom<Category>> GetAllAsync([FromBody] CategorySearchDto SearchModel)
        {
            try
            {
                var result = await _servive.GetAllBySearchAsync(SearchModel);

                return result;
            }
            catch (Exception ex)
            {
                return ResponeExtentions<Category>.GetError500(ex.Message);
            }
        }

        [HttpPost("Add")]
        public async Task<ResponseCustom<Category>> AddAsync([FromBody] CategoryEditDto editModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ResponeExtentions<Category>.GetError400("Validate AddAsync - SupperlierController");
                }

                var result = await _servive.AddAsync(_mapper.Map<Category>(editModel));
                return result;
            }
            catch (Exception ex)
            {
                return ResponeExtentions<Category>.GetError500(ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ResponseCustom<Category>> DeleteAsync(int id)
        {
            try
            {
                var result = await _servive.DeleteAsync(id);
                return result;
            }
            catch (Exception ex)
            {
                return ResponeExtentions<Category>.GetError500(ex.Message);
            }
        }

        [HttpPut("Update")]
        public async Task<ResponseCustom<Category>> UpdateAsync([FromBody] CategoryEditDto editModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ResponeExtentions<Category>.GetError400("Validate UpdateAsync - SupperlierController");
                }
                var result = await _servive.UpdateAsync(_mapper.Map<Category>(editModel));
                return result;
            }
            catch (Exception ex)
            {
                return ResponeExtentions<Category>.GetError500(ex.Message);
            }
        }

        [HttpPost("GetPage")]
        public async Task<ResponseCustom<Category>> GetPageAsync([FromBody] CategorySearchDto search)
        {
            try
            {
                var result = await _servive.GetPageBySearchAsync(search);
                return result;
            }
            catch (Exception ex)
            {
                return ResponeExtentions<Category>.GetError500(ex.Message);
            }
        }
    }
}
