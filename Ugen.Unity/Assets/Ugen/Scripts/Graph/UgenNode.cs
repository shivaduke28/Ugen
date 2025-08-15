using System.Collections.Generic;
using NUnit.Framework;

namespace Ugen.Graph
{
    public abstract class UgenNode
    {
        readonly List<IUgenInput> inputPorts = new();
        readonly List<IUgenOutput> outputPorts = new();

        public string NodeId { get; }

        public IReadOnlyList<IUgenInput> InputPorts => inputPorts;
        public IReadOnlyList<IUgenOutput> OutputPorts => outputPorts;

        protected UgenNode(string nodeId)
        {
            NodeId = nodeId;
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
