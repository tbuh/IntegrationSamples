using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegrationSamples.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public string Platform { get; set; }
        public DateTime? AddedOn { get; set; }
    }
}