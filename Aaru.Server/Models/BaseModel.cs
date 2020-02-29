using System.ComponentModel.DataAnnotations;

namespace DiscImageChef.Server.Models
{
    public abstract class BaseModel<T>
    {
        [Key]
        public T Id { get; set; }
    }
}