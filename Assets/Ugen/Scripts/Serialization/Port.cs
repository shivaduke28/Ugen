using System;

namespace Ugen.Serialization
{
    public sealed class Port
    {
        public readonly int Index;
        public readonly string Name;
        public readonly Type Type;

        public Port(int index, string name, Type type)
        {
            Index = index;
            Name = name;
            Type = type;
        }
    }
}
