using ShopProject.Data.Infrastructure;
using ShopProject.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopProject.Data.Repositories
{
    public interface IGroupRepository : IRepository<Group>
    {
        IEnumerable<Group> GetListGroupByProductId(int productID);
        IEnumerable<Product> GetListProductByGroupId(int groupID);
    }

    public class GroupRepository : RepositoryBase<Group>, IGroupRepository
    {
        public GroupRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<Group> GetListGroupByProductId(int productID)
        {
            var query = from g in DbContext.Groups
                        join pg in DbContext.ProductGroups
                        on g.ID equals pg.GroupID
                        where pg.ProductID == productID
                        select g;
            return query;
        }

        public IEnumerable<Product> GetListProductByGroupId(int groupID)
        {
            var query = from g in DbContext.Groups
                        join pg in DbContext.ProductGroups
                        on g.ID equals pg.GroupID
                        join p in DbContext.Products
                        on pg.ProductID equals p.ID
                        where pg.GroupID == groupID
                              
                        select p;
            return query;
        }
    }
}
