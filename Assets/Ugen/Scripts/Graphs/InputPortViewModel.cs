using R3;
using UnityEngine;

namespace Ugen.Graphs
{
    public class InputPortViewModel
    {
        public string Name { get; }
        public ReactiveProperty<Vector2> ConnectorWorldPosition { get; } = new();

        public InputPortViewModel(string name)
        {
            Name = name;
        }
    }
}
