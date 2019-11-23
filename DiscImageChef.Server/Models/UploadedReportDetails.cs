using System.Collections.Generic;

namespace DiscImageChef.Server.Models
{
    public class UploadedReportDetails
    {
        public UploadedReport Report                { get; set; }
        public List<int>      SameAll               { get; set; }
        public List<int>      SameButManufacturer   { get; set; }
        public List<int>      ReportAll             { get; set; }
        public List<int>      ReportButManufacturer { get; set; }
    }
}