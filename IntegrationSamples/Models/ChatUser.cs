using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegrationSamples.Models
{
    public class ChatUser
    {
        public int Id { get; set; }
        public string AgentId { get; set; }        
        public bool IsOnline { get; set; }
        public ICollection<ChatClient> ConnectedClients { get; set; }
    }

    public class ChatClient
    {        
        public int Id { get; set; }
        public ChatUser ChatUser { get; set; }
        public string ConnectionId { get; set; }
    }
}