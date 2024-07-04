using Share.Models.Domain;
using Share.Models.SearchModels;
using Share.Ultils;
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
        Task<ExecuteRespone<Supplier>> GetPageBySearchAsync(SupplierSearchModel model);
    }
}
