using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Aaru.Server.Models
{
    public class MmcModelForView : BaseModel<int>
    {
        [DisplayFormat(NullDisplayText = "none"), DisplayName("MMC FEATURES ID")]
        public int? FeaturesId { get; set; }
        [DisplayName("Response length (bytes)")]
        public int DataLength { get; set; }
    }
}