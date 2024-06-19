using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Share;
using Share.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Implements
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        private readonly BeerManagementContext _beerManagementContext;

        public Repository(BeerManagementContext beerManagementContext)
        {
            _beerManagementContext = beerManagementContext;
        }

        public async Task AddAsync(T obj)
        {
            await _beerManagementContext.Set<T>().AddAsync(obj);
        }

        public async Task<Boolean> DeleteAsync(int id)
        {
            var obj = await GetByIdAsync(id);
            if (obj == null)
            {
                return false;
            }
            _beerManagementContext.Set<T>().Remove(obj);
            return true;
        }

        private async Task<T> GetByIdAsync(int id)
        {
            return await _beerManagementContext.Set<T>().FindAsync(id);
        }

        public async Task EditAsync(T obj)
        {
            await _beerManagementContext.Set<T>().AddAsync(obj);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _beerManagementContext.Set<T>().ToListAsync();
        }
    }
}
