using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegrationSamples.Models
{
    public class ChatRoom
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string AgentId { get; set; }
        public string ConnectionId { get; set; }
             
        public bool IsClosed { get; set; }
        public DateTime OpenDate { get; set; }

        public DateTime? LastAgentReply { get; set; }
        public DateTime? LastUserReply { get; set; }

        public DateTime? CloseDate { get; set; }

        public ICollection<ChatMessage> ChatMessages { get; set; }
        public IEnumerable<ChatMessage> GetUnreadUserMessages()
        {
            var dtFrom = LastAgentReply ?? DateTime.MinValue;
            return ChatMessages.Where(m => m.AddedOn >= dtFrom && m.IsFromPlatform);
        }
    }
}