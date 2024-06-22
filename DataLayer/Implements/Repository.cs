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
        private DbSet<T> _dbSet { get => _context.Set<T>(); }

        public Repository(BeerManagementContext beerManagementContext)
        {
            _context = beerManagementContext;
        }

        public async Task AddAsync(T obj)
        {
            await _dbSet.AddAsync(obj);
        }

        public async Task<Boolean> DeleteAsync(int id)
        {
            var obj = await GetByIdAsync(id);
            if (obj == null)
            {
                return false;
            }
            _dbSet.Remove(obj);
            return true;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task EditAsync(T obj)
        {
            await _dbSet.AddAsync(obj);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public T GetByKey(Object key)
        {
            var entity = _dbSet.Find(key);
            return entity;
        }

        public IDbContextTransaction OpenTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
            return _transaction;
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                _transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if(_transaction != null)
                {
                    _transaction.Dispose();
                    _transaction = null;
                }
            }
        }

        public async Task RollBackTransactionAsync()
        {
            _transaction.RollbackAsync();
        }
    }
}
