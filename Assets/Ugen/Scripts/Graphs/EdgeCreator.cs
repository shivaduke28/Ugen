namespace Ugen.Graphs
{
    public sealed class EdgeCreator
    {
        readonly GraphViewModel _graph;

        public EdgeCreator(GraphViewModel graph)
        {
            _graph = graph;
        }

        public bool TryCreateEdge(NodeId outputNodeId, int outputPortIndex, NodeId inputNodeId, int inputPortIndex)
        {
            return _graph.TryCreateEdge(outputNodeId, outputPortIndex, inputNodeId, inputPortIndex);
        }
    }
}
