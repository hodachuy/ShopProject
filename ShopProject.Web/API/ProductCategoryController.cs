using AutoMapper;
using ShopProject.Common;
using ShopProject.Model.Models;
using ShopProject.Service;
using ShopProject.Web.Infrastructure.Core;
using ShopProject.Web.Infrastructure.Extensions;
using ShopProject.Web.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace ShopProject.Web.API
{
    [Authorize]
    [RoutePrefix("api/productcategory")]
    public class ProductCategoryController : ApiControllerBase
    {
        #region
        private IProductCategoryService _productCategoryService;

        public ProductCategoryController(IErrorService errorService, IProductCategoryService productCategoryService) : base(errorService)
        {
            _productCategoryService = productCategoryService;
        }

        #endregion

        /// <summary>
        /// Get all name ProductCategory for method seleclist.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("getParent")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var listproductCategory = _productCategoryService.GetAll();

                // map chuyen doi du lieu tu model toi du lieu trang view model
                var productCategoryVm = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(listproductCategory);

                // hien thi du lieu pageinationSet json
                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, productCategoryVm);

                return response;
            });
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize)
        {

            return CreateHttpResponse(request, () =>
             {
                 int totalRow = 0;

                 var listproductCategory = _productCategoryService.GetAll(keyword);

                 foreach (var item in listproductCategory)
                 {
                     if (!String.IsNullOrEmpty(item.Image))
                     {
                         item.Image = ConvertData.ImageToBase64String(item.Image, CommonConstants.PathProductCategory);
                     }

                 }


                 totalRow = listproductCategory.Count();

                 // lay so ban ghi su dung de phan trang tu du lieu cua model.
                 var query = listproductCategory.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);

                 // map chuyen doi du lieu tu model toi du lieu trang view model
                 var productCategoryVm = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(query);

                 // Khai bao paginationSet de the hien du lieu json tren web api voi cac thong tin ve page,totalcount,totalpage,ienumerable<model>
                 var paginationSet = new PaginationSet<ProductCategoryViewModel>()
                 {
                     Items = productCategoryVm,
                     Page = page,
                     TotalCount = totalRow,
                     TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                 };

                 // hien thi du lieu pageinationSet json
                 HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, paginationSet);

                 return response;
             });
        }


        /// <summary>
        /// Get data with infor id of ProductCategory.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="productCategoryID"> infor id </param>
        /// <returns></returns>
        [Route("getbyid/{productCategoryID:int}")]
        [HttpGet]
        public HttpResponseMessage GetID(HttpRequestMessage request, int productCategoryID)
        {
            return CreateHttpResponse(request, () =>
            {
                var productCategory = _productCategoryService.GetById(productCategoryID);
                if (!String.IsNullOrEmpty(productCategory.Image))
                {
                    // Convert image to base 64 string.
                    productCategory.Image = ConvertData.ImageToBase64String(productCategory.Image, CommonConstants.PathProductCategory);
                }
               

                // Map chuyen doi du lieu tu model toi du lieu trang view model.
                var productCategoryVm = Mapper.Map<ProductCategory, ProductCategoryViewModel>(productCategory);

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, productCategoryVm);

                return response;
            });
        }

        [Route("create")]
        [HttpPost]
        public HttpResponseMessage Create(HttpRequestMessage request, ProductCategoryViewModel productCategoryVm)
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

                     ProductCategory productCategory = new ProductCategory();

                     // Chuyen doi du lieu tu trang view toi du lieu cua model
                     productCategory.UpdateProductCategory(productCategoryVm);
                   
                     if (!String.IsNullOrEmpty(productCategoryVm.Image))
                     {
                         var image = ConvertData.Base64ToImage(productCategoryVm.Image);

                         productCategory.Image = productCategory.ID + "_" + productCategory.Name + ".jpg";

                         string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/fileman/Uploads/")
                             + CommonConstants.PathProductCategory + "/" + productCategory.Image);

                         image.Save(filePath, ImageFormat.Jpeg);
                     }

                     productCategory.CreatedDate = DateTime.Now;

                     // sau khi chuyen doi ta add du lieu
                     _productCategoryService.Add(productCategory);
                     _productCategoryService.Save();

                     //Sau khi luu xong chuyen doi lai wa du lieu cua view model de hien thi json
                     var responseData = Mapper.Map<ProductCategory, ProductCategoryViewModel>(productCategory);
                     response = request.CreateResponse(HttpStatusCode.OK, responseData);
                 }

                 return response;
             });
        }

        [Route("edit")]
        [HttpPut]
        public HttpResponseMessage Update(HttpRequestMessage request, ProductCategoryViewModel productCategoryVm)
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
                    ProductCategory productCategory = new ProductCategory();

                    // Chuyen doi du lieu tu trang view toi du lieu cua model
                    productCategory.UpdateProductCategory(productCategoryVm);
                    if (!String.IsNullOrEmpty(productCategoryVm.Image))
                    {
                        var image = ConvertData.Base64ToImage(productCategoryVm.Image);

                        productCategory.Image = productCategory.ID + "_" + productCategory.Name + ".jpg" ;

                        string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/fileman/Uploads/") + CommonConstants.PathProductCategory + "/" + productCategory.Image);

                        image.Save(filePath, ImageFormat.Jpeg);
                    }
                    //File.WriteAllBytes(string path,byte[] bytes);

                    productCategory.UpdatedDate = DateTime.Now;

                    // sau khi chuyen doi ta add du lieu
                    _productCategoryService.Update(productCategory);
                    _productCategoryService.Save();

                    //Sau khi luu xong chuyen doi lai wa du lieu cua view model de hien thi json
                    var responseData = Mapper.Map<ProductCategory, ProductCategoryViewModel>(productCategory);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage Delete(HttpRequestMessage request, int productCategoryID)
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
                    var productCategory = _productCategoryService.Delete(productCategoryID);
                    _productCategoryService.Save();

                    //Sau khi luu xong chuyen doi lai wa du lieu cua view model de hien thi json
                    var responseData = Mapper.Map<ProductCategory, ProductCategoryViewModel>(productCategory);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }
                return response;
            });
        }

        [Route("deletemulti")]
        [HttpDelete]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string lstProductCategoryID)
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
                    var lstItemID = new JavaScriptSerializer().Deserialize<List<int>>(lstProductCategoryID);
                    foreach (var item in lstItemID)
                    {
                        _productCategoryService.Delete(item);
                    }
                    _productCategoryService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, lstItemID.Count);
                }
                return response;
            });
        }
    }
}