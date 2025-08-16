using System;

namespace Ugen.Graph
{
    public interface IUgenInput
    {
        Type ValueType { get; }
    }

    public interface IUgenInput<in T> : IUgenInput
    {
        void Send(T value);
    }
}
