using System;
using R3;
using Ugen.Attributes;

namespace Ugen.Graph.Nodes
{
    [UgenNode]
    public sealed class AddNode : UgenNode, IInitializable, IDisposable
    {
        [UgenInput(0)] readonly UgenInput<float> a = new("a", 0, 0f);
        [UgenInput(1)] readonly UgenInput<float> b = new("b", 1, 0f);
        readonly Subject<float> result = new();
        [UgenOutput(0)] readonly UgenOutput<float> value;
        IDisposable disposable;

        public AddNode(string nodeId) : base(nodeId)
        {
            value = new UgenOutput<float>("result", 0, result);
            AddInputPort(a);
            AddInputPort(b);
            AddOutputPort(value);
        }

        public void Initialize()
        {
            disposable = a.Observable.CombineLatest(b.Observable, (x, y) => x + y)
                .Subscribe(x => result.OnNext(x));
        }

        public void Dispose()
        {
            disposable?.Dispose();
        }
    }
}