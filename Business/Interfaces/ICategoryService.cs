using Share.Models.Domain;
using Share.Models.Dtos.EditDtos;
using Share.Models.Dtos.SearchDtos;
using Share.Models.ResponseObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ICategoryService
    {
        Task<ResponseCustom<Category>> GetAllBySearchAsync(CategorySearchDto model);
        Task<ResponseCustom<Category>> AddAsync(Category model);
        Task<ResponseCustom<Category>> UpdateAsync(Category model);
        Task<ResponseCustom<Category>> DeleteAsync(int id);
        Task<ResponseCustom<Category>> GetPageBySearchAsync(CategorySearchDto model);
    }
}
