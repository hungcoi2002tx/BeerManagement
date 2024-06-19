using Share.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task AddAsync(T obj);
        Task DeleteAsync(int id);
        Task EditAsync(T obj);
    }
}
