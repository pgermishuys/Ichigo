using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Ichigo
{
    public class Server
    {
        private static ManualResetEventSlim _allDone = new ManualResetEventSlim(false);

        public static void StartListening(IPEndPoint endpoint)
        {
            var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Console.WriteLine($"Listening on {endpoint}");

            try
            {
                listener.Bind(endpoint);
                listener.Listen(100);

                while (true)
                {
                    _allDone.Reset();

                    Console.WriteLine("Waiting for a connection...");

                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);

                    _allDone.Wait();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }

        private static void AcceptCallback(IAsyncResult asyncResult)
        {
            _allDone.Set();

            var listener = (Socket) asyncResult.AsyncState;
            var handler = listener.EndAccept(asyncResult);

            var state = new State { WorkerSocket = handler };
            handler.BeginReceive(state.Buffer, 0, State.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }

        private static void ReadCallback(IAsyncResult asyncResult)
        {
            var state = (State) asyncResult.AsyncState;
            var handler = state.WorkerSocket;

            var bytesRead = handler.EndReceive(asyncResult);
            if (bytesRead <= 0) return;
            state.Builder.Append(Encoding.UTF8.GetString(state.Buffer, 0, bytesRead));
            var content = state.Builder.ToString();

            if (content.IndexOf("\r\n\r\n", StringComparison.Ordinal) > -1)
            {
                var requestHandler = new HttpRequestHandler();
                var response = requestHandler.HandleRequest(HttpRequest.Parse(content)).GetAwaiter().GetResult();
                Send(handler, response.ToString());
            }
            else
            {
                handler.BeginReceive(state.Buffer, 0, State.BufferSize, 0, new AsyncCallback(ReadCallback), state);
            }
        }

        private static void Send(Socket handler, string data)
        {
            var byteData = Encoding.UTF8.GetBytes(data);

            handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult asyncResult)
        {
            try
            {
                var handler = (Socket) asyncResult.AsyncState;
                var bytesSent = handler.EndSend(asyncResult);
                Console.WriteLine($"Sent {bytesSent} to client.");

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        internal class State
        {
            public const int BufferSize = 1024;
            public Socket WorkerSocket = null;
            public byte[] Buffer = new byte[State.BufferSize];
            public StringBuilder Builder = new StringBuilder();
        }
    }
}
