using System.Collections.Generic;

namespace Aaru.Server.Models
{
    public class ChsModel
    {
        public ushort Cylinders { get; set; }
        public ushort Heads     { get; set; }
        public ushort Sectors   { get; set; }
    }

    public class ChsModelForView
    {
        public List<ChsModel> List { get; set; }
        public string         Json { get; set; }
    }
}