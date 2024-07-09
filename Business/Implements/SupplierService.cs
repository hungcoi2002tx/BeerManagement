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
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;

        public SupplierService(Logger logger, ISupplierRepository supplierRepository, IMapper mapper)
        {
            _logger = logger;
            _supplierRepository = supplierRepository;
            _mapper = mapper;
        }

        public async Task<ResponseCustom<Supplier>> AddAsync(Supplier model)
        {
            try
            {
                var entity = await _supplierRepository.AddAsync(model);
                return new ResponseCustom<Supplier>
                {
                    Status = true,
                    Object = entity
                };
            }
            catch (Exception ex)
            {
                await _supplierRepository.RollBackTransactionAsync();
                _logger.LogError(ex.ToString());
                return ResponeExtentions<Supplier>.GetError500(ex.ToString());
            }
        }

        public async Task<ResponseCustom<Supplier>> DeleteAsync(Supplier model)
        {
            try
            {
                var entity = _supplierRepository.GetPageBySearchAsync(new SupplierSearchModel()
                {
                    Id = model.Id,
                });
                if (entity == null)
                {
                    return ResponeExtentions<Supplier>.GetError404($"Not Found Id = {model.Id}");
                }
                var result = await _supplierRepository.DeleteAsync(model);
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
                var list = await _supplierRepository.GetAllAsync();
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
                var data = await _supplierRepository.GetPageBySearchAsync(model);
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
                var entity = await _supplierRepository.UpdateAsync(model);
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
