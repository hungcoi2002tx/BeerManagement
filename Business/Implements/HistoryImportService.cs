using AutoMapper;
using Business.Interfaces;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore.Migrations;
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
    public class HistoryImportService : IHistoryImportService
    {
        private readonly Logger _logger;
        private readonly IHistoryImportRepository _repository;
        private readonly IMapper _mapper;

        public HistoryImportService(Logger logger, IHistoryImportRepository repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ResponseCustom<ImportHistory>> DeleteAsync(int id)
        {
            try
            {
                var entity = await _repository.GetPageBySearchAsync(new ImportHistorySearchDto()
                {
                    Id = id,
                });

                if (entity.Item2 == 0)
                {
                    return ResponeExtentions<ImportHistory>.GetError404($"Not Found Id = {id}");
                }

                var model = entity.Item1.First();
                model.IsEnable = !model.IsEnable;
                var result = await _repository.UpdateAsync(model);

                return new ResponseCustom<ImportHistory>()
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

        public async Task<ResponseCustom<ImportHistory>> UpdateAsync(ImportHistory model)
        {
            try
            {
                var entity = await _repository.UpdateAsync(_mapper.Map<ImportHistory>(model));
                return new ResponseCustom<ImportHistory>
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

        public Task<ResponseCustom<ImportHistory>> GetAllBySearchAsync(ImportHistorySearchDto model)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseCustom<ImportHistory>> GetPageBySearchAsync(ImportHistorySearchDto model)
        {
            try
            {
                var data = await _repository.GetPageBySearchAsync(model);
                return new ResponseCustom<ImportHistory>()
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

        async Task<ResponseCustom<ImportHistory>> IHistoryImportService.AddAsync(ImportHistoryEditDto model)
        {
            try
            {
                var data = _mapper.Map<ImportHistory>(model);
                data.Date = DateTime.Now;
                var entity = await _repository.AddAsync(data);
                return new ResponseCustom<ImportHistory>
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
    }
}
