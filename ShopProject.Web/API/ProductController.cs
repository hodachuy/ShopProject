using AutoMapper;
using ShopProject.Common;
using ShopProject.Common.Exceptions;
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
    [RoutePrefix("api/product")]
    public class ProductController : ApiControllerBase
    {
        #region
        private IProductService _productService;
        private IFileImageService _fileImageService;
        private IGroupService _groupService;

        public ProductController(IErrorService errorService, IProductService productService, IFileImageService fileImageService, IGroupService groupService) : base(errorService)
        {
            _productService = productService;
            _fileImageService = fileImageService;
            _groupService = groupService;
        }

        #endregion
        [Route("getparent")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var lstProduct = _productService.GetAll();

                //var lstProductVm = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(lstProduct);

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, lstProduct);

                return response;
            });
        }

        [Route("getalltag")]
        [HttpGet]
        public HttpResponseMessage GetAllTag(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var lstTag = _productService.GetAllTag();

                var lstTagVm = Mapper.Map<IEnumerable<Tag>, IEnumerable<TagViewModel>>(lstTag);

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, lstTagVm);

                return response;
            });
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 10)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;

                var lstProduct = _productService.GetAll(keyword);

                foreach (var item in lstProduct)
                {
                    if (!String.IsNullOrEmpty(item.Image))
                    {
                        item.Image = ConvertData.ImageToBase64String(item.Image, CommonConstants.PathProduct);
                    }
                }

                totalRow = lstProduct.Count();

                var query = lstProduct.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);

                //var lstProductVm = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(query);

                var paginationSet = new PaginationSet<Product>()
                {
                    Items = query,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }

        [Route("getbyid/{productID:int}")]
        [HttpGet]
        public HttpResponseMessage GetByID(HttpRequestMessage request, int productID)
        {
            return CreateHttpResponse(request, () =>
            {
                var productItem = _productService.GetById(productID);

                if (!String.IsNullOrEmpty(productItem.Image))
                {
                    productItem.Image = ConvertData.ImageToBase64String(productItem.Image, CommonConstants.PathProduct);
                }

                var productItemVm = new ProductViewModel();
                productItemVm.ID = productItem.ID;
                productItemVm.Alias = productItem.Alias;
                productItemVm.CategoryID = productItem.CategoryID;
                productItemVm.Content = productItem.Content;
                productItemVm.CreatedBy = productItem.CreatedBy;
                productItemVm.CreatedDate = productItem.CreatedDate;
                productItemVm.Description = productItem.Description;
                productItemVm.DiscountFlag = productItem.DiscountFlag;
                productItemVm.HomeFlag = productItem.HomeFlag;
                productItemVm.HotFlag = productItem.HotFlag;
                productItemVm.Image = productItem.Image;
                productItemVm.MetaDescription = productItem.MetaDescription;
                productItemVm.MetaKeyword = productItem.MetaKeyword;
                productItemVm.Name = productItem.Name;
                productItemVm.OriginalPrice = productItem.OriginalPrice;
                productItemVm.Price = productItem.Price;
                productItemVm.ProductNewFlag = productItem.ProductNewFlag;
                productItemVm.PromotionPrice = productItem.PromotionPrice;
                if (!String.IsNullOrEmpty(productItem.Tags))
                {
                    if (!productItem.Tags.Contains(','))
                    {
                        productItemVm.Tags = new[] { productItem.Tags};
                    }
                    else
                    {
                        string[] Tags = productItem.Tags.Split(',');
                        productItemVm.Tags = Tags;
                    }
                }
                else
                {
                    productItemVm.Tags = null;
                }

                productItemVm.UpdatedBy = productItem.UpdatedBy;
                productItemVm.UpdatedDate = productItem.UpdatedDate;
                productItemVm.ViewCount = productItem.ViewCount;
                productItemVm.Warranty = productItem.Warranty;
                productItemVm.Status = productItem.Status;
                productItemVm.Quantity = productItem.Quantity;

                var lstGroup = _groupService.GetListGroupByProductId(productID);
                var lstGroupVm = Mapper.Map<IEnumerable<Group>,IEnumerable<GroupViewModel>>(lstGroup);
                productItemVm.Groups = lstGroupVm;

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, productItemVm);
                return response;
            });
        }

        [Route("create")]
        [HttpPost]
        public HttpResponseMessage Create(HttpRequestMessage request)
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
                    var productJson = HttpContext.Current.Request.Unvalidated.Form["product"];
                    if (productJson == null)
                    {
                        response = request.CreateErrorResponse(HttpStatusCode.BadRequest, "Vui lòng cung cấp dữ liệu.");
                    }

                    var productVm = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 }.Deserialize<ProductViewModel>(productJson);

                    var product = new Product();
                    product.UpdateProduct(productVm);

                    // Xử lý tag
                    if (productVm.TagsVm != null)
                    {
                        string[] mylist = productVm.TagsVm.Select(x => x.Name).ToArray();
                        var checkDuplicate = new HashSet<string>(mylist);
                        product.Tags = string.Join(",", checkDuplicate);
                    }

                    // Convert chuỗi image.
                    if (!String.IsNullOrEmpty(productVm.Image))
                    {
                        string extension = GetFileExtension.GetExtension(productVm.Image);
                        var image = ConvertData.Base64ToImage(productVm.Image);

                        product.Image = product.ID + "_" + product.Name + "." + extension;

                        string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/fileman/Uploads/")
                            + CommonConstants.PathProduct + "/" + product.Image);

                        image.Save(filePath, ImageFormat.Jpeg);
                    }

                    product.CreatedDate = DateTime.Now;
                    product.CreatedBy = User.Identity.Name;

                    _productService.Add(product);
                    _productService.Save();

                    // Xử lý thêm nhóm sản phẩm.
                    if (productVm.Groups != null)
                    {
                        var lstProductGroup = new List<ProductGroup>();
                        foreach (var group in productVm.Groups)
                        {
                            lstProductGroup.Add(new ProductGroup()
                            {
                                ProductID = product.ID,
                                GroupID = group.ID
                            });
                        }
                        _groupService.AddProductToGroups(lstProductGroup, product.ID);
                        _groupService.Save();
                    }
                  

                    // Xử lý thêm nhiều hình ảnh.
                    var files = HttpContext.Current.Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        var file = files[i];
                        string fileExtension = new FileInfo(file.FileName).Extension.ToUpper();
                        string fileName = new FileInfo(file.FileName).Name;
                        if (file.ContentLength > 0)
                        {
                            Guid id = Guid.NewGuid();
                            string modifiedName = id.ToString() + "_" + product.Name + fileExtension;
                            file.SaveAs(Path.Combine(HttpContext.Current.Server.MapPath("~/fileman/Uploads/")
                            + CommonConstants.PathProduct + "/" + modifiedName));
                            var fileImage = new FileImage()
                            {
                                FileName = modifiedName,
                                Extension = fileExtension,
                                ProductImageID = product.ID
                            };
                            _fileImageService.Add(fileImage);
                        }
                    }
                    _fileImageService.Save();

                    var responseData = Mapper.Map<Product, ProductViewModel>(product);
                    response = request.CreateResponse(HttpStatusCode.OK, responseData);
                }

                return response;
            });
        }

        [Route("edit")]
        [HttpPut]
        public HttpResponseMessage Update(HttpRequestMessage request)
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
                    var productJson = HttpContext.Current.Request.Unvalidated.Form["product"];
                    if (productJson == null)
                    {
                        response = request.CreateErrorResponse(HttpStatusCode.BadRequest, "Vui lòng cung cấp dữ liệu.");
                    }

                    var productVm = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 }.Deserialize<ProductViewModel>(productJson);
                    var product = new Product();
                    product.UpdateProduct(productVm);
                    
                    // Xử lý tag.
                    if (productVm.TagsVm != null)
                    {
                        string[] mylist = productVm.TagsVm.Select(x => x.Name).ToArray();
                        var checkDuplicate = new HashSet<string>(mylist);
                        product.Tags = string.Join(",", checkDuplicate);
                    }

                    // Convert chuỗi image.
                    if (!String.IsNullOrEmpty(productVm.Image))
                    {
                        string extension = GetFileExtension.GetExtension(productVm.Image);
                        var image = ConvertData.Base64ToImage(productVm.Image);

                        product.Image = product.ID + "_" + product.Name + "." + extension;

                        string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/fileman/Uploads/")
                            + CommonConstants.PathProduct + "/" + product.Image);
                        image.Save(filePath, ImageFormat.Jpeg);
                    }

                    product.UpdatedDate = DateTime.Now;
                    product.UpdatedBy = User.Identity.Name;

                    _productService.Update(product);
                    _productService.Save();

                    // Xử lý nhóm sản phẩm.
                    if (productVm.Groups != null)
                    {
                        var lstProductGroup = new List<ProductGroup>();
                        foreach (var group in productVm.Groups)
                        {
                            lstProductGroup.Add(new ProductGroup()
                            {
                                ProductID = product.ID,
                                GroupID = group.ID
                            });
                        }
                        _groupService.AddProductToGroups(lstProductGroup, product.ID);
                        _groupService.Save();
                    }
                  

                    //if (productVm.MoreImages != null)
                    //{
                    //    foreach (var item in productVm.MoreImages)
                    //    {
                    //        if (!String.IsNullOrEmpty(item))
                    //        {
                    //            var image = ConvertData.Base64ToImage(item);
                    //            string extension = GetFileExtension.GetExtension(item);
                    //            Guid id = Guid.NewGuid();

                    //            string fileName = id.ToString() + "_" + product.Name + "." + extension;

                    //            string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/fileman/Uploads/")
                    //                + CommonConstants.PathProduct + "/" + fileName);

                    //            image.Save(filePath, ImageFormat.Jpeg);

                    //            _fileImageService.DeleteMutiImage(product.ID);

                    //            var fileImage = new FileImage()
                    //            {
                    //                FileName = fileName,
                    //                Extension = "." + extension,
                    //                ProdutImageID = product.ID
                    //            };

                    //            _fileImageService.Add(fileImage);
                    //        }
                    //    }
                    //}
                    //_fileImageService.Save();

                    var responseData = Mapper.Map<Product, ProductViewModel>(product);
                    response = request.CreateResponse(HttpStatusCode.OK, responseData);
                }

                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage Delete(HttpRequestMessage request, int productID)
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
                    var product = _productService.Delete(productID);
                    _productService.Save();

                    var responseData = Mapper.Map<Product, ProductViewModel>(product);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }
                return response;
            });
        }

        [Route("deletemulti")]
        [HttpDelete]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string lstProductID)
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
                    var lstItemID = new JavaScriptSerializer().Deserialize<List<int>>(lstProductID);
                    foreach (var item in lstItemID)
                    {
                        _productService.Delete(item);
                    }
                    _productService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, lstItemID.Count);
                }
                return response;
            });
        }
    }
}