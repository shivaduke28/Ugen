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
            inputPorts.Add(new UgenPort(name, valueType, PortDirection.Input, inputPorts.Count));
        }

        protected void AddOutputPort(string name, Type valueType)
        {
            outputPorts.Add(new UgenPort(name, valueType, PortDirection.Output, outputPorts.Count));
        }
    }

    public class UgenPort
    {
        public UgenPort(string name, Type valueType, PortDirection direction, int index)
        {
            Name = name;
            ValueType = valueType;
            Direction = direction;
            Index = index;
        }

        public string Name { get; }
        public Type ValueType { get; }
        public PortDirection Direction { get; }
        public int Index { get; }
    }

    public enum PortDirection
    {
        Input,
        Output
    }


}
