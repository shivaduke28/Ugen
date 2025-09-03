using Ugen.Graphs.Ports;

namespace Ugen.Graphs.Edges
{
    public interface IEdgeFactory
    {
        IEdge Create(Port outputPort, Port inputPort);
    }
}
