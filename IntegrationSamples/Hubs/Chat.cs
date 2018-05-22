using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using IntegrationSamples.Infrastructure;
using Microsoft.AspNet.SignalR;

namespace IntegrationSamples.Hubs
{
    public class Chat : Hub
    {
        private ChatService messageService = new ChatService();
        private FBService fBService = new FBService();

        public void Hello()
        {
            Clients.All.hello();
        }

        public void Send(string message)
        {
            //Clients.All.addNewMessageToPage(name, message);
            var chatMessage = messageService.AddMessage(this.Context.ConnectionId, message);
            fBService.Send(chatMessage);
        }

        public void CloseChat()
        {
            messageService.CloseChatRoom(this.Context.ConnectionId);
            Clients.Caller.addNewMessageToPage("Info", "chat is closed!");
        }

        public void OpenChat()
        {
            var room = messageService.Open(this.Context.ConnectionId, this.Context.ConnectionId);
            Clients.Caller.addNewMessageToPage("Info", "chat is ready!");

            foreach (var item in room.GetUnreadUserMessages())
            {
                Clients.Caller.addNewMessageToPage("Facebook User", item.Text);
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            messageService.Disconnect(this.Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }
    }
}