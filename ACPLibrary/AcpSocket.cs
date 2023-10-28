using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json.Nodes;
using ACPLibrary.Core;
using ACPLibrary.Utils;

namespace ACPLibrary;

public class AcpSocket
{
    private Socket _socketV4;
    private Socket _socketV6;
    
    private int acpLocalPort;
    private int acpRemotePort;
    private IPEndPoint remote;

    private void AcpSocketInit()
    {
        this._socketV4 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        this._socketV4.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        this._socketV4.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
        this._socketV4.Bind(new IPEndPoint(IPAddress.Loopback, AcpCoreService.LocalTargetUdpPort ));
        
        this._socketV6 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        this._socketV6.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        this._socketV6.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
        this._socketV6.Bind(new IPEndPoint(IPAddress.IPv6Loopback, AcpCoreService.LocalTargetUdpPort ));
    }

    public AcpSocket(int localPort, int remotePort, string ipAddress)
    {
        AcpSocketInit();
        this.acpLocalPort = localPort;
        this.acpRemotePort = remotePort;
        this.remote = new IPEndPoint(
            IPAddress.Parse(ipAddress),
            AcpCoreService.NetworkUdpPort
        );
        


    }



    public AcpData Recv(bool ipv6 = false)
    {
        Socket socket = _socketV4;
        if (ipv6)
        {
            socket = _socketV6;
        }
        else
        {
            socket = _socketV4;
        }

        while (true)
        {
            EndPoint point = new IPEndPoint(IPAddress.Any, 0);
            IPEndPoint ipEndPoint = (IPEndPoint)point;
            byte[] data = new byte[65535];
            int length = socket.ReceiveFrom(data, ref point);
            if (length > 0)
            {
                string message = Encoding.UTF8.GetString(data, 0, length);
                JsonNode jsonObject = JsonObject.Parse(message);
                
                if (jsonObject["to"] != null)
                {
                    if (jsonObject["from"] != null)
                    {
                        if (jsonObject["time"] != null)
                        {
                            if (jsonObject["option"] != null)
                            {
                                if (jsonObject["data"] != null)
                                {
                                    return new AcpData(jsonObject["option"].AsObject(), Convert.FromBase64String(jsonObject["data"].ToString()));                                    
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void Send(AcpData data, bool ipv6 = false)
    {
        Socket socket = _socketV4;
        if (ipv6)
        {
            socket = _socketV6;
        }
        else
        {
        }
    }

    public void Send(byte[] data, JsonObject option = null, bool ipv6 = false)
    {
        JsonObject root = new JsonObject();
        root["to"] = acpRemotePort;
        root["from"] = acpLocalPort;
        root["time"] = TimeUtils.GetTimeStamp();
        if (option == null)
        {
            root["option"] = new JsonObject();
        }
        root["data"] = Convert.ToBase64String(data);
        if (ipv6)
        {
            this._socketV6.SendTo(
                Encoding.UTF8.GetBytes(root.ToString()),
                this.remote
                );
        }
        else
        {
            this._socketV4.SendTo(
                Encoding.UTF8.GetBytes(root.ToString()),
                this.remote
            );
        }

    }
    
    
}