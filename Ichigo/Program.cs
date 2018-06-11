using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Ichigo
{
    class Program
    {
        static int Main(string[] args)
        {
            AsyncSocketListener.StartListening();
            return 0;
        }
    }

    public class State
    {
        public const int BufferSize = 1024;
        public Socket WorkerSocket = null;
        public byte[] Buffer = new byte[State.BufferSize];
        public StringBuilder Builder = new StringBuilder();
    }

    public class AsyncSocketListener
    {
        public static ManualResetEventSlim _allDone = new ManualResetEventSlim(false);

        public static void StartListening()
        {
            var hostInfo = Dns.GetHostEntry(Dns.GetHostName());
            var ipAddress = hostInfo.AddressList[1];
            var endpoint = new IPEndPoint(ipAddress, 21113);

            var listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

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

        public static void AcceptCallback(IAsyncResult asyncResult)
        {
            _allDone.Set();

            var listener = (Socket) asyncResult.AsyncState;
            var handler = listener.EndAccept(asyncResult);

            var state = new State { WorkerSocket = handler };
            handler.BeginReceive(state.Buffer, 0, State.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }

        public static void ReadCallback(IAsyncResult asyncResult)
        {
            var state = (State) asyncResult.AsyncState;
            var handler = state.WorkerSocket;

            var bytesRead = handler.EndReceive(asyncResult);
            if (bytesRead <= 0) return;
            state.Builder.Append(Encoding.UTF8.GetString(state.Buffer, 0, bytesRead));
            var content = state.Builder.ToString();

            if (content.IndexOf("<EOF>", StringComparison.Ordinal) > -1)
            {
                Console.WriteLine($"Read {content.Length}bytes from socket.\n Data: {content}");
                Send(handler, content);
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
    }
}
