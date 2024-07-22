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
    public interface IOrderDetailService
    {
        Task<ResponseCustom<OrderDetail>> GetAllBySearchAsync(OrderDetailSearchDto obj);
        Task<ResponseCustom<OrderDetail>> AddAsync(List<OrderDetailAddDto> list);
        Task<ResponseCustom<OrderDetail>> UpdateAsync(List<OrderDetailEditDto> obj);
        Task<ResponseCustom<OrderDetail>> DeleteAsync(int orderId, int productId);
        Task<ResponseCustom<OrderDetail>> GetPageBySearchAsync(OrderDetailSearchDto obj);
    }
}
