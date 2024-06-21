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
        Task<List<T>> GetAllAsync();
        Task AddAsync(T obj);
        Task<Boolean> DeleteAsync(int id);
        Task EditAsync(T obj);
        IDbContextTransaction OpenTransaction();
        void CommitTransactionAsync();
        void RollBackTransactionAsync();
    }
}
