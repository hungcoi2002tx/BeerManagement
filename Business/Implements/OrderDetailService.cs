using AutoMapper;
using Business.Interfaces;
using DataLayer.Implements;
using DataLayer.Interfaces;
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
    public class OrderDetailService : IOrderDetailService
    {
        private readonly Logger _logger;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IMapper _mapper;

        public OrderDetailService(Logger logger, IOrderDetailRepository orderDetailRepository, IMapper mapper)
        {
            _logger = logger;
            _orderDetailRepository = orderDetailRepository;
            _mapper = mapper;
        }

        public async Task<ResponseCustom<OrderDetail>> AddAsync(List<OrderDetailAddDto> list)
        {
            try
            {
                var response = await _orderDetailRepository.AddRangeAsync(_mapper.Map<List<OrderDetail>>(list));

                return new ResponseCustom<OrderDetail>
                {
                    Status = response,
                };
            }
            catch (Exception ex)
            {
                await _orderDetailRepository.RollBackTransactionAsync();
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<ResponseCustom<OrderDetail>> DeleteAsync(int orderId, int productId)
        {
            try
            {
                var response = await _orderDetailRepository.GetPageBySearchAsync(new OrderDetailSearchDto()
                {
                    OrderId = orderId,
                    ProductId = productId,
                });

                if (response.Item2 == 0)
                {
                    return ResponeExtentions<OrderDetail>.GetError404($"Not found Order Detail with orderId = {orderId} and productId = {productId}");
                }

                var model = response.Item1.First();
                model.IsEnable = !model.IsEnable;
                var result = await _orderDetailRepository.UpdateAsync(model);

                return new ResponseCustom<OrderDetail>()
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

        public async Task<ResponseCustom<OrderDetail>> GetAllBySearchAsync(OrderDetailSearchDto obj)
        {
            try
            {
                var response = await _orderDetailRepository.GetAllBySearchAsync(obj);

                return new ResponseCustom<OrderDetail>
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

        public async Task<ResponseCustom<OrderDetail>> GetPageBySearchAsync(OrderDetailSearchDto obj)
        {
            try
            {
                var data = await _orderDetailRepository.GetPageBySearchAsync(obj);

                return new ResponseCustom<OrderDetail>()
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

        public async Task<ResponseCustom<OrderDetail>> UpdateAsync(List<OrderDetailEditDto> obj)
        {
            try
            {
                for (int i = 0; i< obj.Count; i++)
                {
                    await _orderDetailRepository.UpdateAsync(_mapper.Map<OrderDetail>(obj[i]));
                }

                return new ResponseCustom<OrderDetail>
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
