using System;
using R3;

namespace Ugen.Graph
{
    public sealed class UgenInputSubject<T> : IUgenInput<T>
    {
        public Type ValueType => typeof(T);
        readonly Subject<T> _subject = new();
        public Observable<T> Observable => _subject;

        public void Send(T value)
        {
            _subject.OnNext(value);
        }
    }
}
