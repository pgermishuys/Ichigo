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
            Server.StartListening(new IPEndPoint(IPAddress.Loopback, 2113));
            return 0;
        }
    }

}
