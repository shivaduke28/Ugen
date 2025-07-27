using System;
using Ugen.Behaviours;
using UnityEngine;

namespace Ugen.Graphs
{
    public enum UgenValueType
    {
        None = 0,
        Unit = 1,
        Float = 2,
    }

    [Serializable]
    public sealed class UgenBehaviourRef
    {
        [SerializeField] string id;
        [SerializeField] string name;
        [SerializeField] UgenComponentRef[] components;

        public string Id => id;
        public string Name => name;
        public UgenComponentRef[] Components => components;

        public UgenBehaviourRef(string id, string name, UgenComponentRef[] components)
        {
            this.id = id;
            this.name = name;
            this.components = components;
        }
    }

    [Serializable]
    public sealed class UgenComponentRef
    {
        [SerializeField] string name;
        [SerializeField] InputRef[] inputs;
        [SerializeField] OutputRef[] outputs;

        public string Name => name;
        public InputRef[] Inputs => inputs;
        public OutputRef[] Outputs => outputs;

        public UgenComponentRef(string name, InputRef[] inputs, OutputRef[] outputs)
        {
            this.name = name;
            this.inputs = inputs;
            this.outputs = outputs;
        }
    }

    [Serializable]
    public sealed class InputRef
    {
        [SerializeField] string name;
        [SerializeField] UgenValueType type;

        public string Name => name;
        public UgenValueType Type => type;

        public InputRef(string name, UgenValueType type)
        {
            this.name = name;
            this.type = type;
        }
    }

    [Serializable]
    public sealed class OutputRef
    {
        [SerializeField] string name;
        [SerializeField] UgenValueType type;

        public string Name => name;
        public UgenValueType Type => type;

        public OutputRef(string name, UgenValueType type)
        {
            this.name = name;
            this.type = type;
        }
    }

    public static class UgenBehaviourConverter
    {
        public static UgenBehaviourRef Convert(Behaviours.UgenBehaviour ugenBehaviour)
        {
            var components = new UgenComponentRef[ugenBehaviour.Components.Length];
            for (var i = 0; i < components.Length; i++)
            {
                components[i] = Convert(ugenBehaviour.Components[i]);
            }

            return new UgenBehaviourRef(ugenBehaviour.Id, ugenBehaviour.Name, components);
        }

        public static UgenComponentRef Convert(UgenComponent component)
        {
            var inputs = new InputRef[component.Inputs.Length];
            for (var i = 0; i < inputs.Length; i++)
            {
                inputs[i] = Convert(component.Inputs[i]);
            }

            var outputs = new OutputRef[component.Outputs.Length];
            for (var i = 0; i < outputs.Length; i++)
            {
                outputs[i] = Convert(component.Outputs[i]);
            }

            return new UgenComponentRef(component.Name, inputs, outputs);
        }

        public static InputRef Convert(IInput input)
        {
            var valueType = GetValueType(input);
            return new InputRef(input.Name, valueType);
        }

        public static OutputRef Convert(IOutput output)
        {
            var valueType = GetValueType(output);
            return new OutputRef(output.Name, valueType);
        }

        static UgenValueType GetValueType(IInput input)
        {
            return input switch
            {
                Input<float> => UgenValueType.Float,
                _ => UgenValueType.None
            };
        }

        static UgenValueType GetValueType(IOutput output)
        {
            return output switch
            {
                Output<float> => UgenValueType.Float,
                _ => UgenValueType.None
            };
        }
    }
}
