namespace DistributedSnapshot.Models
{
    public class ChannelMessage
    {
        public int FromId { get; set; }
        public int ToId { get; set; }
        public string Content { get; set; } = "";
    }
}
