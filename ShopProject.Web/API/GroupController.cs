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
    [RoutePrefix("api/group")]
    [Authorize]
    public class GroupController : ApiControllerBase
    {
        IGroupService _groupService;
        public GroupController(IErrorService errorService, IGroupService groupService) :base(errorService)
        {
            _groupService = groupService;
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var lstGroups = _groupService.GetAll();
                var lstGroupVm = Mapper.Map<IEnumerable<Group>, IEnumerable<GroupViewModel>>(lstGroups);
                response = request.CreateResponse(HttpStatusCode.OK,lstGroupVm);
                return response;
            });
        }

        [Route("getallpage")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize)
        {

            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;

                var listGroups = _groupService.GetAll(keyword);

                foreach (var item in listGroups)
                {
                    if (!String.IsNullOrEmpty(item.Image))
                    {
                        item.Image = ConvertData.ImageToBase64String(item.Image, CommonConstants.PathProductCategory);
                    }

                }


                totalRow = listGroups.Count();

                // lay so ban ghi su dung de phan trang tu du lieu cua model.
                var query = listGroups.Skip(page * pageSize).Take(pageSize);

                // map chuyen doi du lieu tu model toi du lieu trang view model
                var groupVm = Mapper.Map<IEnumerable<Group>, IEnumerable<GroupViewModel>>(query);

                // Khai bao paginationSet de the hien du lieu json tren web api voi cac thong tin ve page,totalcount,totalpage,ienumerable<model>
                var paginationSet = new PaginationSet<GroupViewModel>()
                {
                    Items = groupVm,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };

                // hien thi du lieu pageinationSet json
                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, paginationSet);

                return response;
            });
        }

        [Route("getbyid/{groupID:int}")]
        [HttpGet]
        public HttpResponseMessage GetID(HttpRequestMessage request, int groupID)
        {
            return CreateHttpResponse(request, () =>
            {
                var groupProduct = _groupService.GetByID(groupID);
                if (!String.IsNullOrEmpty(groupProduct.Image))
                {
                    // Convert image to base 64 string.
                    groupProduct.Image = ConvertData.ImageToBase64String(groupProduct.Image, CommonConstants.PathProductCategory);
                }
                // Map chuyen doi du lieu tu model toi du lieu trang view model.
                var groupProductVm = Mapper.Map<Group, GroupViewModel>(groupProduct);

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, groupProductVm);

                return response;
            });
        }

        [Route("create")]
        [HttpPost]
        public HttpResponseMessage Create(HttpRequestMessage request,GroupViewModel groupVm)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    request.CreateErrorResponse(HttpStatusCode.BadRequest,ModelState);
                }
                else
                {
                    try
                    {
                        var groupDb = new Group();
                        groupDb.UpdateGroup(groupVm);

                        if (!String.IsNullOrEmpty(groupVm.Image))
                        {
                            var image = ConvertData.Base64ToImage(groupVm.Image);

                            groupDb.Image = groupDb.ID + "_" + groupDb.Name + ".jpg";

                            string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/fileman/Uploads/")
                                + CommonConstants.PathProductCategory + "/" + groupDb.Image);

                            image.Save(filePath, ImageFormat.Jpeg);

                        }

                        _groupService.Add(groupDb);
                        _groupService.Save();

                        var responseData = Mapper.Map<Group, GroupViewModel>(groupDb);
                        response = request.CreateResponse(HttpStatusCode.OK, responseData);
                    }
                    catch (Exception ex)
                    {
                        response = request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
                    }

                }
                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        public HttpResponseMessage Update(HttpRequestMessage request, GroupViewModel groupVm)
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
                    try
                    {
                        var groupDb = new Group();
                        groupDb.UpdateGroup(groupVm);

                        if (!String.IsNullOrEmpty(groupVm.Image))
                        {
                            var image = ConvertData.Base64ToImage(groupVm.Image);

                            groupDb.Image = groupDb.ID + "_" + groupDb.Name + ".jpg";

                            string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/fileman/Uploads/")
                                + CommonConstants.PathProductCategory + "/" + groupDb.Image);

                            image.Save(filePath, ImageFormat.Jpeg);

                        }

                        _groupService.Update(groupDb);
                        _groupService.Save();

                        var responseData = Mapper.Map<Group, GroupViewModel>(groupDb);
                        response = request.CreateResponse(HttpStatusCode.OK, responseData);
                    }
                    catch (Exception ex)
                    {
                        response = request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
                    }
                }
                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage Delete(HttpRequestMessage request, int groupID)
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
                    var group = _groupService.Delete(groupID);
                    _groupService.Save();

                    //Sau khi luu xong chuyen doi lai wa du lieu cua view model de hien thi json
                    var responseData = Mapper.Map<Group, GroupViewModel>(group);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }
                return response;
            });
        }

        [Route("deletemulti")]
        [HttpDelete]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string lstGroupID)
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
                    var lstItemID = new JavaScriptSerializer().Deserialize<List<int>>(lstGroupID);
                    foreach (var item in lstItemID)
                    {
                        _groupService.Delete(item);
                    }
                    _groupService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, lstItemID.Count);
                }
                return response;
            });
        }
    }
}