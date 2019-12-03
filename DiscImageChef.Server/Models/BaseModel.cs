using System.ComponentModel.DataAnnotations;

namespace DiscImageChef.Server.Models
{
    public class BaseModel<T>
    {
        [Key]
        public T Id { get; set; }
    }
}