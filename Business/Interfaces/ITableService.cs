using Share.Models.Domain;
using Share.Models.Dtos.AddDtos;
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
    public interface ITableService
    {
        Task<ResponseCustom<Table>> GetAllBySearchAsync(TableSearchDto model);
        Task<ResponseCustom<Table>> AddAsync(TableAddDto obj);
        Task<ResponseCustom<Table>> UpdateAsync(TableEditDto obj);
        Task<ResponseCustom<Table>> DeleteAsync(int id);
        Task<ResponseCustom<Table>> GetPageBySearchAsync(TableSearchDto obj);
    }
}
