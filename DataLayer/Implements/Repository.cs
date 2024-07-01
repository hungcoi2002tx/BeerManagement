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
        public readonly BeerManagementContext _context;
        protected IDbContextTransaction _transaction;
        public DbSet<T> _dbSet { get => _context.Set<T>(); }

        public Repository(BeerManagementContext beerManagementContext)
        {
            _context = beerManagementContext;
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
            catch (Exception ex)
            {
                if (usingTransaction) await RollBackTransactionAsync();
                throw;
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
            catch (Exception ex)
            {
                if (usingTransaction) await RollBackTransactionAsync();
                throw;
            }
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var result = await _dbSet.FindAsync(id);
            await _context.SaveChangesAsync();
            return result;
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
