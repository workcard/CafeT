using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QNABot.Models
{
    public class QnAKnowledgeBase
    {
        public Add add { get; set; }
        public Delete delete { get; set; }
    }

    public class QnaPair
    {
        public string answer { get; set; }
        public string question { get; set; }
    }

    public class Add
    {
        public IList<QnaPair> qnaPairs { get; set; }
        public IList<string> urls { get; set; }
    }

    public class Delete
    {
        public IList<QnaPair> qnaPairs { get; set; }
        public IList<string> urls { get; set; }
    }
}
