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
    [RoutePrefix("api/slide")]
    public class SlideController : ApiControllerBase
    {
        ICommonService _commomService;
        public SlideController(IErrorService errorService, ICommonService commomService) : base(errorService)
        {
            _commomService = commomService;
        }
        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 10)
        {
            return CreateHttpResponse(request, () => {
                HttpResponseMessage response = null;
                int totalRow = 0;

                var lstSlide = _commomService.GetAll(keyword);
                foreach (var item in lstSlide)
                {
                    if (!String.IsNullOrEmpty(item.Image))
                    {
                        item.Image = ConvertData.ImageToBase64String(item.Image, CommonConstants.PathProductCategory);
                    }

                }

                totalRow = lstSlide.Count();

                var query = lstSlide.Skip(page * pageSize).Take(pageSize);

                var lstSlideVm = Mapper.Map<IEnumerable<Slide>, IEnumerable<SlideViewModel>>(query);

                var paginationSet = new PaginationSet<SlideViewModel>()
                {
                    Items = lstSlideVm,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };

                response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;

            });
        }

        [Route("create")]
        [HttpPost]
        public HttpResponseMessage Create(HttpRequestMessage request, SlideViewModel SlideVm)
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

                    Slide slideDb = new Slide();

                    // Chuyen doi du lieu tu trang view toi du lieu cua model
                    slideDb.UpdateSlide(SlideVm);

                    if (!String.IsNullOrEmpty(SlideVm.Image))
                    {
                        var image = ConvertData.Base64ToImage(SlideVm.Image);

                        slideDb.Image = slideDb.ID + "_" + slideDb.Name + ".jpg";

                        string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/fileman/Uploads/")
                            + CommonConstants.PathProductCategory + "/" + slideDb.Image);

                        image.Save(filePath, ImageFormat.Jpeg);
                    }


                    // sau khi chuyen doi ta add du lieu
                    _commomService.Add(slideDb);
                    _commomService.Save();

                    //Sau khi luu xong chuyen doi lai wa du lieu cua view model de hien thi json
                    var responseData = Mapper.Map<Slide, SlideViewModel>(slideDb);
                    response = request.CreateResponse(HttpStatusCode.OK, responseData);
                }

                return response;
            });
        }

        [Route("getbyid/{slideID:int}")]
        [HttpGet]
        public HttpResponseMessage GetID(HttpRequestMessage request, int slideID)
        {
            return CreateHttpResponse(request, () =>
            {
                var slide = _commomService.GetByIdSlide(slideID);
                if (!String.IsNullOrEmpty(slide.Image))
                {
                    // Convert image to base 64 string.
                    slide.Image = ConvertData.ImageToBase64String(slide.Image, CommonConstants.PathProductCategory);
                }


                // Map chuyen doi du lieu tu model toi du lieu trang view model.
                var slideVm = Mapper.Map<Slide, SlideViewModel>(slide);

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, slideVm);

                return response;
            });
        }

        [Route("edit")]
        [HttpPut]
        public HttpResponseMessage Update(HttpRequestMessage request, SlideViewModel slideVm)
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
                    Slide slideDB = new Slide();

                    // Chuyen doi du lieu tu trang view toi du lieu cua model
                    slideDB.UpdateSlide(slideVm);
                    if (!String.IsNullOrEmpty(slideVm.Image))
                    {
                        var image = ConvertData.Base64ToImage(slideVm.Image);

                        slideDB.Image = slideDB.ID + "_" + slideDB.Name + ".jpg";

                        string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/fileman/Uploads/") + CommonConstants.PathProductCategory + "/" + slideDB.Image);

                        image.Save(filePath, ImageFormat.Jpeg);
                    }

                    // sau khi chuyen doi ta add du lieu
                    _commomService.Update(slideDB);
                    _commomService.Save();

                    //Sau khi luu xong chuyen doi lai wa du lieu cua view model de hien thi json
                    var responseData = Mapper.Map<Slide, SlideViewModel>(slideDB);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage Delete(HttpRequestMessage request, int slideID)
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
                    var slide = _commomService.Delete(slideID);
                    _commomService.Save();

                    //Sau khi luu xong chuyen doi lai wa du lieu cua view model de hien thi json
                    var responseData = Mapper.Map<Slide, SlideViewModel>(slide);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }
                return response;
            });
        }

        [Route("deletemulti")]
        [HttpDelete]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string lstSlideID)
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
                    var lstItemID = new JavaScriptSerializer().Deserialize<List<int>>(lstSlideID);
                    foreach (var item in lstItemID)
                    {
                        _commomService.Delete(item);
                    }
                    _commomService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, lstItemID.Count);
                }
                return response;
            });
        }

    }
}