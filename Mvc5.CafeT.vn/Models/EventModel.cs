
using System.Collections.Generic;

namespace Mvc5.CafeT.vn.Models
{
    public class EventModel:EventObject
    {
        public IEnumerable<FileModel> Files { set; get; }
        public IEnumerable<ImageModel> Images { set; get; }

        public EventModel():base()
        {
        }
    }
}