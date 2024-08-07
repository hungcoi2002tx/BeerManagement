﻿using AutoMapper;
using Business.Interfaces;
using DataLayer.Interfaces;
using Microsoft.Extensions.Logging;
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
    public class SupplierService : ISupplierService
    {
        private readonly Logger _logger;
        private readonly ISupplierRepository _repository;
        private readonly IMapper _mapper;

        public SupplierService(Logger logger, ISupplierRepository supplierRepository, IMapper mapper)
        {
            _logger = logger;
            _repository = supplierRepository;
            _mapper = mapper;
        }

        public async Task<ResponseCustom<Supplier>> AddAsync(Supplier model)
        {
            try
            {
                var entity = await _repository.AddAsync(model);

                return new ResponseCustom<Supplier>
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

        public async Task<ResponseCustom<Supplier>> DeleteAsync(int id)
        {
            try
            {
                var entity = await _repository.GetPageBySearchAsync(new SupplierSearchDto()
                {
                    Id = id,
                });

                if (entity.Item2 == 0)
                {
                    return ResponeExtentions<Supplier>.GetError404($"Not Found Id = {id}");
                }

                var model = entity.Item1.First();
                model.IsEnable = !model.IsEnable;
                var result = await _repository.UpdateAsync(model);

                return new ResponseCustom<Supplier>()
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

        public async Task<ResponseCustom<Supplier>> GetAllBySearchAsync(SupplierSearchDto searchModel)
        {
            try
            {
                var list = await _repository.GetAllBySearchAsync(searchModel);

                return new ResponseCustom<Supplier>
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

        public async Task<ResponseCustom<Supplier>> GetPageBySearchAsync(SupplierSearchDto model)
        {
            try
            {
                var data = await _repository.GetPageBySearchAsync(model);

                return new ResponseCustom<Supplier>()
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

        public async Task<ResponseCustom<Supplier>> UpdateAsync(Supplier model)
        {
            try
            {
                var entity = await _repository.UpdateAsync(model);

                return new ResponseCustom<Supplier>
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
