namespace DistributedSnapshot.Models
{
    public class ProcessNode
    {
        public int Id { get; set; }
        public int LocalState { get; set; }
        public bool HasRecorded { get; set; }
        public bool IsAlive { get; set; } = true;
        public bool IsRecovering { get; set; } = false;

        // For snapshot: marker received per channel neighbor
        public HashSet<int> MarkersReceivedFrom { get; set; } = new();

        // Buffer messages arriving before marker per neighbor
        public Dictionary<int, List<ChannelMessage>> ChannelBuffers { get; set; }
            = new();

        public ProcessNode(int id)
        {
            Id = id;
            LocalState = new Random().Next(0, 100);
        }
    }
}
