using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Utilities.Comms
{
    public class ClientMessageTransmitter
    {
        private bool _connectionClosed = false;
        private bool _runReceiver = true;
        private Socket _initialSocket;
        private Socket _commsSocket;
        private TaskCompletionSource<bool> _waitForReady;
        IServerMessageReceivedHandler _handler;

        public Task WaitForReady => _waitForReady.Task;

        public bool Connected => !_connectionClosed && _waitForReady.Task.IsCompleted;

        public ClientMessageTransmitter(IServerMessageReceivedHandler handler)
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[1];
            Initialize(handler, ipAddress);
        }

        public ClientMessageTransmitter(IServerMessageReceivedHandler handler, IPAddress ipAddress)
        {
           Initialize(handler, ipAddress);
        }

        private void Initialize(IServerMessageReceivedHandler handler, IPAddress ipAddress)
        {
            _handler = handler;
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);
            _initialSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _waitForReady = new TaskCompletionSource<bool>();

            try
            {
                _initialSocket.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), _initialSocket);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Close()
        {
            _connectionClosed = true;
            _runReceiver = false;
            _commsSocket.Close();
            _initialSocket.Close();
        }

        public void TrySendMessage(Message message)
        {
            try
            {
                Message.SendMessage(_commsSocket, message);
            }
            catch
            {
                Close();
            }
        }

        private Message TryReceiveMessage(Socket socket)
        {
            try
            {
                return Message.ReceiveMessage(socket);
            }
            catch
            {
                Close();
                return null;
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            client.EndConnect(ar);
            _commsSocket = client;
            _waitForReady.SetResult(true);
            var ignoredTask = StartReceiverAsync(_commsSocket);
        }

        protected async Task StartReceiverAsync(Socket socket)
        {
            while (_runReceiver)
            {
                await Task.Delay(1);
                if (TryReceiveMessage(socket) is Message message && message != null)
                {
                    _handler.HandleServerMessage(message);
                }
            }
        }
    }
}
