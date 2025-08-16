using System;
using R3;

namespace Ugen.Graph
{
    public class UgenInputProperty<T> : IUgenInput<T>
    {
        public string Name { get; }
        public Type ValueType => typeof(T);
        ReactiveProperty<T> Value { get; }
        public Observable<T> Observable => Value;

        public void Send(T value)
        {
            Value.Value = value;
        }

        public UgenInputProperty(string name, T defaultValue = default)
        {
            Name = name;
            Value = new ReactiveProperty<T>(defaultValue);
        }
    }
}
