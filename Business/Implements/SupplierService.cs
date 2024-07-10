using AutoMapper;
using Business.Interfaces;
using DataLayer.Interfaces;
using Microsoft.Extensions.Logging;
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
                return ResponeExtentions<Supplier>.GetError500(ex.ToString());
            }
        }

        public async Task<ResponseCustom<Supplier>> DeleteAsync(int id)
        {
            try
            {
                var entity = await _repository.GetPageBySearchAsync(new SupplierSearchModel()
                {
                    Id = id,
                });
                if (entity.Item2 == 0)
                {
                    return ResponeExtentions<Supplier>.GetError404($"Not Found Id = {id}");
                }
                var result = await _repository.DeleteAsync(entity.Item1.First());
                return new ResponseCustom<Supplier>()
                {
                    Status = result,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return ResponeExtentions<Supplier>.GetError500(ex.ToString());
            }
        }

        public async Task<ResponseCustom<Supplier>> GetAllAsync()
        {
            try
            {
                var list = await _repository.GetAllAsync();
                return new ResponseCustom<Supplier>
                {
                    Status = true,
                    Objects = list.ToList(),
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return ResponeExtentions<Supplier>.GetError500(ex.ToString());
            }
        }

        public async Task<ResponseCustom<Supplier>> GetPageBySearchAsync(SupplierSearchModel model)
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
                return ResponeExtentions<Supplier>.GetError500(ex.ToString());
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
                return ResponeExtentions<Supplier>.GetError500(ex.ToString());
            }
        }
    }
}
