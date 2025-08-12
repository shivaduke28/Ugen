using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Ugen.Graph
{
    [Serializable]
    public abstract class UgenNode
    {
        [SerializeField] string nodeId;
        [SerializeField] Vector2 position;
        readonly List<IUgenInput> inputPorts = new();
        readonly List<IUgenOutput> outputPorts = new();

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

        public IReadOnlyList<IUgenInput> InputPorts => inputPorts;
        public IReadOnlyList<IUgenOutput> OutputPorts => outputPorts;

        public abstract string NodeName { get; }

        protected UgenNode()
        {
            nodeId = Guid.NewGuid().ToString();
        }

        protected void AddInputPort(IUgenInput input)
        {
            Assert.IsNotNull(input);
            inputPorts.Add(input);
        }

        protected void AddOutputPort(IUgenOutput output)
        {
            Assert.IsNotNull(output);
            outputPorts.Add(output);
        }
    }
}
