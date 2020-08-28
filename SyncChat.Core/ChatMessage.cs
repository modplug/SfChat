using System;

namespace SyncChat.Core
{
    public class ChatMessage
    {
        public ChatMessage()
        {
            Id = Guid.NewGuid();
            Date = DateTime.Now;
            Message = Date.ToLongTimeString();
        }
        public string Message { get; }
        public DateTime Date { get; }
        public Guid Id { get; }
    }
}
