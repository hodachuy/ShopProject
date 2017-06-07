using ShopProject.Common.ViewModels;
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
    public interface IOrderService
    {
        Order Create(ref Order order, List<OrderDetail> orderDetails);
        Order Delete(int id);
        void Update(Order Order);
        void UpdateStatus(int orderId);
        IEnumerable<OrderDetail> GetOrderDetailByID(int id);
        IEnumerable<Order> GetAll(string keyword);
        Order GetById(int id);
        Product GetProductByID(int id);
        void Save();
    }
    public class OrderService : IOrderService
    {
        IOrderRepository _orderRepository;
        IOrderDetailRepository _orderDetailRepository;
        IProductRepository _productRepository;
        IUnitOfWork _unitOfWork;

        public OrderService(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            this._orderRepository = orderRepository;
            this._orderDetailRepository = orderDetailRepository;
            this._unitOfWork = unitOfWork;
            this._productRepository = productRepository;
        }
        public Order Create(ref Order order, List<OrderDetail> orderDetails)
        {
            try
            {
                _orderRepository.Add(order);
                _unitOfWork.Commit();

                foreach (var orderDetail in orderDetails)
                {
                    orderDetail.OrderID = order.ID;
                    _orderDetailRepository.Add(orderDetail);
                }
                return order;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void UpdateStatus(int orderId)
        {
            var order = _orderRepository.GetSingleById(orderId);
            order.Status = true;
            _orderRepository.Update(order);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public Order Delete(int id)
        {
            return _orderRepository.Delete(id);
        }

        public IEnumerable<Order> GetAll(string keyword)
        {
            if (!String.IsNullOrEmpty(keyword))
                return _orderRepository.GetMulti(x => x.CustomerName.Contains(keyword) || x.ID.ToString().Contains(keyword));
            else
                return _orderRepository.GetAll();
        }

        public Order GetById(int id)
        {
            return _orderRepository.GetSingleById(id);
        }

        public void Update(Order Order)
        {
            _orderRepository.Update(Order);
        }

        public IEnumerable<OrderDetail> GetOrderDetailByID(int id)
        {
            return _orderDetailRepository.GetMulti(x=>x.OrderID == id);
        }

        public Product GetProductByID(int id)
        {
            return _productRepository.GetSingleById(id);
        }
    }
}
