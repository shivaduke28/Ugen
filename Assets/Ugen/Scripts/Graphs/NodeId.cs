using System;

namespace Ugen.Graphs
{
    public readonly struct NodeId : IEquatable<NodeId>
    {
        readonly int _value;
        static int _currentValue;

        NodeId(int value)
        {
            _value = value;
        }

        public static NodeId New()
        {
            return new NodeId(_currentValue++);
        }

        public bool Equals(NodeId other)
        {
            return _value == other._value;
        }

        public override bool Equals(object obj)
        {
            return obj is NodeId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return _value;
        }

        public static bool operator ==(NodeId left, NodeId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(NodeId left, NodeId right)
        {
            return !left.Equals(right);
        }
    }
}
