using CQRS.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Core.Events
{
    public class BaseEvent : Message
    {
        protected BaseEvent(string type)
        {
            Type = type;
        }
        public int Version { get; set; }
        public string Type { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
}
