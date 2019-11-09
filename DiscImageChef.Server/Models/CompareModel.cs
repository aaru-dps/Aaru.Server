using System.Collections.Generic;

namespace DiscImageChef.Server.Models
{
    public class CompareModel
    {
        public int          LeftId       { get; set; }
        public int          RightId      { get; set; }
        public List<string> ValueNames   { get; set; }
        public List<string> LeftValues   { get; set; }
        public List<string> RightValues  { get; set; }
        public bool         HasError     { get; set; }
        public string       ErrorMessage { get; set; }
        public bool         AreEqual     { get; set; }
    }
}