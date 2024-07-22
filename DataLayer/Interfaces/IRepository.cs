using Microsoft.EntityFrameworkCore.Storage;
using Share.Models.Domain;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> AddAsync(T obj, bool usingTransaction = true);
        Task<bool> DeleteAsync(T obj, bool usingTransaction = true);
        IDbContextTransaction OpenTransaction();
        Task CommitTransactionAsync();
        Task RollBackTransactionAsync();
    }
}
