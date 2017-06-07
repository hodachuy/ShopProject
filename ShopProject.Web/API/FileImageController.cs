using AutoMapper;
using ShopProject.Common;
using ShopProject.Model.Models;
using ShopProject.Service;
using ShopProject.Web.Infrastructure.Core;
using ShopProject.Web.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ShopProject.Web.API
{
    [Authorize]
    [RoutePrefix("api/fileimage")]
    public class FileImageController : ApiControllerBase
    {
        IFileImageService _fileImageService;
        public FileImageController(IErrorService errorService, IFileImageService fileImageService) : base(errorService)
        {
            _fileImageService = fileImageService;
        }

        [Route("getmultiImage/{productID:int}")]
        [HttpGet]
        public HttpResponseMessage GetMultiImage(HttpRequestMessage request, int productID)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                var lstImage = _fileImageService.GetByProductID(productID);
                foreach (var image in lstImage)
                {
                    if (!String.IsNullOrEmpty(image.FileName))
                    {
                        image.FileName = ConvertData.ImageToBase64String(image.FileName, CommonConstants.PathProduct);
                    }
                }

                var lstImageVm = Mapper.Map<IEnumerable<FileImage>, IEnumerable<FileImageViewModel>>(lstImage);
                response = request.CreateResponse(HttpStatusCode.OK, lstImage);
                return response;
            });
        }

        [Route("create")]
        [HttpPost]
        public HttpResponseMessage Create(HttpRequestMessage request, List<FileImageViewModel> fileImageVm)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
               
                foreach (var item in fileImageVm)
                {
                    _fileImageService.DeleteMutiImage(item.ProductImageID);

                    if (!String.IsNullOrEmpty(item.FileName))
                    {
                        var image = ConvertData.Base64ToImage(item.FileName);
                        string extension = GetFileExtension.GetExtension(item.FileName);
                        Guid id = Guid.NewGuid();
                        string time = DateTime.Now.Millisecond.ToString();
                        string fileName = id.ToString() + time + "." + extension;

                        string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/fileman/Uploads/")
                            + CommonConstants.PathProduct + "/" + fileName);
                        image.Save(filePath, ImageFormat.Jpeg);

                        //_fileImageService.DeleteMutiImage(item.ProductImageID);

                        var fileImage = new FileImage();
                        fileImage.FileName = fileName;
                        fileImage.Extension = "." + extension;
                        fileImage.ProductImageID = item.ProductImageID;

                        _fileImageService.Add(fileImage);

                        response = request.CreateResponse(HttpStatusCode.OK, fileImage);
                    }

                }
                _fileImageService.Save();

                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage Delete(HttpRequestMessage request, int fileImageID)
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
                    var fileImage = _fileImageService.Delete(fileImageID);
                    _fileImageService.Save();
                    response = request.CreateResponse(HttpStatusCode.Created, fileImage);
                }

                return response;
            });
        }
    }
}