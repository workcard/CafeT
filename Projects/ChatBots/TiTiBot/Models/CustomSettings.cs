using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QNABot.Models
{
    // This allows us to retrieve custom settings 
    // from appsettings.json
    public class CustomSettings
    {
        public string OcpApimSubscriptionKey { get; set; }
        public string KnowledgeBase { get; set; }
    }
}
