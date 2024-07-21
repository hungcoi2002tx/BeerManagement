using Share.Models.Domain;
using Share.Models.Dtos.SearchDtos;
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
        Task<(List<Product>, int)> GetPageBySearchAsync(ProductSearchDto model);
    }
}
