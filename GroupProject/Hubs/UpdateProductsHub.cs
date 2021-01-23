using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupProject.Hubs
{
    [HubName("updateProductsHub")]
    public class UpdateProductsHub : Hub
    {
        public static void BroadcastData()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<UpdateProductsHub>();
            context.Clients.All.refreshCartData();
        }
    }
}