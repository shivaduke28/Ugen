using System;
using UnityEngine;

namespace Ugen.Graphs
{
    public readonly struct GraphTransform : IEquatable<GraphTransform>
    {
        public static readonly GraphTransform Default = new(Vector2.zero, 1f);

        public readonly Vector2 Position;
        public readonly float Zoom;

        public GraphTransform(Vector2 position, float zoom)
        {
            Position = position;
            Zoom = zoom;
        }

        public bool Equals(GraphTransform other)
        {
            return Position.Equals(other.Position) && Zoom.Equals(other.Zoom);
        }

        public override bool Equals(object obj)
        {
            return obj is GraphTransform other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Position, Zoom);
        }

        public static bool operator ==(GraphTransform left, GraphTransform right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GraphTransform left, GraphTransform right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return $"Position: {Position}, Zoom: {Zoom}";
        }
    }
}
