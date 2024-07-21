using Share.Models.Dtos.SearchDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interfaces
{
    public interface IOrderDetailRepository : IRepository<Share.Models.Domain.OrderDetail>
    {
        Task<(List<Share.Models.Domain.OrderDetail>, int)> GetPageBySearchAsync(OrderDetailSearchDto obj);
        Task<bool> UpdateAsync(Share.Models.Domain.OrderDetail obj);
    }
}
