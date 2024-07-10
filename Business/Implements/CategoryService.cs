using AutoMapper;
using Business.Interfaces;
using DataLayer.Implements;
using DataLayer.Interfaces;
using Share.Models.Domain;
using Share.Models.SearchModels;
using Share.Ultils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implements
{
    public class CategoryService : ICategoryService
    {
        private readonly Logger _logger;
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public CategoryService(Logger logger, ICategoryRepository repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ResponseCustom<Category>> AddAsync(Category model)
        {
            try
            {
                var entity = await _repository.AddAsync(model);
                return new ResponseCustom<Category>
                {
                    Status = true,
                    Object = entity
                };
            }
            catch (Exception ex)
            {
                await _repository.RollBackTransactionAsync();
                _logger.LogError(ex.ToString());
                return ResponeExtentions<Category>.GetError500(ex.ToString());
            }
        }

        public async Task<ResponseCustom<Category>> DeleteAsync(int id)
        {
            try
            {
                var entity = await _repository.GetPageBySearchAsync(new CategorySearchModel()
                {
                    Id = id,
                });
                if (entity.Item2 == 0)
                {
                    return ResponeExtentions<Category>.GetError404($"Not Found Id = {id}");
                }
                var result = await _repository.DeleteAsync(entity.Item1.First());
                return new ResponseCustom<Category>()
                {
                    Status = result,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return ResponeExtentions<Category>.GetError500(ex.ToString());
            }
        }

        public async Task<ResponseCustom<Category>> GetAllAsync()
        {
            try
            {
                var list = await _repository.GetAllAsync();
                return new ResponseCustom<Category>
                {
                    Status = true,
                    Objects = list.ToList(),
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return ResponeExtentions<Category>.GetError500(ex.ToString());
            }
        }

        public async Task<ResponseCustom<Category>> GetPageBySearchAsync(CategorySearchModel model)
        {
            try
            {
                var data = await _repository.GetPageBySearchAsync(model);
                return new ResponseCustom<Category>()
                {
                    Status = true,
                    Objects = data.Item1,
                    Total = data.Item2
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return ResponeExtentions<Category>.GetError500(ex.ToString());
            }
        }

        public async Task<ResponseCustom<Category>> UpdateAsync(Category model)
        {
            try
            {
                var entity = await _repository.UpdateAsync(model);
                return new ResponseCustom<Category>
                {
                    Status = true,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return ResponeExtentions<Category>.GetError500(ex.ToString());
            }
        }
    }
}
