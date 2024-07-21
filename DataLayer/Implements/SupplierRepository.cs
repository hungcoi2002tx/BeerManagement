using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Share.Constant;
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
    public class SupplierRepository : Repository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(BeerManagementContext beerManagementContext) : base(beerManagementContext)
        {
        }



        public Task<List<Supplier>> GetAllBySearchAsync(SupplierSearchDto model)
        {
            throw new NotImplementedException();
        }

        private IQueryable<Supplier> GetQueryable(SupplierSearchDto model)
        {
            try
            {
                IQueryable<Supplier> filter = _context.Suppliers;

                if (model.Id != 0)
                {
                    filter = filter.Where(x => x.Id == model.Id);
                }
                if (model.SupplierName.IsNotNullOrEmpty())
                {
                    filter = filter.Where(x => x.SupplierName == model.SupplierName);
                }
                if (model.PhoneNumber.IsNotNullOrEmpty())
                {
                    filter = filter.Where(x => x.PhoneNumber == model.PhoneNumber);
                }
                if (model.IsEnable != null)
                {
                    filter = filter.Where(x => x.IsEnable == model.IsEnable);
                }

                return filter;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<(List<Supplier>,int)> GetPageBySearchAsync(SupplierSearchDto model)
        {
            try
            {
                var filter = GetQueryable(model);

                var count = await filter.CountAsync();

                if (model.Page != null && model.Page.PageIndex != 0)
                {
                    filter = filter.Skip(model.Page.PageSize * (model.Page.PageIndex-1))
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

        public async Task<bool> UpdateAsync(Supplier model)
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
