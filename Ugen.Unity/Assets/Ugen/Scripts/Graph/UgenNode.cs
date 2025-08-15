using System.Collections.Generic;
using NUnit.Framework;

namespace Ugen.Graph
{
    public abstract class UgenNode
    {
        readonly List<IUgenInput> _inputPorts = new();
        readonly List<IUgenOutput> _outputPorts = new();

        public string NodeId { get; }

        public IReadOnlyList<IUgenInput> InputPorts => _inputPorts;
        public IReadOnlyList<IUgenOutput> OutputPorts => _outputPorts;

        protected UgenNode(string nodeId)
        {
            NodeId = nodeId;
        }

        protected void AddInputPort(IUgenInput input)
        {
            Assert.IsNotNull(input);
            _inputPorts.Add(input);
        }

        protected void AddOutputPort(IUgenOutput output)
        {
            Assert.IsNotNull(output);
            _outputPorts.Add(output);
        }
    }
}
