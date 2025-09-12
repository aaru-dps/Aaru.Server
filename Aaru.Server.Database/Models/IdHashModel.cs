using Aaru.CommonTypes.Structs.Devices.SCSI;
using Aaru.Helpers;

namespace Aaru.Server.Database.Models;

public class IdHashModel : BaseModel<int>
{
    string?       _description;
    public string Hash { get; set; }
    public string? Description
    {
        get => _description ?? string.Join(' ', VendorIdentification, ProductIdentification, ProductRevisionLevel);
        set => _description = value;
    }
    public int[]    Duplicates            { get; set; }
    public string   VendorIdentification  => StringHandlers.CToString(Inquiry?.VendorIdentification);
    public string   ProductIdentification => StringHandlers.CToString(Inquiry?.ProductIdentification);
    public string   ProductRevisionLevel  => StringHandlers.CToString(Inquiry?.ProductRevisionLevel);
    public Inquiry? Inquiry               { get; set; }
}