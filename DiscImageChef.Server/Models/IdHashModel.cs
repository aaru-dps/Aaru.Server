namespace DiscImageChef.Server.Models
{
    public class IdHashModel : BaseModel<int>
    {
        public IdHashModel(int id, string hash)
        {
            Id   = id;
            Hash = hash;
        }

        public string Hash        { get; set; }
        public string Description { get; set; }
        public int[]  Duplicates  { get; set; }
    }
}