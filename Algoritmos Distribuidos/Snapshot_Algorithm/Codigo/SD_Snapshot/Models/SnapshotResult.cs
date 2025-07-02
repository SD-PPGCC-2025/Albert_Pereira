namespace DistributedSnapshot.Models
{
    public class SnapshotResult
    {
        public Dictionary<int, int> RecordedStates { get; set; } = new();
        public Dictionary<string, List<ChannelMessage>> InTransit { get; set; }
            = new();
    }
}
