using System.Collections.Generic;

namespace Aaru.Server.Models;

public class SscModel
{
    public byte? BlockSizeGranularity { get; set; }
    public uint? MaxBlockLength       { get; set; }
    public uint? MinBlockLength       { get; set; }
}

public class SscModelForView
{
    public List<SscModel> List { get; set; }
    public string         Json { get; set; }
}