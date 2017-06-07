using ShopProject.Common.Exceptions;
using ShopProject.Data.Infrastructure;
using ShopProject.Data.Repositories;
using ShopProject.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopProject.Service
{
    public interface IGroupService
    {
        Group Add(Group group);
        void Update(Group group);
        Group Delete(int id);
        void Save();
        IEnumerable<Group> GetAll();
        IEnumerable<Group> GetAll(string keyword);
        IEnumerable<Group> GetGroupActive();
        IEnumerable<Product> GetListProductByGroupActive(int groupID,int top);
        IEnumerable<Product> GetListProductByGroupIdPaging(int groupID, int page, int pageSize, string sort, out int totalRow);
        Group GetByID(int id);
        bool AddProductToGroups(IEnumerable<ProductGroup> productGroups, int productID);

        IEnumerable<Group> GetListGroupByProductId(int productID);

        IEnumerable<Product> GetListProductByGroupId(int groupID);

    }
    public class GroupService : IGroupService
    {
        IGroupRepository _groupRepository;
        IProductGroupRepository _productGroupRepository;
        IUnitOfWork _unitOfWork;
        public GroupService(IGroupRepository groupRepository, IProductGroupRepository productGroupRepository,IUnitOfWork unitOfWork)
        {
            _groupRepository = groupRepository;
            _productGroupRepository = productGroupRepository;
            _unitOfWork = unitOfWork;
        }
        public Group Add(Group group)
        {
            return  _groupRepository.Add(group);
        }

        public bool AddProductToGroups(IEnumerable<ProductGroup> productGroups, int productID)
        {
            _productGroupRepository.DeleteMulti(x => x.ProductID == productID);
            foreach (var productGroup in productGroups)
            {
                _productGroupRepository.Add(productGroup);
            }
            return true;
        }

        public Group Delete(int id)
        {
            return _groupRepository.Delete(id);
        }

        public IEnumerable<Group> GetAll()
        {
            return _groupRepository.GetAll();
        }

        public IEnumerable<Group> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _groupRepository.GetMulti(x => x.Name.Contains(keyword));
            else
                return _groupRepository.GetAll();
        }

        public Group GetByID(int id)
        {
            return _groupRepository.GetSingleById(id);
        }

        public IEnumerable<Group> GetGroupActive()
        {
            return _groupRepository.GetMulti(x => x.Status);
        }

        public IEnumerable<Group> GetListGroupByProductId(int productID)
        {
            return _groupRepository.GetListGroupByProductId(productID);
        }

        public IEnumerable<Product> GetListProductByGroupActive(int groupID,int top)
        {
             return _groupRepository.GetListProductByGroupId(groupID).Take(top);            
        }

        public IEnumerable<Product> GetListProductByGroupId(int groupID)
        {
            return _groupRepository.GetListProductByGroupId(groupID);
        }

        public IEnumerable<Product> GetListProductByGroupIdPaging(int groupID, int page, int pageSize, string sort, out int totalRow)
        {
            var query = _groupRepository.GetListProductByGroupId(groupID);
            switch (sort)
            {
                case "popular":
                    query = query.OrderByDescending(x => x.ViewCount);
                    break;
                case "discount":
                    query = query.OrderByDescending(x => x.PromotionPrice.HasValue);
                    break;
                case "price":
                    query = query.OrderBy(x => x.Price);
                    break;
                default:
                    query = query.OrderByDescending(x => x.CreatedDate);
                    break;
            }

            totalRow = query.Count();

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Group group)
        {
            _groupRepository.Update(group);
        }
    }
}
