namespace Aaru.Server.Database.Models;

public abstract class NameCountModel<T> : BaseModel<T>
{
    public string Name  { get; set; }
    public long   Count { get; set; }
}