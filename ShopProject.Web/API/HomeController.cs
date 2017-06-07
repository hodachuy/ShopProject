using ShopProject.Service;
using ShopProject.Web.Infrastructure.Core;
using System.Web.Http;

namespace ShopProject.Web.API
{
    [Authorize]
    [RoutePrefix("api/home")]
    public class HomeController : ApiControllerBase
    {
        IErrorService _errorService;
        public HomeController(IErrorService errorService) : base(errorService)
        {
            this._errorService = errorService;
        }

        [HttpGet]
        [Route("TestMethod")]
        public string TestMethod()
        {
            return "Hello, Shop Member. ";
        }
    }
}