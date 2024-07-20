using Share.Models.Domain;
using Share.Models.Dtos.SearchDtos;
using Share.Models.ResponseObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ISupplierService
    {
        Task<ResponseCustom<Supplier>> GetAllAsync();
        Task<ResponseCustom<Supplier>> AddAsync(Supplier model);
        Task<ResponseCustom<Supplier>> UpdateAsync(Supplier model);
        Task<ResponseCustom<Supplier>> DeleteAsync(int id);
        Task<ResponseCustom<Supplier>> GetPageBySearchAsync(SupplierSearchDto model);
    }
}
