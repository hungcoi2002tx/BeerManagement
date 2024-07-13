using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Share.Models.Domain;
using Share.Models.SearchModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Implements
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(BeerManagementContext context) : base(context)
        {
            
        }

        public async Task<(List<Product>, int)> GetPageBySearchAsync(ProductSearchModel model)
        {
            try
            {
                IQueryable<Product> filter = _context.Products;
                if(model.Id != 0)
                {
                    filter = filter.Where(x => x.Id == model.Id);
                }
                if(model.Name != null)
                {
                    filter = filter.Where(x => x.Name == model.Name);
                }
                int count = await filter.CountAsync();
                if (model.Page != null && model.Page.PageIndex != 0)
                {
                    filter = filter.Skip(model.Page.PageSize * (model.Page.PageIndex - 1))
                        .Take(model.Page.PageSize);
                }
                var data = await filter.ToListAsync();
                return (data, count);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Product model)
        {
            try
            {
                OpenTransaction();
                _context.Attach(model).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                await CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                await RollBackTransactionAsync();
                throw;
            }
        }
    }
}
