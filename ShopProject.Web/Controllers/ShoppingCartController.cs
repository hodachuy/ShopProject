using ShopProject.Service;
using ShopProject.Web.App_Start;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ShopProject.Common;
using ShopProject.Web.Models;
using Microsoft.AspNet.Identity;
using System.Web.Script.Serialization;
using ShopProject.Model.Models;
using AutoMapper;
using ShopProject.Web.Infrastructure.Extensions;
using ShopProject.Web.Infrastructure.NganLuongAPI;
using Shopproject.Common;

namespace ShopProject.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        IProductService _productService;
        IOrderService _orderService;
        private ApplicationUserManager _userManager;

        private string _merchantId = ConfigHelper.GetByKey("MerchantId");
        private string _merchantPassword = ConfigHelper.GetByKey("MerchantPassword");
        private string _merchantEmail = ConfigHelper.GetByKey("MerchantEmail");
        public ShoppingCartController(IProductService productService, IOrderService orderService, ApplicationUserManager userManager)
        {
            _productService = productService;
            _userManager = userManager;
            _orderService = orderService;
        }

        public List<ShoppingCartViewModel> GetCart()
        {
            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            if (cart == null || cart.Count == 0)
            {
                cart = new List<ShoppingCartViewModel>();
                Session[CommonConstants.SessionCart] = cart;
            }
            return cart;
        }

        [ChildActionOnly]
        public ActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        public ActionResult CountCart()
        {
            var cart = GetCart();

            return PartialView("_CountCart", cart);
        }

        public ActionResult ViewCart()
        {
            var cart = GetCart();
            return PartialView("_ViewCart", cart);
        }

        public ActionResult CheckOut()
        {
            var cart = GetCart();
            if (cart == null || cart.Count() == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(cart);
        }

        public JsonResult GetUser()
        {
            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var user = _userManager.FindById(userId);
                return Json(new
                {
                    data = user,
                    status = true
                });
            }
            return Json(new
            {
                status = false
            });
        }

        public ActionResult CreateOrder(string orderViewModel)
        {
            var order = new JavaScriptSerializer().Deserialize<OrderViewModel>(orderViewModel);

            var orderNew = new Order();

            orderNew.UpdateOrder(order);

            if (Request.IsAuthenticated)
            {
                orderNew.CustomerId = User.Identity.GetUserId();
                orderNew.CreatedBy = User.Identity.GetUserName();
            }

            var cart = GetCart();
            List<OrderDetail> orderDetails = new List<OrderDetail>();

            foreach (var item in cart)
            {
                var detail = new OrderDetail();
                detail.ProductID = item.ProductId;
                detail.Quantity = item.Quantity;
                detail.Price = item.Product.Price;
                orderDetails.Add(detail);
                _productService.SellProduct(item.ProductId, item.Quantity);

            }
            var orderReturn = _orderService.Create(ref orderNew, orderDetails);
            _orderService.Save();
            // get ID Order.
            TempData["OrderID"] = orderReturn.ID;
            if (order.PaymentMethod == "CASH")
            {
                _orderService.Save();
                return Json(new
                {
                    status = true,
                    data = "CASH"
                });
            }
            else
            {

                var currentLink = ConfigHelper.GetByKey("CurrentLink");
                RequestInfo info = new RequestInfo();
                info.Merchant_id = _merchantId;
                info.Merchant_password = _merchantPassword;
                info.Receiver_email = _merchantEmail;



                info.cur_code = "vnd";
                info.bank_code = order.BankCode;

                info.Order_code = orderReturn.ID.ToString();
                info.Total_amount = orderDetails.Sum(x => x.Quantity * x.Price).ToString();
                info.fee_shipping = "0";
                info.Discount_amount = "0";
                info.order_description = "Thanh toán đơn hàng tại BigShop";
                info.return_url = currentLink + "xac-nhan-don-hang.html";
                info.cancel_url = currentLink + "huy-don-hang.html";

                info.Buyer_fullname = order.CustomerName;
                info.Buyer_email = order.CustomerEmail;
                info.Buyer_mobile = order.CustomerMobile;

                APICheckoutV3 objNLChecout = new APICheckoutV3();
                ResponseInfo result = objNLChecout.GetUrlCheckout(info, order.PaymentMethod);
                if (result.Error_code == "00")
                {
                    return Json(new
                    {
                        status = true,
                        urlCheckout = result.Checkout_url,
                        message = result.Description
                    });
                }
                else

                    return Json(new
                    {
                        status = false,
                        message = result.Description
                    });
            }
        }

        #region CRUD and Get ShoppingCart
        public ActionResult GetAll()
        {
            var cart = GetCart();
            return Json(new
            {
                data = cart,
                status = true,

            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Add(int productId)
        {
            var cart = GetCart();

            var product = _productService.GetById(productId);

            if (product.Quantity == 0)
            {
                return Json(new
                {
                    status = false,
                    message = "Sản phẩm này hiện đang hết hàng"
                });
            }
            if (cart.Any(x => x.ProductId == productId))
            {
                foreach (var item in cart)
                {
                    if (item.ProductId == productId)
                    {
                        item.Quantity += 1;
                    }
                }
            }
            else
            {
                ShoppingCartViewModel newItem = new ShoppingCartViewModel();
                newItem.ProductId = productId;
                newItem.Product = Mapper.Map<Product, ProductViewModel>(product);
                newItem.Quantity = 1;
                cart.Add(newItem);
            }
            Session[CommonConstants.SessionCart] = cart;


            return Json(new
            {
                status = true,
                data = cart
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Update(string cartData)
        {
            var cartViewModel = new JavaScriptSerializer().Deserialize<List<ShoppingCartViewModel>>(cartData);

            var cartSession = GetCart();
            foreach (var item in cartSession)
            {
                foreach (var jitem in cartViewModel)
                {
                    if (item.ProductId == jitem.ProductId)
                    {
                        item.Quantity = jitem.Quantity;
                    }
                }
            }
            Session[CommonConstants.SessionCart] = cartSession;
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult DeleteItem(int productId)
        {
            var cartSession = GetCart();
            if (cartSession != null)
            {
                cartSession.RemoveAll(x => x.ProductId == productId);
                Session[CommonConstants.SessionCart] = cartSession;
                return Json(new
                {
                    status = true
                });
            }
            return Json(new
            {
                status = false,
                data = Session[CommonConstants.SessionCart]
            });
        }

        [HttpPost]
        public JsonResult DeleteAll()
        {
            Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            return Json(new
            {
                status = true
            });
        }
        #endregion

        public ActionResult ConfirmOrder()
        {
            string token = Request["token"];
            RequestCheckOrder info = new RequestCheckOrder();
            info.Merchant_id = _merchantId;
            info.Merchant_password = _merchantPassword;
            info.Token = token;
            APICheckoutV3 objNLChecout = new APICheckoutV3();
            ResponseCheckOrder result = objNLChecout.GetTransactionDetail(info);
            if (result.errorCode == "00")
            {
                //update status order
                _orderService.UpdateStatus(int.Parse(result.order_code));
                _orderService.Save();
                ViewBag.IsSuccess = true;
                ViewBag.Result = "Thanh toán thành công. Chúng tôi sẽ liên hệ lại sớm nhất.";
            }
            else
            {
                ViewBag.IsSuccess = true;
                ViewBag.Result = "Có lỗi xảy ra. Vui lòng liên hệ admin.";
            }
            return View();
        }
        public ActionResult CancelOrder()
        {
            var orderID = (int)TempData["OrderID"];
            _orderService.Delete(orderID);
            _orderService.Save();
            TempData.Remove("OrderID");
            Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            return RedirectToAction("Index","Home");
        }
    }
}