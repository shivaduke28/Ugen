using System;

namespace Ugen.Graph
{
    public interface IUgenInput
    {
        string Name { get; }
        Type ValueType { get; }
    }
}
