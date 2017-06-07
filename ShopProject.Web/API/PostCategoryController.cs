using AutoMapper;
using ShopProject.Model.Models;
using ShopProject.Service;
using ShopProject.Web.Infrastructure.Core;
using ShopProject.Web.Infrastructure.Extensions;
using ShopProject.Web.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ShopProject.Web.API
{
    [RoutePrefix("api/postcategory")]
    [Authorize]
    public class PostCategoryController : ApiControllerBase
    {
        IPostCategoryService _postCategoryService;

        public PostCategoryController(IErrorService errorService, IPostCategoryService postCategoryService) :
            base(errorService)
        {
            this._postCategoryService = postCategoryService;
        }
        [Route("getall")]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var listpostCategory = _postCategoryService.GetAll();
                var postCategoryVm = Mapper.Map<IEnumerable<PostCategory>,IEnumerable<PostCategoryViewModel>>(listpostCategory);
                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, postCategoryVm);
                return response;
            });
        }

        [Route("add")]
        public HttpResponseMessage Post(HttpRequestMessage request, PostCategoryViewModel postCategoryVm)
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
                    PostCategory postCategory = new PostCategory();
                    postCategory.UpdatePostCategory(postCategoryVm);
                    var category = _postCategoryService.Add(postCategory);
                    _postCategoryService.Save();
                    response = request.CreateResponse(HttpStatusCode.Created, category);
                }

                return response;
            });
        }

        [Route("update")]
        public HttpResponseMessage Put(HttpRequestMessage request, PostCategoryViewModel postCategoryVm)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (ModelState.IsValid)
                {
                    request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var categoryDB = _postCategoryService.GetById(postCategoryVm.ID);
                    categoryDB.UpdatePostCategory(postCategoryVm);
                    _postCategoryService.Update(categoryDB);
                    _postCategoryService.Save();
                    response = request.CreateResponse(HttpStatusCode.OK);
                }

                return response;
            });
        }

        [Route("delete")]
        public HttpResponseMessage Delete(HttpRequestMessage request, int postCategoryID)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (ModelState.IsValid)
                {
                    request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var category = _postCategoryService.Delete(postCategoryID);
                    _postCategoryService.Save();
                    response = request.CreateResponse(HttpStatusCode.OK);
                }

                return response;
            });
        }
    }
}