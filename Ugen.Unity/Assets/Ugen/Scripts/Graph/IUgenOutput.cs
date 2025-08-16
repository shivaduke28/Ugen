using System;
using R3;

namespace Ugen.Graph
{
    public interface IUgenOutput
    {
        string Name { get; }
        Type ValueType { get; }
        void ConnectTo(IUgenInput input, CompositeDisposable disposables);
    }

    public interface IUgenOutput<T> : IUgenOutput
    {
        Observable<T> Observable { get; }
    }
}
