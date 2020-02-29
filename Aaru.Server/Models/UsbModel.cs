using System.Collections.Generic;

namespace DiscImageChef.Server.Models
{
    public class UsbModel
    {
        public string Manufacturer { get; set; }
        public string Product      { get; set; }
        public ushort VendorID     { get; set; }
        public ushort ProductID    { get; set; }
    }

    public class UsbModelForView
    {
        public List<UsbModel> List { get; set; }
        public string         Json { get; set; }
    }
}