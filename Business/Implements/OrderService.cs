using AutoMapper;
using Business.Interfaces;
using DataLayer.Implements;
using DataLayer.Interfaces;
using Microsoft.Extensions.Logging;
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
    public class OrderService : IOrderService
    {
        private readonly Logger _logger;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(Logger logger, IOrderRepository orderRepository, IMapper mapper)
        {
            _logger = logger;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<ResponseCustom<Order>> AddAsync(OrderAddDto obj)
        {
            try
            {
                var response = await _orderRepository.AddAsync(_mapper.Map<Order>(obj));

                return new ResponseCustom<Order>
                {
                    Status = true,
                    Object = response
                };
            }
            catch (Exception ex)
            {
                await _orderRepository.RollBackTransactionAsync();
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<ResponseCustom<Order>> DeleteAsync(int id)
        {
            try
            {
                var response = await _orderRepository.GetPageBySearchAsync(new OrderSearchDto()
                {
                    Id = id,
                });

                if (response.Item2 == 0)
                {
                    return ResponeExtentions<Order>.GetError404($"Not found table with id = {id}");
                }

                var model = response.Item1.First();
                model.IsEnable = !model.IsEnable;
                var result = await _orderRepository.UpdateAsync(model);

                return new ResponseCustom<Order>()
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

        public async Task<ResponseCustom<Order>> GetAllAsync()
        {
            try
            {
                var response = await _orderRepository.GetAllAsync();

                return new ResponseCustom<Order>
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

        public async Task<ResponseCustom<Order>> GetPageBySearchAsync(OrderSearchDto obj)
        {
            try
            {
                var data = await _orderRepository.GetPageBySearchAsync(obj);

                return new ResponseCustom<Order>()
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

        public async Task<ResponseCustom<Order>> UpdateAsync(OrderEditDto obj)
        {
            try
            {
                var entity = await _orderRepository.UpdateAsync(_mapper.Map<Order>(obj));

                return new ResponseCustom<Order>
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
