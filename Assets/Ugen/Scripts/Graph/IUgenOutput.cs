using System;
using R3;

namespace Ugen.Graph
{
    public interface IUgenOutput
    {
        string Name { get; }
        Type ValueType { get; }
        int Index { get; }
        void ConnectTo(IUgenInput input, CompositeDisposable disposables);
    }
}
