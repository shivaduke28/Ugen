using System;
using R3;
using UnityEngine;

namespace Ugen.Graph
{
    [Serializable]
    public class SerializableUgenInputProperty<T> : IUgenInput<T>
    {
        [SerializeField] SerializableReactiveProperty<T> _value;
        public Type ValueType => typeof(T);

        public Observable<T> Observable => _value;

        public void Send(T value)
        {
            _value.Value = value;
        }

        public SerializableUgenInputProperty(T defaultValue = default)
        {
            _value = new SerializableReactiveProperty<T>(defaultValue);
        }
    }
}
