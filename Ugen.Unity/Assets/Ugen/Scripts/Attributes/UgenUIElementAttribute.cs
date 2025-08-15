using System;

namespace Ugen.Attributes
{
    public sealed class UgenUIElementAttribute : Attribute
    {
        public string Name { get; }

        public UgenUIElementAttribute(string name = "")
        {
            Name = name;
        }
    }
}
