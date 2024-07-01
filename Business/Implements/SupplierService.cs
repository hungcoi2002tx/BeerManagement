using AutoMapper;
using Business.Interfaces;
using DataLayer.Interfaces;
using Microsoft.Extensions.Logging;
using Share.Constant;
using Share.Models.Domain;
using Share.Models.SearchModels;
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
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;

        public SupplierService(Logger logger, ISupplierRepository supplierRepository, IMapper mapper)
        {
            _logger = logger;
            _supplierRepository = supplierRepository;
            _mapper = mapper;
        }

        public async Task<ExecuteRespone<Supplier>> AddAsync(Supplier model)
        {
            try
            {
                _supplierRepository.OpenTransaction();
                var entity = await _supplierRepository.AddAsync(model);
                await _supplierRepository.CommitTransactionAsync();
                return new ExecuteRespone<Supplier>
                {
                    Status = true,
                    Object = entity
                };
            }
            catch (Exception ex)
            {
                await _supplierRepository.RollBackTransactionAsync();
                _logger.LogError(ex.ToString());
                return new ExecuteRespone<Supplier>
                {
                    Status = false
                };
            }
        }

        public async Task<ExecuteRespone<Supplier>> DeleteAsync(Supplier model)
        {
            try
            {
                var result = await _supplierRepository.DeleteAsync(model);
                return new ExecuteRespone<Supplier>()
                {
                    Status = true,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new ExecuteRespone<Supplier>()
                {
                    Status = false
                };
            }
        }

        public async Task<List<Supplier>> GetAllAsync()
        {
            try
            {
                var list = await _supplierRepository.GetAllAsync();
                return list;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new List<Supplier>();
            }
        }

        public async Task<List<Supplier>> GetPageBySearchAsync(SupplierSearchModel model)
        {
            try
            {
                var data = await _supplierRepository.GetPageBySearchAsync(model);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new List<Supplier>();
            }
        }

        public async Task<ExecuteRespone<Supplier>> UpdateAsync(Supplier model)
        {
            try
            {
                var entity = await _supplierRepository.UpdateAsync(model);
                return new ExecuteRespone<Supplier>
                {
                    Status = true,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new ExecuteRespone<Supplier>
                {
                    Status = false
                };
            }
        }
    }
}
