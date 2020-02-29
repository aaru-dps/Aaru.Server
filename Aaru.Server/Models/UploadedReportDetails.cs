using System.Collections.Generic;
using Aaru.CommonTypes.Metadata;

namespace Aaru.Server.Models
{
    public class UploadedReportDetails
    {
        public UploadedReport              Report                 { get; set; }
        public List<int>                   SameAll                { get; set; }
        public List<int>                   SameButManufacturer    { get; set; }
        public List<int>                   ReportAll              { get; set; }
        public List<int>                   ReportButManufacturer  { get; set; }
        public int                         ReadCapabilitiesId     { get; set; }
        public List<TestedMedia>           TestedMedias           { get; set; }
        public List<TestedSequentialMedia> TestedSequentialMedias { get; set; }
    }
}