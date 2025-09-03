using Ugen.Graphs.Ports;

namespace Ugen.Graphs.Edges
{
    public sealed class DefaultEdgeFactory : IEdgeFactory
    {
        public IEdge Create(Port outputPort, Port inputPort)
        {
            return new DefaultEdge(outputPort, inputPort);
        }
    }

    public sealed class DefaultEdge : IEdge
    {
        public DefaultEdge(Port outputPort, Port inputPort)
        {
        }

        public void Dispose()
        {
        }
    }
}
