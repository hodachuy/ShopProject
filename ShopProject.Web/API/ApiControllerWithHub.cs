using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using ShopProject.Service;
using ShopProject.Web.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ShopProject.Web.API
{
    public class ApiControllerWithHub<THub> : ApiControllerBase where THub : IHub
    {
        public ApiControllerWithHub(IErrorService errorService) : base(errorService)
        {

        }
        Lazy<IHubContext> hub = new Lazy<IHubContext>(
           () => GlobalHost.ConnectionManager.GetHubContext<THub>()
        );

        protected IHubContext Hub
        {
            get { return hub.Value; }
        }
    }
}