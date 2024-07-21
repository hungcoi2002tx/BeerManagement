using AutoMapper;
using Business.Interfaces;
using DataLayer.Implements;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata;
using Share.Models.Domain;
using Share.Models.Dtos.AddDtos;
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
    public class TableService : ITableService
    {
        private readonly Logger _logger;
        private readonly ITableRepository _tableRepository;
        private readonly IMapper _mapper;

        public TableService(Logger logger, ITableRepository tableRepository, IMapper mapper)
        {
            _logger = logger;
            _tableRepository = tableRepository;
            _mapper = mapper;
        }

        public async Task<ResponseCustom<Table>> AddAsync(TableAddDto obj)
        {
            try
            {
                var response = await _tableRepository.AddAsync(_mapper.Map<Table>(obj));

                return new ResponseCustom<Table>
                {
                    Status = true,
                    Object = response
                };
            }
            catch (Exception ex)
            {
                await _tableRepository.RollBackTransactionAsync();
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<ResponseCustom<Table>> DeleteAsync(int id)
        {
            try
            {
                var response = await _tableRepository.GetPageBySearchAsync(new TableSearchDto()
                {
                    Id = id,
                });

                if (response.Item2 == 0)
                {
                    return ResponeExtentions<Table>.GetError404($"Not found table with id = {id}");
                }

                var model = response.Item1.First();
                model.IsEnable = !model.IsEnable;
                var result = await _tableRepository.UpdateAsync(model);

                return new ResponseCustom<Table>()
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

        public async Task<ResponseCustom<Table>> GetAllBySearchAsync(TableSearchDto searchModel)
        {
            try
            {
                var response = await _tableRepository.GetAllBySearchAsync(searchModel);

                return new ResponseCustom<Table>
                {
                    Status = true,
                    Objects = response,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<ResponseCustom<Table>> GetPageBySearchAsync(TableSearchDto obj)
        {
            try
            {
                var data = await _tableRepository.GetPageBySearchAsync(obj);

                return new ResponseCustom<Table>()
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

        public async Task<ResponseCustom<Table>> UpdateAsync(TableEditDto obj)
        {
            try
            {
                var entity = await _tableRepository.UpdateAsync(_mapper.Map<Table>(obj));

                return new ResponseCustom<Table>
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
