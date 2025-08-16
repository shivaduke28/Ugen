using System;
using R3;
using UnityEngine;

namespace Ugen.Graph
{
    [Serializable]
    public sealed class SerializableUgenInputSubject<T> : IUgenInput<T>
    {
        [SerializeField] T _value;
        public Type ValueType => typeof(T);
        readonly Subject<T> _subject = new();
        public Observable<T> Observable => _subject;

        public void Send(T value)
        {
            _value = value;
            _subject.OnNext(value);
        }

        public void ForceNotify()
        {
            _subject.OnNext(_value);
        }
    }
}
