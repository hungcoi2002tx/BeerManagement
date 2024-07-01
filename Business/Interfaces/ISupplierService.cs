using Share.Constant;
using Share.Models.Domain;
using Share.Models.SearchModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ISupplierService
    {
        Task<List<Supplier>> GetAllAsync();
        Task<ExecuteRespone<Supplier>> AddAsync(Supplier model);
        Task<ExecuteRespone<Supplier>> UpdateAsync(Supplier model);
        Task<ExecuteRespone<Supplier>> DeleteAsync(Supplier model);
        Task<List<Supplier>> GetPageBySearchAsync(SupplierSearchModel model);
    }
}
