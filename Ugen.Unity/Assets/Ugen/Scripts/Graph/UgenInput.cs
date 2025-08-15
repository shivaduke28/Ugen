using System;
using R3;

namespace Ugen.Graph
{
    public class UgenInput<T> : IUgenInput
    {
        public string Name { get; }
        public Type ValueType => typeof(T);
        public int Index { get; }
        ReactiveProperty<T> Value { get; }
        public Observable<T> Observable => Value;

        public void Send(T value)
        {
            Value.Value = value;
        }

        public UgenInput(string name, int index, T defaultValue = default)
        {
            Name = name;
            Index = index;
            Value = new ReactiveProperty<T>(defaultValue);
        }
    }
}
