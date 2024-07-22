using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Share.Models.Domain;
using Share.Models.Dtos.SearchDtos;
using Share.Ultils;
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
        private IQueryable<Product> GetQueryable(ProductSearchDto model)
        {
            try
            {
                IQueryable<Product> filter = _context.Products;

                if (model.IsEnable != null)
                {
                    filter = filter.Where(x => x.IsEnable == model.IsEnable);
                }
                if (model.Id != 0)
                {
                    filter = filter.Where(x => x.Id == model.Id);
                }
                if (model.Name != null)
                {
                    filter = filter.Where(x => x.Name == model.Name);
                }
                if (model.IsForSell != null)
                {
                    filter = filter.Where(x => x.ForSell == model.IsForSell);
                }
                if(model.Ids?.Any() == true)
                {
					filter = filter.Where(x => model.Ids.Contains(x.Id));
				}
                return filter;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<(List<Product>, int)> GetPageBySearchAsync(ProductSearchDto model)
        {
            try
            {
                var filter = GetQueryable(model);

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
            catch (Exception)
            {
                await RollBackTransactionAsync();
                throw;
            }
        }

        public async Task<List<Product>> GetAllBySearchAsync(ProductSearchDto searchModel)
        {
            try
            {
                var filter = GetQueryable(searchModel);
                var data = await filter.ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
