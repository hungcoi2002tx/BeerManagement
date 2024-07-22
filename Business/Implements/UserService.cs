using AutoMapper;
using Business.Interfaces;
using DataLayer.Interfaces;
using Share.Models.Domain;
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
    public class UserService : IUserService
    {
        private readonly Logger _logger;
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UserService(Logger logger, IUserRepository repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ResponseCustom<User>> AddAsync(User model)
        {
            try
            {
                var entity = await _repository.AddAsync(model);
                return new ResponseCustom<User>
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

        public async Task<ResponseCustom<User>> DeleteAsync(int id)
        {
            try
            {
                var entity = await _repository.GetPageBySearchAsync(new UserSearchDto()
                {
                    Id = id,
                });

                if (entity.Item2 == 0)
                {
                    return ResponeExtentions<User>.GetError404($"Not Found Id = {id}");
                }

                var model = entity.Item1.First();
                model.IsEnable = !model.IsEnable;
                var result = await _repository.UpdateAsync(model);

                return new ResponseCustom<User>()
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

        public async Task<ResponseCustom<User>> GetAllBySearchAsync(UserSearchDto searchModel)
        {
            try
            {
                var list = await _repository.GetAllBySearchAsync(searchModel);

                return new ResponseCustom<User>
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

        public async Task<ResponseCustom<User>> GetPageBySearchAsync(UserSearchDto model)
        {
            try
            {
                var data = await _repository.GetPageBySearchAsync(model);
                return new ResponseCustom<User>()
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

        public async Task<ResponseCustom<User>> UpdateAsync(User model)
        {
            try
            {
                var entity = await _repository.UpdateAsync(_mapper.Map<User>(model));
                return new ResponseCustom<User>
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
