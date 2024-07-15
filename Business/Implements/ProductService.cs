using AutoMapper;
using Business.Interfaces;
using DataLayer.Interfaces;
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
    public class ProductService : IProductService
    {
        private readonly Logger _logger;
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductService(Logger logger, IProductRepository productRepository, IMapper mapper)
        {
            _logger = logger;
            _repository = productRepository;
            _mapper = mapper;
        }

        public async Task<ResponseCustom<Product>> AddAsync(Product model)
        {
            try
            {
                var entity = await _repository.AddAsync(model);
                return new ResponseCustom<Product>
                {
                    Status = true,
                    Object = entity
                };
            }
            catch (Exception ex)
            {
                await _repository.RollBackTransactionAsync();
                _logger.LogError(ex.ToString());
                return ResponeExtentions<Product>.GetError500(ex.ToString());
            }
        }

        public async Task<ResponseCustom<Product>> DeleteAsync(int id)
        {
            try
            {
                var entity = await _repository.GetPageBySearchAsync(new ProductSearchModel
                {
                    Id = id
                });
                if(entity.Item2 == 0)
                {
                    return ResponeExtentions<Product>.GetError404($"Not Found Id = {id}");
                }
                var product = entity.Item1.First();
                product.IsEnable = false;
                var result = await _repository.UpdateAsync(product);
                return new ResponseCustom<Product>()
                {
                    Status = result,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return ResponeExtentions<Product>.GetError500(ex.ToString());
            }
        }

        public async Task<ResponseCustom<Product>> GetAllAsync()
        {
            try
            {
                var products = await _repository.GetAllAsync();
                return new ResponseCustom<Product>
                {
                    Status = true,
                    Objects = products.ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return ResponeExtentions<Product>.GetError500(ex.ToString());
            }
        }

        public async Task<ResponseCustom<Product>> GetPageBySearchAsync(ProductSearchModel model)
        {
            try
            {               
                var data = await _repository.GetPageBySearchAsync(model);
                return new ResponseCustom<Product>()
                {
                    Status = true,
                    Objects = data.Item1,
                    Total = data.Item2
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return ResponeExtentions<Product>.GetError500(ex.ToString());
            }
        }

        public async Task<ResponseCustom<Product>> UpdateAsync(Product model)
        {
            try
            {
                var entity = await _repository.UpdateAsync(model);
                return new ResponseCustom<Product>
                {
                    Status = true,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return ResponeExtentions<Product>.GetError500(ex.ToString());
            }
        }
    }
}
