using Share.Models.Domain;
using Share.Models.Dtos.SearchDtos;
using Share.Models.ResponseObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IUserService
    {
        Task<ResponseCustom<User>> GetAllBySearchAsync(UserSearchDto model);
        Task<ResponseCustom<User>> AddAsync(User model);
        Task<ResponseCustom<User>> UpdateAsync(User model);
        Task<ResponseCustom<User>> DeleteAsync(int id);
        Task<ResponseCustom<User>> GetPageBySearchAsync(UserSearchDto model);
    }
}
