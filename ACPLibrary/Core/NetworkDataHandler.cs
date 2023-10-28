using System.Net;
using System.Net.Sockets;

namespace ACPLibrary.Core;

public class NetworkDataHandler
{
    private AcpCoreService CoreService;

    public NetworkDataHandler(AcpCoreService coreService)
    {
        this.CoreService = CoreService;
    }
    
    private void UdpHandlerThread(Socket socket)
    {
        while (true)
        {
            EndPoint point = new IPEndPoint(IPAddress.Any, 0);
            byte[] buffer = new byte[65535];
            int length = socket.ReceiveFrom(buffer, ref point);
            // string message = Encoding.UTF8.GetString(buffer,0,length);

        }
    }

    
    private void TcpHandlerThread(Socket socket)
    {
        while (true)
        {
            
        }
    }
    
    public void Register()
    {
        
    }
}