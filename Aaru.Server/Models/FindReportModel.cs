namespace Aaru.Server.Models;

public class FindReportModel : BaseModel<int>
{
    public string       Manufacturer { get; set; }
    public string       Model        { get; set; }
    public string       Revision     { get; set; }
    public string       Bus          { get; set; }
    public List<Device> LikeDevices  { get; set; }
}