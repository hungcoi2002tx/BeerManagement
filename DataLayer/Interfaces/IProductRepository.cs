using Share.Models.Domain;
using Share.Models.SearchModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<bool> UpdateAsync(Product model);
        Task<(List<Product>, int)> GetPageBySearchAsync(ProductSearchModel model);
        Task<bool> SoftDeleteAsync(Product model);
    }
}
