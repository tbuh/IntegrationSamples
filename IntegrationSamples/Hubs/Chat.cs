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
        private GamificationService gamificationService = new GamificationService();

        public void Hello()
        {
            Clients.All.hello();
        }

        public void Send(string message)
        {
            //Clients.All.addNewMessageToPage(name, message);
            var chatMessage = messageService.AddMessage(this.Context.ConnectionId, message);
            if (chatMessage == null)
            {
                Clients.Caller.addNewMessageToPage("Info", "Go online!");
            }
            else
            {
                try
                {
                    fBService.Send(chatMessage);
                    gamificationService.AddPoint(chatMessage.ChatRoom.AgentId);
                }
                catch (Exception ex)
                {
                    Clients.Caller.addNewMessageToPage("Info", ex.Message);
                }                
            }
        }

        public void CloseChat()
        {
            messageService.CloseChatRoom(this.Context.ConnectionId);
            Clients.Caller.addNewMessageToPage("Info", "chat is closed!");
        }

        public void OpenChat(string agentId)
        {
            var room = messageService.Open(agentId, this.Context.ConnectionId);
            Clients.Caller.addNewMessageToPage("Info", "chat is ready!");

            foreach (var item in room.GetUnreadUserMessages())
            {
                Clients.Caller.addNewMessageToPage("Facebook User", item.Text);
            }
        }

        public void BotSupport(string question)
        {
            QuestionService qs = new QuestionService();
            Clients.Caller.addBotMessageToPage("Bot:", qs.GetAnswer(question));
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            messageService.CloseChatRoom(this.Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }
    }
}