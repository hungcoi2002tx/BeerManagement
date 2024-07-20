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
    public class TableRepository : Repository<Table>, ITableRepository
    {
        public TableRepository(BeerManagementContext beerManagementContext) : base(beerManagementContext)
        {
        }

        public async Task<(List<Share.Models.Domain.Table>, int)> GetPageBySearchAsync(TableSearchDto obj)
        {
            try
            {
                IQueryable<Table> filter = _context.Tables;

                if (obj.Id != 0)
                {
                    filter = filter.Where(x => x.Id == obj.Id);
                }
                if (obj.Number != 0)
                {
                    filter = filter.Where(x => x.Number == obj.Number);
                }
                if (obj.Status != -1)
                {
                    filter = filter.Where(x => x.Status == obj.Status);
                }

                var count = await filter.CountAsync();

                if (obj.Page != null && obj.Page.PageIndex != 0)
                {
                    filter = filter.Skip(obj.Page.PageSize * (obj.Page.PageIndex - 1))
                        .Take(obj.Page.PageSize);
                }

                var data = await filter.ToListAsync();

                return (data, count);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Table obj)
        {
            try
            {
                OpenTransaction();

                _context.Attach(obj).State = EntityState.Modified;
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
