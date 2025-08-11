using System;
using UnityEngine;

namespace Ugen.Graph.Nodes
{
    [Serializable]
    public sealed class AddNode : UgenNode
    {
        public override string NodeName => "Add";

        protected override void InitializePorts()
        {
            AddInputPort("a", typeof(float));
            AddInputPort("b", typeof(float));
            AddOutputPort("result", typeof(float));
        }

        // This node doesn't have a corresponding Behaviour
        // The calculation will be done in the graph execution phase
        public float Calculate(float a, float b)
        {
            return a + b;
        }
    }
}