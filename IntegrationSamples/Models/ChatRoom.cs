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
        public bool IsClosed { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }

        public ICollection<ChatMessage> ChatMessages { get; set; }
    }
}