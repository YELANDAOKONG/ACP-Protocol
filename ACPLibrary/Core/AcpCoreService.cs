using System.Buffers.Text;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json.Nodes;
using ACPLibrary.Utils;

namespace ACPLibrary.Core;

public class AcpCoreService
{
    // public Socket NetworkTcpSocket;
    // public Socket NetworkTcpSocketV6;
    public Socket NetworkUdpSocket;
    public Socket NetworkUdpSocketV6;
    public Socket LocalUdpSocket;
    public Socket LocalUdpSocketV6;

    public static readonly int NetworkUdpPort = 743;
    public static readonly int LocalUdpPort = 1743;
    public static readonly int LocalTargetUdpPort = 2743;

    private NetworkDataHandler NetworkHandler;
    private LocalDataHandler LocalHandler;

    public AcpCoreService()
    {
        // this.NetworkTcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        // this.NetworkTcpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        //// this.NetworkTcpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
        
        // this.NetworkTcpSocketV6 = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
        // this.NetworkTcpSocketV6.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        //// this.NetworkTcpSocketV6.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
        
        this.NetworkUdpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        this.NetworkUdpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        this.NetworkUdpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
        
        this.LocalUdpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        this.LocalUdpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        this.LocalUdpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
        
        this.NetworkUdpSocketV6 = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
        this.NetworkUdpSocketV6.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        this.NetworkUdpSocketV6.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
        
        this.LocalUdpSocketV6 = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
        this.LocalUdpSocketV6.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        this.LocalUdpSocketV6.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
        
        // this.NetworkTcpSocket.Bind(new IPEndPoint(IPAddress.Any, this.NetworkUdpPort));
        // this.NetworkTcpSocketV6.Bind(new IPEndPoint(IPAddress.IPv6Any, this.NetworkUdpPort));
        
        this.NetworkUdpSocket.Bind(new IPEndPoint(IPAddress.Any, NetworkUdpPort));
        this.NetworkUdpSocketV6.Bind(new IPEndPoint(IPAddress.IPv6Any, NetworkUdpPort));
        
        this.LocalUdpSocket.Bind(new IPEndPoint(IPAddress.Loopback, LocalUdpPort));
        this.LocalUdpSocketV6.Bind(new IPEndPoint(IPAddress.IPv6Loopback, LocalUdpPort));

        this.NetworkHandler = new NetworkDataHandler(this);
        this.LocalHandler = new LocalDataHandler(this);

        this.NetworkHandler.Register();
        this.LocalHandler.Register();
    }



    public void LocalSendPackage(byte[] data, bool ipv6 = false)
    {
        if (ipv6)
        {
            this.LocalUdpSocketV6.SendTo(data, new IPEndPoint(IPAddress.IPv6Loopback, LocalTargetUdpPort));
        }
        else
        {
            this.LocalUdpSocket.SendTo(data, new IPEndPoint(IPAddress.Loopback, LocalTargetUdpPort));
        }
    }



    public void NetworkSendPackage(byte[] data, IPEndPoint endPoint, int localAcpPort, int targetAcpPort, JsonObject option = null, bool ipv6 = false)
    {
        
        string message = Encoding.UTF8.GetString(data, 0, data.Length);
        JsonNode jsonObject = new JsonObject();
        jsonObject["to"] = targetAcpPort;
        jsonObject["from"] = localAcpPort;
        jsonObject["time"] = TimeUtils.GetTimeStamp();
        if (option == null)
        {
            jsonObject["option"] = new JsonObject();
        }
        jsonObject["data"] = Convert.ToBase64String(data);
        
        if (ipv6)
        {
            this.NetworkUdpSocketV6.SendTo(data, endPoint);
        }
        else
        {
            this.NetworkUdpSocket.SendTo(data, endPoint);
        }
    }




}