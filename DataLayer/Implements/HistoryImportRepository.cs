using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Share.Models.Domain;
using Share.Models.Dtos.SearchDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Implements
{
    public class HistoryImportRepository : Repository<ImportHistory>, IHistoryImportRepository
    {
        public HistoryImportRepository(BeerManagementContext beerManagementContext) : base(beerManagementContext)
        {
        }

        public async Task<(List<ImportHistory>, int)> GetPageBySearchAsync(ImportHistorySearchDto model)
        {
            try
            {
                var filter = GetQueryable(model);
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

        private IQueryable<ImportHistory> GetQueryable(ImportHistorySearchDto model)
        {
            try
            {
                IQueryable<ImportHistory> filter = _context.ImportHistories;
                if(model.IsInclueProduct == true)
                {
                    filter = filter.Include(x => x.Product);    
                }
                filter = filter.OrderByDescending(x => x.Date);
                return filter;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<ImportHistory>> GetAllBySearchAsync(ImportHistorySearchDto searchModel)
        {
            try
            {
                var filter = GetQueryable(searchModel);
                var data = await filter.ToListAsync();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateAsync(ImportHistory model)
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
