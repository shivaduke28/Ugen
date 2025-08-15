using System;

namespace Ugen.Serialization
{
    public sealed class PortData
    {
        public readonly int Index;
        public readonly string Name;
        public readonly Type ValueType;

        public PortData(int index, string name, Type valueType)
        {
            Index = index;
            Name = name;
            ValueType = valueType;
        }
    }
}
