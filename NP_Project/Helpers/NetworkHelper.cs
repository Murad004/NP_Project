using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NP_Project.Helpers
{
    public class NetworkHelper
    {
        public TcpListener TcpListener;
        IPEndPoint endPoint;
        BinaryReader BinaryReader;

        public void Start()
        {
            endPoint = new IPEndPoint(IPAddress.Parse(EndPointHelper.IP), EndPointHelper.PORT);
            TcpListener = new TcpListener(endPoint);
            TcpListener.Start();

            while (true)
            {
                var client = TcpListener.AcceptTcpClient();
                Task.Run(() =>
                {
                    var stream = client.GetStream();
                    BinaryReader.ReadString();
                });
            }
        }
    }
}
