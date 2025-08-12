using System;
using R3;

namespace Ugen.Graph
{
    public class UgenOutput<T> : IUgenOutput
    {
        public string Name { get; }
        public Type ValueType => typeof(T);
        public int Index { get; }
        public Observable<T> Observable { get; }

        public UgenOutput(string name, int index, Observable<T> observable)
        {
            Name = name;
            Index = index;
            Observable = observable;
        }

        public void ConnectTo(IUgenInput input, CompositeDisposable disposables)
        {
            if (input is UgenInput<T> typedInput)
            {
                Observable.Subscribe(v => typedInput.Send(v))
                    .AddTo(disposables);
            }
            else
            {
                throw new ArgumentException("Invalid input type");
            }
        }
    }
}
