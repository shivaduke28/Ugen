using System;

namespace Ugen.Attributes
{
    public sealed class UgenInputAttribute : Attribute
    {
        public int Index { get; }
        public string Name { get; }

        public UgenInputAttribute(int index, string name = "")
        {
            Index = index;
            Name = name;
        }
    }
}
