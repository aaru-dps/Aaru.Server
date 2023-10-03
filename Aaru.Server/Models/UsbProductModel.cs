using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Aaru.Server.Models;

public class UsbProductModel
{
    [DisplayName("Manufacturer")]
    public string VendorName { get; set; }

    public int VendorId { get; set; }

    [DisplayName("Product")]
    public string ProductName { get; set; }

    [DisplayName("Product ID")]
    [DisplayFormat(DataFormatString = "0x{0:X4}")]
    public ushort ProductId { get; set; }
}