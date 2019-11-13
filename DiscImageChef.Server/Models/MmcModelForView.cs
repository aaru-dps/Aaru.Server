using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DiscImageChef.Server.Models
{
    public class MmcModelForView
    {
        public int Id { get; set; }
        [DisplayFormat(NullDisplayText = "none"), DisplayName("MMC FEATURES ID")]
        public int? FeaturesId { get; set; }
        [DisplayName("Response length (bytes)")]
        public int DataLength { get; set; }
    }
}