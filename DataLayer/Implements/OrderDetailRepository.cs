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
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(BeerManagementContext beerManagementContext) : base(beerManagementContext)
        {
        }

        public async Task<List<OrderDetail>> GetAllBySearchAsync(OrderDetailSearchDto searchModel)
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

        private IQueryable<OrderDetail> GetQueryable(OrderDetailSearchDto obj)
        {
            try
            {
                IQueryable<OrderDetail> filter = _context.OrderDetails;
                if (obj.OrderId != -1)
                {
                    filter = filter.Where(x => x.OrderId == obj.OrderId);
                }

                if (obj.ProductId != -1)
                {
                    filter = filter.Where(x => x.ProductId == obj.ProductId);
                }
                if (obj.GetProduct)
                {
                    filter = filter.Include(x => x.Product);
                }
                return filter;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<(List<OrderDetail>, int)> GetPageBySearchAsync(OrderDetailSearchDto obj)
        {
            try
            {
                var filter = GetQueryable(obj);

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

        public async Task<bool> UpdateAsync(OrderDetail obj)
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
