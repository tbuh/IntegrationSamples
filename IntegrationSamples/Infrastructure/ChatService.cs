using IntegrationSamples.Models;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

namespace IntegrationSamples.Infrastructure
{
    public class ChatService
    {
        //private static CloudQueue _cloudQueue;
        private ISDbContext iSDbContext;
        public ChatService()
        {
            iSDbContext = new ISDbContext();
            //_cloudQueue = GetCloudQueue();
        }

        //public static void Init()
        //{
        //    _cloudQueue.CreateIfNotExists();
        //}
        public ChatMessage AddFBMessage(string userId, string messageText)
        {
            var room = GetChatRoomByUserId(userId);
            if (room == null)
            {
                room = new ChatRoom
                {
                    IsClosed = false,
                    OpenDate = DateTime.Now,
                    UserId = userId,
                    ChatMessages = new List<ChatMessage>(),
                };
                iSDbContext.Entry(room).State = EntityState.Added;
            }
            else if (room.UserId == null)
            {
                room.UserId = userId;
                iSDbContext.Entry(room).State = EntityState.Modified;
            }

            var chatMessage = new ChatMessage()
            {
                ChatRoom = room,
                AddedOn = DateTime.Now,
                IsFromPlatform = true,
                Platform = "FB",
                Text = messageText
            };

            iSDbContext.Entry(chatMessage).State = EntityState.Added;

            iSDbContext.SaveChanges();

            return chatMessage;
        }

        public ChatMessage AddMessage(string connectionId, string messageText)
        {
            var chatClient = GetChatClient(connectionId);
            var room = GetChatRoomByAgentIdOrAvailable(chatClient.ChatUser.AgentId);

            ChatMessage m = new ChatMessage
            {
                IsFromPlatform = false,
                Text = messageText,
                ChatRoom = room,
                AddedOn = DateTime.Now
            };
            iSDbContext.Entry(m).State = EntityState.Added;
            iSDbContext.SaveChanges();
            return m;
        }

        public void CloseChatRoom(string agentId)
        {
            var room = GetChatRoomByAgentIdOrAvailable(agentId);
            room.CloseDate = DateTime.Now;
            room.IsClosed = true;
            iSDbContext.Entry(room).State = EntityState.Modified;
            iSDbContext.SaveChanges();
        }

        public void Disconnect(string connectionId)
        {
            var chatClient = GetChatClient(connectionId);
            if (chatClient == null) return;

            var user = chatClient.ChatUser;
            user.ConnectedClients.Remove(chatClient);
            if (!user.ConnectedClients.Any())
            {
                user.IsOnline = false;
                iSDbContext.Entry(user).State = EntityState.Modified;
            }
            iSDbContext.Entry(chatClient).State = EntityState.Deleted;

            iSDbContext.SaveChanges();
        }

        public ChatRoom Open(string agentId, string connectionId)
        {
            var user = iSDbContext.GetChatUser(agentId);
            if (user == null)
            {
                user = new ChatUser()
                {
                    AgentId = agentId,
                    IsOnline = true,
                    ConnectedClients = new List<ChatClient>()
                };
                iSDbContext.Entry(user).State = EntityState.Added;
            }
            else
            {
                user.IsOnline = true;
                if (!user.ConnectedClients.Any(cc => cc.ConnectionId == connectionId))
                {
                    var uConnection = new ChatClient { ChatUser = user, ConnectionId = connectionId };
                    user.ConnectedClients.Add(uConnection);
                    iSDbContext.Entry(uConnection).State = EntityState.Added;
                }
            }

            var room = GetChatRoomByAgentIdOrAvailable(agentId);
            if (room == null)
            {
                room = new ChatRoom
                {
                    IsClosed = false,
                    OpenDate = DateTime.Now,
                    AgentId = agentId,
                    ChatMessages = new List<ChatMessage>(),
                };
                iSDbContext.Entry(room).State = EntityState.Added;
            }
            else if (room.AgentId == null)
            {
                room.AgentId = agentId;
                iSDbContext.Entry(room).State = EntityState.Modified;
            }            

            iSDbContext.SaveChanges();
            return room;
        }

        private ChatClient GetChatClient(string connectionId)
        {
            var chatClient = iSDbContext.ChatClients.Include(c => c.ChatUser).SingleOrDefault(uch => uch.ConnectionId == connectionId);
            return chatClient;
        }

        public ChatUser GetChatUserByAgentId(string agentId)
        {
            var ChatUser = iSDbContext.ChatUsers.Include(c => c.ConnectedClients).Single(uch => uch.AgentId == agentId);
            return ChatUser;
        }

        private ChatRoom GetChatRoomByAgentIdOrAvailable(string agentId)
        {
            var room = iSDbContext.ChatRooms.Include(r => r.ChatMessages).FirstOrDefault(uch => !uch.IsClosed && (uch.AgentId == agentId || uch.AgentId == null));
            return room;
        }

        private ChatRoom GetChatRoomByUserId(string userId)
        {
            var room = iSDbContext.ChatRooms.FirstOrDefault(uch => !uch.IsClosed && (uch.UserId == userId || uch.UserId == null));
            return room;
        }

        //private static CloudQueue GetCloudQueue()
        //{
        //    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
        //            CloudConfigurationManager.GetSetting("vbuh_AzureStorageConnectionString"));

        //    var queueblobClient = storageAccount.CreateCloudQueueClient();
        //    var queue = queueblobClient.GetQueueReference("fb-queue");
        //    return queue;
        //}
    }
}