using System;
using R3;
using UnityEngine;

namespace Ugen.Behaviours
{
    public interface IInput
    {
        string Name { get; }
    }

    [Serializable]
    public abstract class Input<T> : IInput
    {
        [SerializeField] string name;
        public string Name => name;
        public abstract void Send(T value);
        public abstract Observable<T> Observable();

        protected Input(string name)
        {
            this.name = name;
        }
    }

    [Serializable]
    public class FloatInput : Input<float>
    {
        [SerializeField] SerializableReactiveProperty<float> value;

        public FloatInput(string name, float defaultValue) : base(name)
        {
            value = new SerializableReactiveProperty<float>(defaultValue);
        }

        public override Observable<float> Observable() => value;

        public override void Send(float newValue)
        {
            // force notify
            value.OnNext(newValue);
        }
    }

    public interface IOutput
    {
        string Name { get; }
    }

    public class Output<T> : IOutput
    {
        public string Name { get; }
        public Observable<T> Observable { get; }

        public Output(string name, Observable<T> observable)
        {
            Name = name;
            Observable = observable;
        }
    }
}
