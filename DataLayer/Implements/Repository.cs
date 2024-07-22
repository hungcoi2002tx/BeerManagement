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
        private IDbContextTransaction _transaction;
        protected readonly BeerManagementContext _context;
        protected DbSet<T> _dbSet { get => _context.Set<T>(); }

        public Repository(BeerManagementContext context)
        {
            _context = context;
        }

        public async Task<T> AddAsync(T obj, bool usingTransaction = true)
        {
            try
            {
                if (usingTransaction) OpenTransaction();

                await _dbSet.AddAsync(obj);
                await _context.SaveChangesAsync();

                if (usingTransaction) await CommitTransactionAsync();

                return obj;
            }
            catch (Exception)
            {
                if (usingTransaction) await RollBackTransactionAsync();
                throw ;
            }
        }

        public async Task<bool> DeleteAsync(T obj, bool usingTransaction = true)
        {
            try
            {
                if (usingTransaction) OpenTransaction();

                _dbSet.Remove(obj);
                _context.SaveChanges();

                if (usingTransaction) await CommitTransactionAsync();

                return true;
            }
            catch (Exception)
            {
                if (usingTransaction) await RollBackTransactionAsync();
                throw;
            }
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var result = await _dbSet.FindAsync(id);

            return result;
        }

        public async Task<List<T>> GetAllBySearchAsync()
        {
            var result = await _dbSet.ToListAsync();

            return result;
        }

        public T? GetByKey(Object key)
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
            await _transaction.CommitAsync();
        }

        public async Task RollBackTransactionAsync()
        {
            await _transaction.RollbackAsync();
        }

        public async Task<bool> AddRangeAsync(List<T> list, bool usingTransaction = true)
        {
            try
            {
                if (usingTransaction) OpenTransaction();

                await _dbSet.AddRangeAsync(list);
                await _context.SaveChangesAsync();

                if (usingTransaction) await CommitTransactionAsync();

                return true;
            }
            catch (Exception)
            {
                if (usingTransaction) await RollBackTransactionAsync();
                throw;
            }
        }
    }
}
