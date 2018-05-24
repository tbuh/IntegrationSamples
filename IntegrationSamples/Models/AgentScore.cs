using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntegrationSamples.Models
{
    public class AgentScore
    {
        public int Id { get; set; }
        public string AgentId { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
    }
}