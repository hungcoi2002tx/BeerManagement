using AutoMapper;
using Business.Interfaces;
using DataLayer.Implements;
using DataLayer.Interfaces;
using Share.Models.Domain;
using Share.Models.Dtos.EditDtos;
using Share.Models.Dtos.SearchDtos;
using Share.Models.ResponseObject;
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
                throw;
            }
        }

        public async Task<ResponseCustom<Category>> DeleteAsync(int id)
        {
            try
            {
                var entity = await _repository.GetPageBySearchAsync(new CategorySearchDto()
                {
                    Id = id,
                });

                if (entity.Item2 == 0)
                {
                    return ResponeExtentions<Category>.GetError404($"Not Found Id = {id}");
                }

                var model = entity.Item1.First();
                model.IsEnable = !model.IsEnable;
                var result = await _repository.UpdateAsync(model);

                return new ResponseCustom<Category>()
                {
                    Status = result,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<ResponseCustom<Category>> GetAllBySearchAsync(CategorySearchDto searchModel)
        {
            try
            {
                var list = await _repository.GetAllBySearchAsync(searchModel);

                return new ResponseCustom<Category>
                {
                    Status = true,
                    Objects = list.ToList(),
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<ResponseCustom<Category>> GetPageBySearchAsync(CategorySearchDto model)
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
                throw;
            }
        }

        public async Task<ResponseCustom<Category>> UpdateAsync(Category model)
        {
            try
            {
                var entity = await _repository.UpdateAsync(_mapper.Map<Category>(model));
                return new ResponseCustom<Category>
                {
                    Status = true,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
    }
}
