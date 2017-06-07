using ShopProject.Service;
using ShopProject.Web.Infrastructure.Core;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ShopProject.Web.API
{
    [Authorize]
    [RoutePrefix("api/statistic")]
    public class StatisticController : ApiControllerBase
    {
        IStatisticService _statisticService;

        public StatisticController(IErrorService errorService, IStatisticService statisticService) : base(errorService)
        {
            _statisticService = statisticService;
        }

        [Route("getrevenue")]
        [HttpGet]
        public HttpResponseMessage GetRevenueStatistic(HttpRequestMessage request, string fromDate, string toDate)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var statistic = _statisticService.GetRevenueStatistic(fromDate, toDate);
                response = request.CreateResponse(HttpStatusCode.OK, statistic);
                return response;
            });
        }
    }
}
