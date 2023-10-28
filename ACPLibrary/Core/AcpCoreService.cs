using System.Net;
using System.Net.Sockets;

namespace ACPLibrary.Core;

public class AcpCoreService
{
    private Socket NetworkTcpSocket;
    private Socket NetworkTcpSocketV6;
    private Socket NetworkUdpSocket;
    private Socket NetworkUdpSocketV6;
    private Socket LocalUdpSocket;
    private Socket LocalUdpSocketV6;

    private readonly int NetworkUdpPort = 743;
    private readonly int LocalUdpPort = 1743;

    public AcpCoreService()
    {
        this.NetworkTcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        this.NetworkTcpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        // this.NetworkTcpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
        
        this.NetworkTcpSocketV6 = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
        this.NetworkTcpSocketV6.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        // this.NetworkTcpSocketV6.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
        
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
        
        this.NetworkTcpSocket.Bind(new IPEndPoint(IPAddress.Any, this.NetworkUdpPort));
        this.NetworkTcpSocketV6.Bind(new IPEndPoint(IPAddress.IPv6Any, this.NetworkUdpPort));
        
        this.NetworkUdpSocket.Bind(new IPEndPoint(IPAddress.Any, this.NetworkUdpPort));
        this.NetworkUdpSocketV6.Bind(new IPEndPoint(IPAddress.IPv6Any, this.NetworkUdpPort));
        
        this.LocalUdpSocket.Bind(new IPEndPoint(IPAddress.Loopback, this.LocalUdpPort));
        this.LocalUdpSocketV6.Bind(new IPEndPoint(IPAddress.IPv6Loopback, this.LocalUdpPort));
        
    }
}