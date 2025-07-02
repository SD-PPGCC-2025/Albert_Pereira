using DistributedSnapshot.Models;

namespace DistributedSnapshot.Services
{
    /// <summary>
    /// Simulates Chandy–Lamport snapshot on an in-memory fully connected graph of N nodes.
    /// </summary>
    public class SnapshotService
    {
        private readonly Dictionary<int, ProcessNode> _nodes;
        private readonly List<ChannelMessage> _globalChannel;

        public SnapshotService()    
        {
            // build 3 nodes fully connected
            _nodes = Enumerable.Range(1, 3)
                .Select(id => new ProcessNode(id)).ToDictionary(p => p.Id);

            // seed some in-flight messages:
            _globalChannel = new List<ChannelMessage>
            {
                new() { FromId = 2, ToId = 1, Content = "MENSAGEM NODE 2 PARA 1" },
                new() { FromId = 3, ToId = 1, Content = "MENSAGEM NODE 3 PARA 1" },
                new() { FromId = 1, ToId = 2, Content = "MENSAGEM NODE 1 PARA 2" },
                new() { FromId = 3, ToId = 2, Content = "MENSAGEM NODE 3 PARA 2" },
                new() { FromId = 1, ToId = 3, Content = "MENSAGEM NODE 1 PARA 3" },
                new() { FromId = 2, ToId = 3, Content = "MENSAGEM NODE 2 PARA 3" },
                // etc...
            };

            // init each node's buffers (will record per neighbor)
            foreach (var node in _nodes.Values)     
                node.ChannelBuffers = _nodes.Values
                    .Where(n => n.Id != node.Id)
                    .ToDictionary(n => n.Id, n => new List<ChannelMessage>());
        }

        public SnapshotResult StartSnapshot(int initiatorId)
        {
            // reset
            foreach (var node in _nodes.Values)
            {
                if (!node.IsAlive) continue; // ignora nós falhos
                node.HasRecorded = false;
                node.MarkersReceivedFrom.Clear();
                foreach (var buf in node.ChannelBuffers.Values)
                    buf.Clear();
            }

            if (!_nodes.TryGetValue(initiatorId, out var initiator) || !initiator.IsAlive)
                throw new InvalidOperationException("Initiator is not alive.");

            // 1) initiator records its own state
            RecordLocalState(initiatorId);

            // 3) scatter all in-flight messages into b uffers before marker
            foreach (var msg in _globalChannel)
            {
                if (!_nodes[msg.ToId].IsAlive) continue; // ignora destino falho
                _nodes[msg.ToId].ChannelBuffers[msg.FromId].Add(msg);
            }

            // 2) initiator sends marker on all outgoing channels
            foreach (var peer in _nodes.Keys.Where(id => id != initiatorId && _nodes[id].IsAlive))
                ReceiveMarker(peer, fromId: initiatorId);

            // compile snapshot result
            var result = new SnapshotResult();
            foreach (var n in _nodes.Values.Where(n => n.IsAlive))
                result.RecordedStates[n.Id] = n.LocalState;

            foreach (var n in _nodes.Values.Where(n => n.IsAlive))
            {
                foreach (var kv in n.ChannelBuffers)
                {
                    var key = $"{kv.Key}→{n.Id}";
                    result.InTransit[key] = kv.Value.ToList();
                }
            }

            return result;
        }

        private void RecordLocalState(int nodeId)
        {
            var node = _nodes[nodeId];
            if (node.HasRecorded) return;
            node.HasRecorded = true;

            // send marker to all other nodes
            foreach (var peer in 
                _nodes.Keys.Where(id => id != nodeId))
                ReceiveMarker(peer, fromId: nodeId);
        }

        private void ReceiveMarker(int nodeId, int fromId)
        {
            var node = _nodes[nodeId];

            // first marker from this channel: record state & start buffering others
            if (!node.MarkersReceivedFrom.Contains(fromId))
            {
                node.MarkersReceivedFrom.Add(fromId);

                // record local state if not done
                if (!node.HasRecorded)
                    RecordLocalState(nodeId);
            }
        }

        public void FailNode(int nodeId)
        {
            if (_nodes.TryGetValue(nodeId, out var node))
            {
                node.IsAlive = false;
                node.IsRecovering = false;
            }
        }

        public void RecoverNode(int nodeId)
        {
            if (_nodes.TryGetValue(nodeId, out var node))
            {
                node.IsAlive = true;
                node.IsRecovering = true;
                // Opcional: redefinir estado local ou buffers se necessário
            }
        }

        public IEnumerable<ProcessNode> GetAllNodes() => _nodes.Values;
    }
}
