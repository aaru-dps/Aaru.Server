using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Aaru.Server.Models;

public class FireWireModel
{
    [DisplayName("Vendor ID"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "0x{0:X8}")]
    public uint VendorID { get; set; }
    [DisplayName("Product ID"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "0x{0:X8}")]
    public uint ProductID { get; set; }
    [DisplayFormat(NullDisplayText = "Unknown")]
    public string Manufacturer { get; set; }
    [DisplayFormat(NullDisplayText = "Unknown")]
    public string Product { get; set; }
    [DisplayName("Is media removable?")]
    public bool RemovableMedia { get; set; }
}

public class FireWireModelForView
{
    public List<FireWireModel> List { get; set; }
    public string              Json { get; set; }
}