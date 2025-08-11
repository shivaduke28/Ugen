using System;
using R3;

namespace Ugen.Graph
{
    public class UgenOutput<T> : IUgenOutput
    {
        public string Name { get; }
        public Type ValueType => typeof(T);
        readonly Observable<T> observable;

        public UgenOutput(string name, Observable<T> observable)
        {
            Name = name;
            this.observable = observable;
        }

        public void ConnectTo(IUgenInput input, CompositeDisposable disposables)
        {
            if (input is UgenInput<T> typedInput)
            {
                observable.Subscribe(v => typedInput.Send(v))
                    .AddTo(disposables);
            }
        }
    }
}