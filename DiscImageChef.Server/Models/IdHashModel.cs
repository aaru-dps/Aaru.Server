namespace DiscImageChef.Server.Models
{
    public class IdHashModel
    {
        public IdHashModel(int id, string hash)
        {
            Id   = id;
            Hash = hash;
        }

        public int    Id          { get; set; }
        public string Hash        { get; set; }
        public string Description { get; set; }
        public int[]  Duplicates  { get; set; }
    }
}