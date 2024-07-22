using Share.Models.Domain;
using Share.Models.Dtos.SearchDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> UpdateAsync(User model);
        Task<(List<User>, int)> GetPageBySearchAsync(UserSearchDto model);
        Task<List<User>> GetAllBySearchAsync(UserSearchDto model);
    }
}
