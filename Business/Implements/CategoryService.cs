using Business.Interfaces;
using DataLayer.Interfaces;
using Share.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implements
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _categoryRepository.AddAsync(category);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            
        }

        public Task EditCategoryAsync(Category category)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Category>> GetAllCategoryAsync()
        {
            try
            {
                _categoryRepository.OpenTransaction();
                var list = await _categoryRepository.GetAllAsync();
                await _categoryRepository.CommitTransactionAsync();
                return list;
            }
            catch (Exception ex)
            {
                _categoryRepository.RollBackTransactionAsync();
                throw;
            }
        }
    }
}
