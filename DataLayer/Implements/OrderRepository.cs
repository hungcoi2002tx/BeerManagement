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
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(BeerManagementContext beerManagementContext) : base(beerManagementContext)
        {
        }

        public async Task<(List<Order>, int)> GetPageBySearchAsync(OrderSearchDto obj)
        {
            try
            {
                IQueryable<Order> filter = _context.Orders;

                if (obj.Id != 0)
                {
                    filter = filter.Where(x => x.Id == obj.Id);
                }
                if (obj.Date != null)
                {
                    filter = filter.Where(x => x.Date == obj.Date);
                }
                if (obj.PaymentDate != null)
                {
                    filter = filter.Where(x => x.PaymentDate == obj.PaymentDate);
                }
                if (obj.PaymentStatus != -1)
                {
                    filter = filter.Where(x => x.PaymentStatus == obj.PaymentStatus);
                }
                if (obj.TableId != -1)
                {
                    filter = filter.Where(x => x.TableId == obj.TableId);
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

        public Task<bool> UpdateAsync(Order model)
        {
            throw new NotImplementedException();
        }
    }
}
