using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Share.Models.Domain;
using Share.Models.Dtos.SearchDtos;
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

        public async Task<(List<Product>, int)> GetPageBySearchAsync(ProductSearchDto model)
        {
            try
            {
                IQueryable<Product> filter = _context.Products;

                if(model.IsEnableOnly) 
                { 
                    filter = filter.Where(x => x.IsEnable == true); 
                }
                if(model.Id != 0)
                {
                    filter = filter.Where(x => x.Id == model.Id);
                }
                if(model.Name != null)
                {
                    filter = filter.Where(x => x.Name == model.Name);
                }

                filter = filter.OrderBy(x => x.Id);
                int count = await filter.CountAsync();

                if (model.Page != null && model.Page.PageIndex != 0)
                {
                    filter = filter.Skip(model.Page.PageSize * (model.Page.PageIndex - 1))
                        .Take(model.Page.PageSize);
                }

                if(model.IsIncludeCategory)
                {
                    filter = filter.Include(x => x.Category);
                }

                if(model.IsIncludeSupplier)
                {
                    filter = filter.Include(x => x.Supplier);
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
