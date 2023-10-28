using System.Text.Json.Nodes;

namespace ACPLibrary;

public class AcpData
{
    public JsonObject Option;
    public byte[] Data;

    public AcpData()
    {
        
    }
    
    public AcpData(JsonObject option, byte[] data)
    {
        this.Option = option;
        this.Data = data;
    }
}