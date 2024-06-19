using DataLayer.Interfaces;
using Share;
using Share.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Implements
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(BeerManagementContext beerManagementContext) : base(beerManagementContext)
        {
        }
    }
}
