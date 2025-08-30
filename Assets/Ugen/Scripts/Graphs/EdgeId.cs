using System;

namespace Ugen.Graphs
{
    public readonly struct EdgeId : IEquatable<EdgeId>
    {
        readonly int _value;
        static int _currentValue;

        EdgeId(int value)
        {
            _value = value;
        }

        public static EdgeId New()
        {
            return new EdgeId(_currentValue++);
        }

        public static EdgeId Invalid => new EdgeId(-1);

        public bool Equals(EdgeId other)
        {
            return _value == other._value;
        }

        public override bool Equals(object obj)
        {
            return obj is EdgeId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return _value;
        }

        public static bool operator ==(EdgeId left, EdgeId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(EdgeId left, EdgeId right)
        {
            return !left.Equals(right);
        }
    }
}
