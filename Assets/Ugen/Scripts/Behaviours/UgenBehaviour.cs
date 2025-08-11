using System;
using System.Collections.Generic;
using R3;
using UnityEngine;

namespace Ugen.Behaviours
{
    public abstract class UgenBehaviour : MonoBehaviour
    {
        readonly List<IUgenInput> inputs = new();
        readonly List<IUgenOutput> outputs = new();
        readonly CompositeDisposable disposables = new();

        protected IReadOnlyList<IUgenInput> Inputs => inputs;
        protected IReadOnlyList<IUgenOutput> Outputs => outputs;

        protected void RegisterInput(IUgenInput input)
        {
            inputs.Add(input);
        }

        protected void RegisterOutput(IUgenOutput output)
        {
            outputs.Add(output);
        }

        protected virtual void Awake()
        {
            InitializePorts();
        }

        protected abstract void InitializePorts();

        protected virtual void OnDestroy()
        {
            disposables.Dispose();
        }

        public IUgenInput GetInput(int index)
        {
            if (index < 0 || index >= inputs.Count)
            {
                Debug.LogError($"Input index {index} out of range (0-{inputs.Count - 1}) on {GetType().Name}");
                return null;
            }
            return inputs[index];
        }

        public IUgenOutput GetOutput(int index)
        {
            if (index < 0 || index >= outputs.Count)
            {
                Debug.LogError($"Output index {index} out of range (0-{outputs.Count - 1}) on {GetType().Name}");
                return null;
            }
            return outputs[index];
        }

        public void ConnectTo(int outputIndex, UgenBehaviour target, int inputIndex)
        {
            var output = GetOutput(outputIndex);
            var input = target.GetInput(inputIndex);

            if (output == null || input == null)
            {
                return; // Error already logged in GetInput/GetOutput
            }

            if (output.ValueType != input.ValueType)
            {
                Debug.LogError($"Type mismatch: {output.ValueType} != {input.ValueType}");
                return;
            }

            output.ConnectTo(input, disposables);
        }
    }

    public interface IUgenInput
    {
        string Name { get; }
        Type ValueType { get; }
    }

    public interface IUgenOutput
    {
        string Name { get; }
        Type ValueType { get; }
        void ConnectTo(IUgenInput input, CompositeDisposable disposables);
    }

    public class UgenInput<T> : IUgenInput
    {
        public string Name { get; }
        public Type ValueType => typeof(T);
        public ReactiveProperty<T> Value { get; }

        public UgenInput(string name, T defaultValue = default)
        {
            Name = name;
            Value = new ReactiveProperty<T>(defaultValue);
        }
    }

    public class UgenOutput<T> : IUgenOutput
    {
        public string Name { get; }
        public Type ValueType => typeof(T);
        public ReadOnlyReactiveProperty<T> Value { get; }
        readonly ReactiveProperty<T> source;

        public UgenOutput(string name, T defaultValue = default)
        {
            Name = name;
            source = new ReactiveProperty<T>(defaultValue);
            Value = source.ToReadOnlyReactiveProperty();
        }

        public void SetValue(T value)
        {
            source.Value = value;
        }

        public void ConnectTo(IUgenInput input, CompositeDisposable disposables)
        {
            if (input is UgenInput<T> typedInput)
            {
                Value.Subscribe(v => typedInput.Value.Value = v)
                    .AddTo(disposables);
            }
        }
    }
}
