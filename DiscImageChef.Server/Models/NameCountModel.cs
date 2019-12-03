namespace DiscImageChef.Server.Models
{
    public class NameCountModel<T> : BaseModel<T>
    {
        public string Name  { get; set; }
        public long   Count { get; set; }
    }
}