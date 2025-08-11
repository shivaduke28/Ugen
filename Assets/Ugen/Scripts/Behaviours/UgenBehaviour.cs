using System;
using System.Collections.Generic;
using R3;
using Ugen.Graph;
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
    }

}
