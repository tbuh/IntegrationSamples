using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IntegrationSamples.Infrastructure;
using Microsoft.AspNet.SignalR;

namespace IntegrationSamples.Hubs
{
    public class Chat : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

        public void Send(string name, string message)
        {
            //Clients.All.addNewMessageToPage(name, message);
            new SendFBMessage().Send(name, message);
        }
    }
}