﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ARDrone.Control.Network
{
    public abstract class UdpWorker : KeepAliveNetworkWorker
    {
        protected UdpClient client;

        public UdpWorker(String remoteIpAddress, int port, int timeoutValue)
            : base(remoteIpAddress, port, timeoutValue)
        { }

        protected override void CreateSocket()
        {
            client = CreateUdpSocket(LocalIpAddress, Port, TimeoutValue);
        }

        public override void DisconnectFromSocket()
        {
            client.Close();
            client = null;
        }

        protected UdpClient CreateUdpSocket(string ip, int port, int timeoutValue)
        {
            UdpClient client = null;

            try
            {
                IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(ip), port);
                client = new UdpClient(endpoint);

                client.Client.SendTimeout = timeoutValue;
                client.Client.ReceiveTimeout = timeoutValue;
            }
            catch { throw; }

            return client;
        }

        public override void SendMessage(int message)
        {
            SendMessage(BitConverter.GetBytes(message));
        }

        public override void SendMessage(String message)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            SendMessage(buffer);
        }

        public override void SendMessage(byte[] message)
        {
            client.Send(message, message.Length, endpoint);
        }
    }
}