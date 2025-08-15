using System;

namespace Ugen.Attributes
{
    public sealed class UgenOutputAttribute : Attribute
    {
        public int Index { get; }
        public string Name { get; }

        public UgenOutputAttribute(int index, string name = "")
        {
            Index = index;
            Name = name;
        }
    }
}
