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
            _context.SaveChanges();
        }

        public async Task<Boolean> DeleteAsync(int id)
        {
            var obj = await GetByIdAsync(id);
            if (obj == null)
            {
                return false;
            }
            _dbSet.Remove(obj);
            _context.SaveChanges();

            return true;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var result = await _dbSet.FindAsync(id);
            _context.SaveChanges();
            return result;
        }

        public async Task EditAsync(T obj)
        {
            await _dbSet.AddAsync(obj);
            _context.SaveChanges();
        }

        public async Task<List<T>> GetAllAsync()
        {
            var result = await _dbSet.ToListAsync();
            return result;
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
            _transaction.CommitAsync();
        }

        public async Task RollBackTransactionAsync()
        {
            _transaction.RollbackAsync();
        }
    }
}
