using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IntegrationSamples.Hubs;
using IntegrationSamples.Infrastructure;
using IntegrationSamples.Models;
using Microsoft.AspNet.SignalR;

namespace IntegrationSamples.Controllers
{
    public class FBBotController : Controller
    {
        private ChatService chatService = new ChatService();
        private FBService fBService = new FBService();

        // GET: FBBot
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Receive()
        {
            var query = Request.QueryString;

            //_logWriter.WriteLine(Request.RawUrl);

            if (query["hub.mode"] == "subscribe" &&
                query["hub.verify_token"] == AppSettings.verify_token)
            {
                //string type = Request.QueryString["type"];
                var retVal = query["hub.challenge"];
                return Json(int.Parse(retVal), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return HttpNotFound("Receive Failed");
            }
        }

        [ActionName("Receive")]
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> ReceivePost(BotRequest data)
        {
            foreach (var entry in data.entry)
            {
                foreach (var message in entry.messaging)
                {
                    if (string.IsNullOrWhiteSpace(message?.message?.text))
                        continue;

                    var fbmsg = message.message.text;
                    try
                    {
                        Chat._fbUserId = message.sender.id;
                        var chatMessage = chatService.AddFBMessage(message.sender.id, fbmsg);

                        await Hubs.ChatMessage.SendMessage(chatMessage.Text);

                        if (chatMessage.ChatRoom.AgentId != null)
                        {
                            //var chatUser = chatService.GetChatUserByAgentId(chatMessage.ChatRoom.AgentId);
                            //if (chatUser != null)
                            //{
                                var notificationHub = GlobalHost.ConnectionManager.GetHubContext<Hubs.Chat>();
                                //foreach (var connectedClient in chatUser.ConnectedClients)
                                //{
                                //    await (Task)notificationHub.Clients.Client(connectedClient.ConnectionId).addNewMessageToPage("Facebook User", chatMessage.Text);
                                //}
                            await (Task)notificationHub.Clients.Client(chatMessage.ChatRoom.ConnectionId).addNewMessageToPage("Facebook User", chatMessage.Text);
                            
                            //}
                        }
                    }
                    catch (Exception ex)
                    {
                        fBService.Send(message.sender.id, $"Error: {ex.Message}. Please wait few minutes!");
                    }
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult TestReceive()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> TestReceive(IntegrationSamples.Models.ChatMessage msg)
        {
            var chatMessage = chatService.AddFBMessage(msg.Platform, msg.Text);

            if (chatMessage.ChatRoom.AgentId != null)
            {
                //var chatUser = chatService.GetChatUserByAgentId(chatMessage.ChatRoom.AgentId);
                //if (chatUser != null)
                //{
                var notificationHub = GlobalHost.ConnectionManager.GetHubContext<Hubs.Chat>();
                //foreach (var connectedClient in chatUser.ConnectedClients)
                //{
                //    await (Task)notificationHub.Clients.Client(connectedClient.ConnectionId).addNewMessageToPage("Facebook User", chatMessage.Text);
                //}
                await (Task)notificationHub.Clients.Client(chatMessage.ChatRoom.AgentId).addNewMessageToPage("Facebook User", chatMessage.Text);
                //}
            }
            return View(msg);
        }
    }
}