using ShopProject.Data.Infrastructure;
using ShopProject.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopProject.Data.Repositories
{
    public interface IProductGroupRepository : IRepository<ProductGroup>
    {
    }

    public class ProductGroupRepository : RepositoryBase<ProductGroup>, IProductGroupRepository
    {
        public ProductGroupRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
