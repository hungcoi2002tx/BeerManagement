using Share.Models.Dtos.SearchDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interfaces
{
    public interface IOrderRepository : IRepository<Share.Models.Domain.Order>
    {
        Task<(List<Share.Models.Domain.Order>, int)> GetPageBySearchAsync(OrderSearchDto obj);
        Task<bool> UpdateAsync(Share.Models.Domain.Order model);
    }
}
