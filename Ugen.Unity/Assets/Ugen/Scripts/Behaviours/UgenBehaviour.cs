using System.Collections.Generic;
using R3;
using Ugen.Graph;
using UnityEngine;

namespace Ugen.Behaviours
{
    public abstract class UgenBehaviour : MonoBehaviour
    {
        readonly List<IUgenInput> _inputs = new();
        readonly List<IUgenOutput> _outputs = new();

        public IReadOnlyList<IUgenInput> Inputs => _inputs;
        public IReadOnlyList<IUgenOutput> Outputs => _outputs;

        protected void RegisterInput(IUgenInput input)
        {
            _inputs.Add(input);
        }

        protected void RegisterOutput(IUgenOutput output)
        {
            _outputs.Add(output);
        }

        protected virtual void Awake()
        {
            InitializePorts();
        }

        protected abstract void InitializePorts();

        public IUgenInput GetInput(int index)
        {
            if (index < 0 || index >= _inputs.Count)
            {
                Debug.LogError($"Input index {index} out of range (0-{_inputs.Count - 1}) on {GetType().Name}");
                return null;
            }

            return _inputs[index];
        }

        public IUgenOutput GetOutput(int index)
        {
            if (index < 0 || index >= _outputs.Count)
            {
                Debug.LogError($"Output index {index} out of range (0-{_outputs.Count - 1}) on {GetType().Name}");
                return null;
            }

            return _outputs[index];
        }
    }
}
