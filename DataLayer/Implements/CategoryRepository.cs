using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Share;
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
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(BeerManagementContext beerManagementContext) : base(beerManagementContext)
        {
        }

        public async Task<(List<Category>, int)> GetPageBySearchAsync(CategorySearchDto model)
        {
            try
            {
                IQueryable<Category> filter = _context.Categories;

                if (model.Id != 0)
                {
                    filter = filter.Where(x => x.Id == model.Id);
                }
                if (model.Name.IsNotNullOrEmpty())
                {
                    filter = filter.Where(x => x.Name == model.Name);
                }
                var count = await filter.CountAsync();
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

        public async Task<bool> UpdateAsync(Category model)
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
    }
}
