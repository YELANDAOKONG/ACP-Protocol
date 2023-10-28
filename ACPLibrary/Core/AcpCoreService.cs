using System.Net;
using System.Net.Sockets;

namespace ACPLibrary.Core;

public class AcpCoreService
{
    public Socket NetworkTcpSocket;
    public Socket NetworkTcpSocketV6;
    public Socket NetworkUdpSocket;
    public Socket NetworkUdpSocketV6;
    public Socket LocalUdpSocket;
    public Socket LocalUdpSocketV6;

    public readonly int NetworkUdpPort = 743;
    public readonly int LocalUdpPort = 1743;

    private NetworkDataHandler NetworkHandler;
    private LocalDataHandler LocalHandler;

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

        this.NetworkHandler = new NetworkDataHandler(this);
        this.LocalHandler = new LocalDataHandler(this);

        this.NetworkHandler.Register();
        this.LocalHandler.Register();
    }
    
    
    
}