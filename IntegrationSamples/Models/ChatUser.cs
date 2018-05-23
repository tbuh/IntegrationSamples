using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntegrationSamples.Models
{
    public class ChatUser
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }             
    }

    public class ChatClient
    {        
        public int Id { get; set; }             
        public ChatUser ChatUser { get; set; }
        public string ConnectionId { get; set; }
    }
}