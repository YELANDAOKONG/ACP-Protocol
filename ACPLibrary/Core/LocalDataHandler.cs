using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json.Nodes;

namespace ACPLibrary.Core;

public class LocalDataHandler
{
    private AcpCoreService CoreService;

    public LocalDataHandler(AcpCoreService coreService)
    {
        this.CoreService = coreService;
    }


    private void UdpHandlerThread(Socket socket)
    {
        while (true)
        {
            try
            {
                EndPoint point = new IPEndPoint(IPAddress.Any, 0);
                byte[] buffer = new byte[65535];
                int length = socket.ReceiveFrom(buffer, ref point);
                if (((IPEndPoint)point).Port == AcpCoreService.LocalTargetUdpPort)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, length);
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
                                        // Send this package to Network
                                        CoreService.NetworkSendPackage(
                                                Convert.FromBase64String(jsonObject["data"].ToString()),
                                                new IPEndPoint(
                                                    IPAddress.Parse(jsonObject["ipaddr"].ToString()),
                                                    int.Parse(jsonObject["ipport"].ToString())
                                                ),
                                                int.Parse(jsonObject["from"].ToString()),
                                                int.Parse(jsonObject["to"].ToString()),
                                                option: jsonObject["option"].AsObject(),
                                                ipv6: (((IPEndPoint)point).Address.AddressFamily == AddressFamily.InterNetworkV6)
                                            );
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }

        }
    }
    

    public void Register()
    {
        Thread IPv4HandlerThread = new Thread(() =>
        {
            UdpHandlerThread(CoreService.LocalUdpSocket);
        });
        Thread IPv6HandlerThread = new Thread(() =>
        {
            UdpHandlerThread(CoreService.LocalUdpSocketV6);
        });
        IPv4HandlerThread.Start();
        //IPv4HandlerThread.Join();
        IPv6HandlerThread.Start();
        //IPv6HandlerThread.Join();

    }
    
    
}