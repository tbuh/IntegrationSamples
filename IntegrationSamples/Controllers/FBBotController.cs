using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IntegrationSamples.Infrastructure;
using IntegrationSamples.Models;
using Microsoft.AspNet.SignalR;

namespace IntegrationSamples.Controllers
{
    public class FBBotController : Controller
    {
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
        public ActionResult ReceivePost(BotRequest data)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (var entry in data.entry)
                {
                    foreach (var message in entry.messaging)
                    {
                        if (string.IsNullOrWhiteSpace(message?.message?.text))
                            continue;

                        var msg = message.message.text;

                        try
                        {
                            var notificationHub = GlobalHost.ConnectionManager.GetHubContext<Hubs.Chat>();
                            notificationHub.Clients.All.addNewMessageToPage(message.sender.id, msg);
                        }
                        catch (Exception ex)
                        {
                            new SendFBMessage().Send(message.sender.id, $"Error: {ex.Message}. Please wait few minutes!");
                        }
                    }
                }
            });

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }        
    }
}