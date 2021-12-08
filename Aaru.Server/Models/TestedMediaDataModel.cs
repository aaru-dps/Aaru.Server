namespace Aaru.Server.Models;

public class TestedMediaDataModel
{
    public int    TestedMediaId { get; set; }
    public string DataName      { get; set; }
    public string RawDataAsHex  { get; set; }
    public string Decoded       { get; set; }
}