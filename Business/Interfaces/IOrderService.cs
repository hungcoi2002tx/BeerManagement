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
    public interface IOrderService
    {
        Task<ResponseCustom<Order>> GetAllBySearchAsync(OrderSearchDto obj);
        Task<ResponseCustom<Order>> AddAsync(OrderAddDto obj);
        Task<ResponseCustom<Order>> UpdateAsync(OrderEditDto obj);
        Task<ResponseCustom<Order>> DeleteAsync(int id);
        Task<ResponseCustom<Order>> GetPageBySearchAsync(OrderSearchDto obj);
    }
}
