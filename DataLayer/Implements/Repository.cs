using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Share;
using Share.Models.Domain;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Implements
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        private readonly BeerManagementContext _context;
        protected IDbContextTransaction _transaction;

        public Repository(BeerManagementContext beerManagementContext)
        {
            _context = beerManagementContext;
        }

        public async Task AddAsync(T obj)
        {
            await _context.Set<T>().AddAsync(obj);
        }

        public async Task<Boolean> DeleteAsync(int id)
        {
            var obj = await GetByIdAsync(id);
            if (obj == null)
            {
                return false;
            }
            _context.Set<T>().Remove(obj);
            return true;
        }

        private async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task EditAsync(T obj)
        {
            await _context.Set<T>().AddAsync(obj);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public T GetByKey(Object key)
        {
            var entity = _context.Set<T>().Find(key);
            return entity;
        }

        public IDbContextTransaction OpenTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
            return _transaction;
        }

        public void CommitTransactionAsync()
        {
            _transaction.CommitAsync();
        }

        public void RollBackTransactionAsync()
        {
            _transaction.RollbackAsync();
        }
    }
}
