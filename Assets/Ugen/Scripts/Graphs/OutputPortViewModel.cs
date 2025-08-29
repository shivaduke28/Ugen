using R3;
using UnityEngine;

namespace Ugen.Graphs
{
    public class OutputPortViewModel
    {
        public string Name { get; }
        public ReactiveProperty<Vector2> ConnectorWorldPosition { get; } = new();

        public OutputPortViewModel(string name)
        {
            Name = name;
        }
    }
}
