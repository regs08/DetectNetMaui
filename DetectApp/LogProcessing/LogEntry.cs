public class LogEntry
{
    public string Event { get; set; }
    public string Label { get; set; }
    public double Confidence { get; set; }
    public int Xmin { get; set; }
    public int Ymin { get; set; }
    public int Xmax { get; set; }
    public int Ymax { get; set; }
    public string Filename { get; set; }
    public string Timestamp { get; set; }
    public string Details { get; set; }
}