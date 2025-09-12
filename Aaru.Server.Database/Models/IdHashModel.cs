using Aaru.CommonTypes.Structs.Devices.SCSI;
using Aaru.Helpers;

namespace Aaru.Server.Database.Models;

public class IdHashModel : BaseModel<int>
{
    public string   Hash { get; set; }
    public string   Description => string.Join(' ', VendorIdentification, ProductIdentification, ProductRevisionLevel);
    public int[]    Duplicates { get; set; }
    public string   VendorIdentification => StringHandlers.CToString(Inquiry?.VendorIdentification);
    public string   ProductIdentification => StringHandlers.CToString(Inquiry?.ProductIdentification);
    public string   ProductRevisionLevel => StringHandlers.CToString(Inquiry?.ProductRevisionLevel);
    public Inquiry? Inquiry { get; set; }
}