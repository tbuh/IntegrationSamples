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
        private ISDbContext iSDbContext;
        public ChatService()
        {
            iSDbContext = new ISDbContext();
        }

        public ChatMessage AddFBMessage(string userId, string messageText)
        {
            var room = GetChatRoomAvailableForUser(userId);
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
            room.LastUserReply = chatMessage.AddedOn;

            iSDbContext.Entry(chatMessage).State = EntityState.Added;

            iSDbContext.SaveChanges();

            return chatMessage;
        }

        public ChatMessage AddMessage(string connectionId, string messageText)
        {
            var room = GetChatRoomAvailableByConnection(connectionId);
            if (room == null) return null;

            ChatMessage m = new ChatMessage
            {
                IsFromPlatform = false,
                Text = messageText,
                ChatRoom = room,
                AddedOn = DateTime.Now
            };
            room.LastAgentReply = m.AddedOn;
            iSDbContext.Entry(m).State = EntityState.Added;
            iSDbContext.SaveChanges();
            return m;
        }

        public void CloseChatRoom(string connectionId)
        {
            var room = GetChatRoomAvailableByConnection(connectionId);
            if (room == null) return;

            room.CloseDate = DateTime.Now;
            room.IsClosed = true;
            if (room.ChatMessages.Count > 0)
                iSDbContext.Entry(room).State = EntityState.Modified;
            else
                iSDbContext.Entry(room).State = EntityState.Deleted;
            iSDbContext.SaveChanges();
        }

        public ChatRoom Open(string agentId, string connectionId)
        {
            var user = iSDbContext.GetChatUser(agentId);
            var room = GetChatRoomAvailableForAgent(agentId);

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
            room.ConnectionId = connectionId;
            iSDbContext.SaveChanges();
            return room;
        }

        private ChatRoom GetChatRoomAvailableByConnection(string connectionId)
        {
            var room = iSDbContext.ChatRooms.Include(r => r.ChatMessages).FirstOrDefault(uch => !uch.IsClosed && uch.ConnectionId == connectionId);
            return room;
        }

        private ChatRoom GetChatRoomAvailableForAgent(string agentId)
        {
            var room = iSDbContext.ChatRooms.Include(r => r.ChatMessages).FirstOrDefault(uch => !uch.IsClosed && (uch.AgentId == null || uch.AgentId == agentId));
            return room;
        }

        private ChatRoom GetChatRoomAvailableForUser(string userId)
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