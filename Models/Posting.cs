namespace SimpleBoardAPI.Models
{
    public class Posting
    {
        public string id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public int view { get; set; }
        public long timestamp { get; set; }
    }
}
