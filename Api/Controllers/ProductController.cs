﻿using AutoMapper;
using Business.Implements;
using Business.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Share.Models.Domain;
using Share.Models.EditModels;
using Share.Models.SearchModels;
using Share.Ultils;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<ResponseCustom<Product>> GetAllAsync()
        {
            try
            {
                var result = await _productService.GetAllAsync();
                return result;
            }
            catch (Exception ex)
            {
                return ResponeExtentions<Product>.GetError500(ex.Message);
            }
        }

        [HttpGet("GetPage")]
        public async Task<ResponseCustom<Product>> GetPageAsync([FromBody] ProductSearchModel search)
        {
            try
            {
                var result = await _productService.GetPageBySearchAsync(search);
                return result;
            }
            catch (Exception ex)
            {
                return ResponeExtentions<Product>.GetError500(ex.Message);
            }
        }

        [HttpPost("Add")]
        public async Task<ResponseCustom<Product>> AddAsync([FromBody] ProductEditModel editModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ResponeExtentions<Product>.GetError400("Validate AddAsync - SupperlierController");
                }
                var result = await _productService.AddAsync(_mapper.Map<Product>(editModel));
                return result;
            }
            catch (Exception ex)
            {
                return ResponeExtentions<Product>.GetError500(ex.Message);
            }
        }

        [HttpPut("Update")]
        public async Task<ResponseCustom<Product>> UpdateAsync([FromBody] ProductEditModel editModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ResponeExtentions<Product>.GetError400("Validate UpdateAsync - ProductController");
                }
                var result = await _productService.UpdateAsync(_mapper.Map<Product>(editModel));
                return result;
            }
            catch (Exception ex)
            {
                return ResponeExtentions<Product>.GetError500(ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ResponseCustom<Product>> DeleteAsync(int id)
        {
            try
            {
                var result = await _productService.DeleteAsync(id);
                return result;
            }
            catch (Exception ex)
            {
                return ResponeExtentions<Product>.GetError500(ex.Message);
            }
        }
    }
}