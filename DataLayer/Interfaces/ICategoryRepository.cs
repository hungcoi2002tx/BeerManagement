using Share.Models.Domain;
using Share.Models.Dtos.SearchDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<bool> UpdateAsync(Category model);
        Task<(List<Category>, int)> GetPageBySearchAsync(CategorySearchDto model);
    }
}
