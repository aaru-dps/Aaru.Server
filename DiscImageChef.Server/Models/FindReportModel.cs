using System.Collections.Generic;

namespace DiscImageChef.Server.Models
{
    public class FindReportModel
    {
        public int          Id           { get; set; }
        public string       Manufacturer { get; set; }
        public string       Model        { get; set; }
        public string       Revision     { get; set; }
        public string       Bus          { get; set; }
        public List<Device> LikeDevices  { get; set; }
    }
}