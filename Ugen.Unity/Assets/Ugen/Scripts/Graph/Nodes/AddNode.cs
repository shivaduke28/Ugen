using System;
using R3;

namespace Ugen.Graph.Nodes
{
    [Serializable]
    [UgenNode]
    public sealed partial class AddNode : UgenNode, IInitializable, IDisposable
    {
        public override string NodeName => "Add";

        readonly UgenInput<float> a = new("a", 0, 0f);
        readonly UgenInput<float> b = new("b", 1, 0f);
        readonly Subject<float> result = new();
        readonly UgenOutput<float> output;
        IDisposable disposable;

        public AddNode()
        {
            output = new UgenOutput<float>("result", 0, result);
            AddInputPort(a);
            AddInputPort(b);
            AddOutputPort(output);
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
