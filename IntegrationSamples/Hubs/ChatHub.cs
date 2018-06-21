using IntegrationSamples.Infrastructure;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace IntegrationSamples.Hubs
{
    public class ChatMessage
    {
        public string message { get; set; }
        public async static Task SendMessage(string message)
        {
            if (string.IsNullOrEmpty(Chat._agentId)) return;
            var notificationHub2 = GlobalHost.ConnectionManager.GetHubContext<Hubs.Chat>();
            await (Task)notificationHub2.Clients.Client(Chat._agentId).Send(new ChatMessage { message = message });
        }
    }
}