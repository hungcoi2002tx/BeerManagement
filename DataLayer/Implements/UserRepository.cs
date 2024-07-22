using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
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
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(BeerManagementContext beerManagementContext) : base(beerManagementContext)
        {
        }
        public async Task<(List<User>, int)> GetPageBySearchAsync(UserSearchDto model)
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

        private IQueryable<User> GetQueryable(UserSearchDto model)
        {
            try
            {
                IQueryable<User> filter = _context.Users;
                if (model.Id != 0)
                {
                    filter = filter.Where(x => x.Id == model.Id);
                }
                if (model.Account.IsNotNullOrEmpty())
                {
                    filter = filter.Where(x => x.Account == model.Account);
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

        public async Task<bool> UpdateAsync(User model)
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

        public async Task<List<User>> GetAllBySearchAsync(UserSearchDto searchModel)
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
    }
}
