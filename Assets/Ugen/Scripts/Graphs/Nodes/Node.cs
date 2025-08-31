using System;
using Ugen.Graphs.Ports;

namespace Ugen.Graphs.Nodes
{
    public abstract class Node
    {
        public readonly NodeId Id;
        public readonly string Name;

        public abstract Port[] InputPorts { get; }
        public abstract Port[] OutputPorts { get; }

        protected Node(NodeId id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public sealed class FloatNode : Node
    {
        public FloatNode(NodeId id) : base(id, "Float")
        {
            InputPorts = new[] { new Port("in") };
            OutputPorts = new[] { new Port("out") };
        }

        public override Port[] InputPorts { get; }
        public override Port[] OutputPorts { get; }
    }

    public sealed class Vector3Node : Node
    {
        public Vector3Node(NodeId id) : base(id, "Vector3")
        {
            InputPorts = new[] { new Port("x"), new Port("y"), new Port("z") };
            OutputPorts = new[] { new Port("out") };
        }

        public override Port[] InputPorts { get; }
        public override Port[] OutputPorts { get; }
    }

    public sealed class UpdateNode : Node
    {
        public UpdateNode(NodeId id) : base(id, "Update")
        {
            InputPorts = Array.Empty<Port>();
            OutputPorts = new[] { new Port("out") };
        }

        public override Port[] InputPorts { get; }
        public override Port[] OutputPorts { get; }
    }

    public sealed class AddForceNode : Node
    {
        public AddForceNode(NodeId id) : base(id, "Add Force")
        {
            InputPorts = new[] { new Port("force") };
            OutputPorts = Array.Empty<Port>();
        }

        public override Port[] InputPorts { get; }
        public override Port[] OutputPorts { get; }
    }
}
