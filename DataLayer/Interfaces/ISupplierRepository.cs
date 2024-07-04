using Share.Constant;
using Share.Models.Domain;
using Share.Models.SearchModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interfaces
{
    public interface ISupplierRepository : IRepository<Supplier>
    {
        Task<bool> UpdateAsync(Supplier model);
        Task<(List<Supplier>,int )> GetPageBySearchAsync(SupplierSearchModel model);
    }
}
