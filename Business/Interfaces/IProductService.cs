﻿using Share.Models.Domain;
using Share.Models.SearchModels;
using Share.Ultils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IProductService
    {
        Task<ResponseCustom<Product>> GetAllAsync();
        Task<ResponseCustom<Product>> AddAsync(Product model);
        Task<ResponseCustom<Product>> UpdateAsync(Product model);
        Task<ResponseCustom<Product>> DeleteAsync(int id);
        Task<ResponseCustom<Product>> GetPageBySearchAsync(ProductSearchModel model);
    }
}