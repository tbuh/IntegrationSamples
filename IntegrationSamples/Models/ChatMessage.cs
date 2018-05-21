using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegrationSamples.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsFromPlatform { get; set; }
        public string Platform { get; set; }
        public DateTime AddedOn { get; set; }

        public int ChatRoomId { get; set; }
        public ChatRoom ChatRoom { get; set; }
    }
}