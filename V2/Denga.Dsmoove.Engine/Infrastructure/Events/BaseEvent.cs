using System;

namespace Denga.Dsmoove.Engine.Infrastructure.Events
{
    public abstract class BaseEvent
    {
        public Guid MessageId { get; } = Guid.NewGuid();
        public DateTime CreatedAt { get; } = DateTime.Now;
    }
}