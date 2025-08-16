using System;
using R3;
using Ugen.Attributes;

namespace Ugen.Graph.Nodes
{
    [UgenNode]
    public sealed class AddNode : UgenNode, IInitializable, IDisposable
    {
        [UgenInput(0)] readonly UgenInputProperty<float> _a = new("a");
        [UgenInput(1)] readonly UgenInputProperty<float> _b = new("b");
        [UgenOutput(0, "result")] readonly UgenOutputSubject<float> _result = new("result");
        IDisposable _disposable;

        public AddNode(string nodeId) : base(nodeId)
        {
            AddInputPort(_a);
            AddInputPort(_b);
            AddOutputPort(_result);
        }

        void IInitializable.Initialize()
        {
            _disposable = _a.Observable.CombineLatest(_b.Observable, (x, y) => x + y)
                .Subscribe(x => _result.OnNext(x));
        }

        void IDisposable.Dispose()
        {
            _disposable?.Dispose();
        }
    }
}
