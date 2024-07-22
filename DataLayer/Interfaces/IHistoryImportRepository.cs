using Microsoft.EntityFrameworkCore.Migrations;
using Share.Models.Domain;
using Share.Models.Dtos.SearchDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interfaces
{
    public interface IHistoryImportRepository : IRepository<ImportHistory>
    {
        Task<bool> UpdateAsync(ImportHistory model);
        Task<(List<ImportHistory>, int)> GetPageBySearchAsync(ImportHistorySearchDto model);
        Task<List<ImportHistory>> GetAllBySearchAsync(ImportHistorySearchDto model);
    }
}
