using System;
using System.Collections.Generic;
using System.Text;

namespace Denga.Dsmoove.Engine.Infrastructure.Events
{
    public class TrackerUpdatedEvent : BaseEvent
    {
        public int TorrentId { get; set; }
    }
}
