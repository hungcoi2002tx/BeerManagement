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
    public interface IHistoryImportService
    {
        Task<ResponseCustom<ImportHistory>> GetAllBySearchAsync(ImportHistorySearchDto model);
        Task<ResponseCustom<ImportHistory>> AddAsync(ImportHistoryEditDto model);
        Task<ResponseCustom<ImportHistory>> DeleteAsync(int id);
        Task<ResponseCustom<ImportHistory>> GetPageBySearchAsync(ImportHistorySearchDto model);
    }
}
