using System.ComponentModel.DataAnnotations;

namespace Aaru.Server.Database.Models;

public abstract class BaseModel<T>
{
    [Key]
    public T Id { get; set; }
}