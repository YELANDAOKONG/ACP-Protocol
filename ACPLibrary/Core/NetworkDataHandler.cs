using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json.Nodes;

namespace ACPLibrary.Core;

public class NetworkDataHandler
{
    private AcpCoreService CoreService;

    public NetworkDataHandler(AcpCoreService coreService)
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
                                    jsonObject["ipaddr"] = ((IPEndPoint)point).Address.ToString();
                                    jsonObject["ipport"] = ((IPEndPoint)point).Port;
                                    CoreService.LocalSendPackage(
                                        Encoding.UTF8.GetBytes(jsonObject.ToString()),
                                        ipv6: (((IPEndPoint)point).Address.AddressFamily == AddressFamily.InterNetworkV6)
                                    
                                    );
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

    
    // private void TcpHandlerThread(Socket socket)
    // {
    //     try
    //     {
    //         
    //     }
    //     catch (Exception ex)
    //     {
    //             
    //     }
    // }
    
    public void Register()
    {
        Thread IPv4HandlerThread = new Thread(() =>
        {
            UdpHandlerThread(this.CoreService.NetworkUdpSocket);
        });
        Thread IPv6HandlerThread = new Thread(() =>
        {
            UdpHandlerThread(this.CoreService.NetworkUdpSocketV6);
        });
        IPv4HandlerThread.Start();
        //IPv4HandlerThread.Join();
        IPv6HandlerThread.Start();
        //IPv6HandlerThread.Join();
        
    }
}