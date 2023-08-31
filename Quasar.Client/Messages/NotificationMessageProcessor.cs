using Fahad.Common.Messages;

namespace Fahad.Client.Messages
{
    public abstract class NotificationMessageProcessor : MessageProcessorBase<string>
    {
        protected NotificationMessageProcessor() : base(true)
        {
        }
    }
}
