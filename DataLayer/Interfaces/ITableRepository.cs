using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Share.Models.Domain;
using Share.Models.Dtos.SearchDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interfaces
{
    public interface ITableRepository : IRepository<Share.Models.Domain.Table>
    {
        Task<(List<Share.Models.Domain.Table>, int)> GetPageBySearchAsync(TableSearchDto tableSearchDto);
        Task<bool> UpdateAsync(Share.Models.Domain.Table model);
    }
}
