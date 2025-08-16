using System;
using R3;

#nullable enable

namespace Ugen.Graph
{
    public class UgenOutputSubject<T> : IUgenOutput<T>
    {
        readonly Subject<T> _subject = new();
        public string Name { get; }
        public Type ValueType => typeof(T);

        public UgenOutputSubject(string name)
        {
            Name = name;
        }

        public void ConnectTo(IUgenInput input, CompositeDisposable disposables)
        {
            if (input is IUgenInput<T> typedInput)
            {
                Observable.Subscribe(v => typedInput.Send(v))
                    .AddTo(disposables);
            }
            else
            {
                throw new ArgumentException("Invalid input type");
            }
        }

        public void OnNext(T value)
        {
            _subject.OnNext(value);
        }

        public Observable<T> Observable => _subject;
    }

    public class UgenOutput<T> : IUgenOutput<T>
    {
        public string Name { get; }
        public Type ValueType => typeof(T);
        public Observable<T> Observable { get; }

        public UgenOutput(string name, Observable<T> observable)
        {
            Name = name;
            Observable = observable;
        }

        public void ConnectTo(IUgenInput input, CompositeDisposable disposables)
        {
            if (input is IUgenInput<T> typedInput)
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
