using Api.Ultils;
using AutoMapper;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Share.Models;
using Share.Models.Domain;
using System.CodeDom.Compiler;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        private readonly IUserService _servive;
        private readonly IMapper _mapper;

        public LoginController(IConfiguration config, IUserService servive, IMapper mapper)
        {
            _config = config;
            _servive = servive;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            var user = await AuthenticateAsync(userLogin);
            if (user != null)
            {
                var userModel = _mapper.Map<UserModel>(user);
                var token = Generated(userModel);
                return Ok(token);
            }
            return NotFound("User not found");
        }

        private string Generated(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Role, user.Role),
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"]
                , _config["Jwt:Audience"]
                , claims
                , expires: DateTime.Now.AddMilliseconds(15)
                , signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<User> AuthenticateAsync(UserLogin userLogin)
        {
            var data = await _servive.GetPageBySearchAsync(new Share.Models.Dtos.SearchDtos.UserSearchDto
            {
                IsEnable = true,
                Account = userLogin.UserName
            });

            if (data.Objects?.Any() == true)
            {
                var pass = data.Objects.First().Password;
                if (PasswordHasher.VerifyPassword(userLogin.Password, pass))
                {
                    return data.Objects.First();
                }
            }
            return null;
        }
    }
}
