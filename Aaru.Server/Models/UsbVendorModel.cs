using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Aaru.Server.Models
{
    public class UsbVendorModel
    {
        [DisplayName("Manufacturer")]
        public string Vendor { get; set; }
        [DisplayName("Vendor ID"), DisplayFormat(DataFormatString = "0x{0:X4}")]
        public ushort VendorId { get;                set; }
        public List<UsbProductModel> Products { get; set; }
    }
}