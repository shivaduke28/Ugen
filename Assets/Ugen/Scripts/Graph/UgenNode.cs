using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ugen.Graph
{
    [Serializable]
    public abstract class UgenNode
    {
        [SerializeField] string nodeId;
        [SerializeField] Vector2 position;
        List<UgenPort> inputPorts = new();
        List<UgenPort> outputPorts = new();

        public string NodeId
        {
            get => nodeId;
            set => nodeId = value;
        }

        public Vector2 Position
        {
            get => position;
            set => position = value;
        }

        public IReadOnlyList<UgenPort> InputPorts => inputPorts;
        public IReadOnlyList<UgenPort> OutputPorts => outputPorts;

        public abstract string NodeName { get; }

        protected UgenNode()
        {
            nodeId = Guid.NewGuid().ToString();
            InitializePorts();
        }

        protected abstract void InitializePorts();

        protected void AddInputPort(string name, Type valueType)
        {
            inputPorts.Add(new UgenPort
            {
                Name = name,
                ValueType = valueType,
                Direction = PortDirection.Input,
                Index = inputPorts.Count
            });
        }

        protected void AddOutputPort(string name, Type valueType)
        {
            outputPorts.Add(new UgenPort
            {
                Name = name,
                ValueType = valueType,
                Direction = PortDirection.Output,
                Index = outputPorts.Count
            });
        }
    }

    [Serializable]
    public class UgenPort
    {
        [SerializeField] string name;
        [SerializeField] string valueTypeName;
        [SerializeField] PortDirection direction;
        [SerializeField] int index;

        public string Name
        {
            get => name;
            set => name = value;
        }

        public Type ValueType
        {
            get => Type.GetType(valueTypeName);
            set => valueTypeName = value?.AssemblyQualifiedName;
        }

        public PortDirection Direction
        {
            get => direction;
            set => direction = value;
        }

        public int Index
        {
            get => index;
            set => index = value;
        }
    }

    public enum PortDirection
    {
        Input,
        Output
    }


}
