using AutoMapper;
using ShopProject.Common;
using ShopProject.Model.Models;
using ShopProject.Service;
using ShopProject.Web.Infrastructure.Core;
using ShopProject.Web.Infrastructure.Extensions;
using ShopProject.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace ShopProject.Web.API
{
    [Authorize]
    [RoutePrefix("api/order")]
    public class OrderController : ApiControllerBase
    {
        IOrderService _orderService;
        public OrderController(IErrorService errorService, IOrderService orderService) :base(errorService)
        {
            _orderService = orderService;
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 10)
        {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                int totalRow = 0;

                var lstOrder = _orderService.GetAll(keyword);

                totalRow = lstOrder.Count();

                var query = lstOrder.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);

                var lstOrderVm = Mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(query);

                var paginationSet = new PaginationSet<OrderViewModel>()
                {
                    Items = lstOrderVm,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };

                response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;

            });
        }

        [Route("getbyid/{orderID:int}")]
        [HttpGet]
        public HttpResponseMessage GetByID(HttpRequestMessage request, int orderID)
        {
            return CreateHttpResponse(request, () =>
            {
                var orderItem = _orderService.GetById(orderID);
                var orderItemVm = Mapper.Map<Order, OrderViewModel>(orderItem);
                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, orderItemVm);
                return response;
            });
        }


        [Route("getorderdetail/{orderID:int}")]
        [HttpGet]
        public HttpResponseMessage GetOrderDetail(HttpRequestMessage request,int orderID)
        {
            return CreateHttpResponse(request, () =>
            {
                var lstOrderDetail = _orderService.GetOrderDetailByID(orderID);
                foreach (var item in lstOrderDetail)
                {
                    var product = _orderService.GetProductByID(item.ProductID);
                    item.Product.Image = ConvertData.ImageToBase64String(product.Image, CommonConstants.PathProduct);
                }

                var responseData = Mapper.Map<IEnumerable<OrderDetail>,IEnumerable<OrderDetailViewModel>>(lstOrderDetail);
                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [Route("edit")]
        [HttpPut]
        public HttpResponseMessage Edit(HttpRequestMessage request, OrderViewModel orderVm)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var orderDb = new Order();
                    orderDb.ID = orderVm.ID;
                    orderDb.UpdateOrder(orderVm);
                    _orderService.Update(orderDb);
                    _orderService.Save();
                    var responseData = Mapper.Map<Order, OrderViewModel>(orderDb);
                    response = request.CreateResponse(HttpStatusCode.OK, responseData);
                }
                return response;
            });
        }
        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage Delete(HttpRequestMessage request, int ordertID)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var order = _orderService.Delete(ordertID);
                    _orderService.Save();

                    var responseData = Mapper.Map<Order, OrderViewModel>(order);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }
                return response;
            });
        }

        [Route("deletemulti")]
        [HttpDelete]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string lstOrderID)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var lstItemID = new JavaScriptSerializer().Deserialize<List<int>>(lstOrderID);
                    foreach (var item in lstItemID)
                    {
                        _orderService.Delete(item);
                    }
                    _orderService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, lstItemID.Count);
                }
                return response;
            });
        }
    }
}