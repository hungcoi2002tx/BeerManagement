using Api.CustomAttribute;
using Api.Ultils;
using AutoMapper;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
    public class UserController : ControllerBase
    {
        private readonly IUserService _servive;
        private readonly IMapper _mapper;

        public UserController(IUserService service, IMapper mapper)
        {
            _servive = service;
            _mapper = mapper;
        }

        [HttpPost("GetAll")]
		[CustomAuthorize("Admin", "Manager")]
		public async Task<ResponseCustom<User>> GetAllAsync([FromBody] UserSearchDto SearchModel)
        {
            try
            {
                var result = await _servive.GetAllBySearchAsync(SearchModel);

                return result;
            }
            catch (Exception ex)
            {
                return ResponeExtentions<User>.GetError500(ex.Message);
            }
        }

        [HttpPost("Add")]
		[CustomAuthorize("Admin", "Manager")]
		public async Task<ResponseCustom<User>> AddAsync([FromBody] UserEditDto editModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ResponeExtentions<User>.GetError400("Validate AddAsync - UserController");
                }
                editModel.Password = PasswordHasher.HashPassword(editModel.Password);
                var result = await _servive.AddAsync(_mapper.Map<User>(editModel));
                return result;
            }
            catch (Exception ex)
            {
                return ResponeExtentions<User>.GetError500(ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
		[CustomAuthorize("Admin", "Manager")]
		public async Task<ResponseCustom<User>> DeleteAsync(int id)
        {
            try
            {
                var result = await _servive.DeleteAsync(id);
                return result;
            }
            catch (Exception ex)
            {
                return ResponeExtentions<User>.GetError500(ex.Message);
            }
        }

        [HttpPut("Update")]
		[CustomAuthorize("Admin", "Manager")]
		public async Task<ResponseCustom<User>> UpdateAsync([FromBody] UserEditDto editModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ResponeExtentions<User>.GetError400("Validate UpdateAsync - UserController");
                }
                var result = await _servive.UpdateAsync(_mapper.Map<User>(editModel));
                return result;
            }
            catch (Exception ex)
            {
                return ResponeExtentions<User>.GetError500(ex.Message);
            }
        }

        [HttpPost("GetPage")]
		[CustomAuthorize("Admin", "Manager", "Staff")]
		public async Task<ResponseCustom<User>> GetPageAsync([FromBody] UserSearchDto search)
        {
            try
            {
                var result = await _servive.GetPageBySearchAsync(search);
                return result;
            }
            catch (Exception ex)
            {
                return ResponeExtentions<User>.GetError500(ex.Message);
            }
        }
    }
}
